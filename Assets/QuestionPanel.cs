using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class QuestionPanel : MonoBehaviour {
	public HangarDoor puertaAsociada;
    HangarDoorNemoris puertaAsociadaNemoris;
	public QuestionManager.Pregunta pregunta;
    //public string enunciado;
	//public string [] preguntas;
	//public int respuestaCorrecta = 0;
	UIToggle [] toggles;
	public GameObject alternativaPrefab;
	Transform posicionPreguntas;
	UILabel enunciadoLabel;
	public bool usingPanel = false;
	int selectedToggle = 0;
    public TweenScale mainPanelTwScale;
	public bool preguntaAlAzar = true;
	private int dificultadAlAzar;
	public int idPregunta;
	PanelResumen resumen;
    public PlayMakerFSM fsm;
    bool correctAnswerSelected = false;
    public GameObject correctMessage;
    public GameObject incorrectMessage;
    public AudioClip [] audioClips;
    AudioSource audioSource;
    EventDelegate toggleSelect;

    void Awake () {
        puertaAsociadaNemoris = puertaAsociada.gameObject.GetComponent<HangarDoorNemoris>();
        posicionPreguntas = transform.Find("AnswersAnchor");
		enunciadoLabel = transform.Find("Question").GetComponent<UILabel>();
		//puerta cerrada por defecto
		puertaAsociada.doorLocked = true;
        puertaAsociadaNemoris.LockDoors();
		resumen = FindObjectOfType<PanelResumen>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
	}

	public void CargarPanel(){
        //Debug.Log(gameObject.name);
		//si se espera una pregunta al azar o algun parametro clave esta vacio, obtiene pregunta al azar
		if(preguntaAlAzar)
			pregunta = GameObject.FindObjectOfType<QuestionManager>().getPregunta();
		else
			pregunta = GameObject.FindObjectOfType<QuestionManager>().getPreguntaId(idPregunta);
		//trunca el arreglo de preguntas a 4 si hay mas
		if(pregunta.alternativas.Length > 4){
			string [] aux = new string[4];
			for(int i = 0; i < aux.Length; i++)
				aux[i] = pregunta.alternativas[i];
			pregunta.alternativas = aux;
		}
		//rellena con 3 preguntas si no hay ninguna asignada
		int nPreguntas = (pregunta.alternativas == null || pregunta.alternativas.Length == 0 ? 3 : pregunta.alternativas.Length);
		//escribe enunciado
		if(enunciadoLabel != null) enunciadoLabel.text = (pregunta.enunciado == "" ? "enunciado" : pregunta.enunciado);
		//crea lista de toggles correspondientes
		toggles = new UIToggle[nPreguntas];
        ConstructAlternatives();
	}

    //construye las alternativas desde prefab, asigna toggles, escribe texto
    void ConstructAlternatives()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            GameObject go = (GameObject)Instantiate(alternativaPrefab, posicionPreguntas.transform.position, Quaternion.identity, this.transform);
            go.transform.localRotation = posicionPreguntas.transform.localRotation;
            go.transform.position = posicionPreguntas.position - posicionPreguntas.up * i * 0.12f;

            toggles[i] = go.GetComponent<UIToggle>();
            EventDelegate.Add(toggles[i].onChange,() => PlayAudioSelect());
            toggles[i].transform.Find("Label").GetComponent<UILabel>().text = (pregunta.alternativas == null || pregunta.alternativas.Length == 0 ? "default" : pregunta.alternativas[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (correctAnswerSelected) return;

		if(usingPanel && puertaAsociadaNemoris.doorLocked){
			if(Input.GetKeyDown(KeyCode.Alpha1) && toggles.Length > 0){
				toggles[0].value = true;
				selectedToggle = 0;

            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && toggles.Length > 1){
				toggles[1].value = true;
				selectedToggle = 1;
			}
				
			if(Input.GetKeyDown(KeyCode.Alpha3) && toggles.Length > 2){
				toggles[2].value = true;
				selectedToggle = 2;
			}

			if(Input.GetKeyDown(KeyCode.Alpha4) && toggles.Length > 3){
				toggles[3].value = true;
				selectedToggle = 3;
			}

			if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
				StartCoroutine(EvaluarRespuesta(selectedToggle));
		}
	}

	public void EnablePanel(bool b){
		usingPanel = b;
        if (b){
            mainPanelTwScale.PlayForward();
        }
		else{
            toggles[selectedToggle].value = false;
            enunciadoLabel.gameObject.SetActive(false);
            foreach (UIToggle to in toggles)
            {
                to.gameObject.SetActive(false);
            }
            mainPanelTwScale.PlayReverse();
        }
	}

    //sólo se ejecuta cuando la animacion va hacia adelante
    public void ShowQuestionAndResponses()
    {
        if (mainPanelTwScale.direction == AnimationOrTween.Direction.Forward)
        {
            bool activate = !enunciadoLabel.gameObject.activeSelf && !correctAnswerSelected;
            enunciadoLabel.gameObject.SetActive(activate);
            enunciadoLabel.text = pregunta.enunciado;
            enunciadoLabel.GetComponent<TypewriterEffect>().ResetToBeginning();
            for (int i = 0; i < toggles.Length; i++)
            {
                toggles[i].gameObject.SetActive(activate);
                toggles[i].transform.Find("Label").GetComponent<UILabel>().text = pregunta.alternativas[i];
                toggles[i].transform.Find("Label").GetComponent<TypewriterEffect>().ResetToBeginning();
            }
        }
    }

	IEnumerator EvaluarRespuesta(int respuesta){
        PlayAudioAnswer(respuesta == pregunta.respuestaCorrecta);
        if (respuesta == pregunta.respuestaCorrecta)
        {
            correctAnswerSelected = true;
            correctMessage.SetActive(true);
            Debug.Log("Correcto");
            puertaAsociadaNemoris.UnlockDoors();
            enunciadoLabel.gameObject.SetActive(false);
            resumen.Respuesta(true);
            showAlternatives(false);
        }
        else
        {
            Debug.Log("Incorrecto");
            resumen.Respuesta(false);
            fsm.SendEvent("IncorrectAnswer");
            enunciadoLabel.gameObject.SetActive(false);
            showAlternatives(false);
            incorrectMessage.SetActive(true);
            yield return new WaitForSeconds(2f);
            showAlternatives(true);
            incorrectMessage.SetActive(false);
        }
	}

    void showAlternatives(bool show)
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].gameObject.SetActive(show);
        }
    }

    void PlayAudioSelect(){
        audioSource.Stop();
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    void PlayAudioAnswer(bool b){
        audioSource.Stop();
        audioSource.clip = (b ? audioClips[1] : audioClips[2]);
        audioSource.Play();
    }
}
