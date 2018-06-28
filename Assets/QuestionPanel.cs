using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	bool usingPanel = false;
	int selectedToggle = 0;
    public TweenScale mainPanelTwScale;
	public bool preguntaAlAzar = true;
	public int dificultadAlAzar;

	// Use this for initialization
	void Start () {
        puertaAsociadaNemoris = puertaAsociada.gameObject.GetComponent<HangarDoorNemoris>();
        posicionPreguntas = transform.Find("AnswersAnchor");
		enunciadoLabel = transform.Find("Question").GetComponent<UILabel>();
		//puerta cerrada por defecto
		puertaAsociada.doorLocked = true;
        puertaAsociadaNemoris.LockDoors();
		//si se espera una pregunta al azar o algun parametro clave esta vacio, obtiene pregunta al azar
		if(preguntaAlAzar || pregunta.enunciado == "" || pregunta.alternativas == null)
			pregunta = GameObject.FindObjectOfType<QuestionManager>().getPregunta(dificultadAlAzar);
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
		enunciadoLabel.text = (pregunta.enunciado == "" ? "enunciado" : pregunta.enunciado);
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
            toggles[i].transform.Find("Label").GetComponent<UILabel>().text = (pregunta.alternativas == null || pregunta.alternativas.Length == 0 ? "default" : pregunta.alternativas[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(usingPanel && puertaAsociada.doorLocked){
			if(Input.GetKeyDown(KeyCode.Alpha1) && toggles.Length > 0){
				toggles[0].value = true;
				selectedToggle = 0;
			}
			if(Input.GetKeyDown(KeyCode.Alpha2) && toggles.Length > 1){
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
				EvaluarRespuesta(selectedToggle);
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
            bool activate = !enunciadoLabel.gameObject.activeSelf;
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

	void EvaluarRespuesta(int respuesta){
		if(respuesta == pregunta.respuestaCorrecta){
			Debug.Log("Correcto");
            puertaAsociadaNemoris.UnlockDoors();
        }
		else{
			Debug.Log("Incorrecto");
			
		}
	}
}
