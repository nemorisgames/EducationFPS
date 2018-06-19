using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestionPanel : MonoBehaviour {
	public HangarDoor puertaAsociada;
	public string enunciado;
	public string [] preguntas;
	public int respuestaCorrecta = 0;
	UIToggle [] toggles;
	public GameObject alternativaPrefab;
	Transform posicionPreguntas;
	UILabel enunciadoLabel;
	GameObject cubierta;
	bool usingPanel = false;
	int selectedToggle = 0;
	public bool usarCubierta;

	// Use this for initialization
	void Start () {
		posicionPreguntas = transform.Find("posicionPreguntas");
		enunciadoLabel = transform.Find("Enunciado").GetComponent<UILabel>();
		cubierta = transform.Find("cubierta").gameObject;
		//puerta cerrada por defecto
		puertaAsociada.doorLocked = true;
		if(!usarCubierta)
			cubierta.SetActive(false);
		//trunca el arreglo de preguntas a 4 si hay mas
		if(preguntas.Length > 4){
			string [] aux = new string[4];
			for(int i = 0; i < aux.Length; i++)
				aux[i] = preguntas[i];
			preguntas = aux;
		}
		//rellena con 3 preguntas si no hay ninguna asignada
		int nPreguntas = (preguntas == null || preguntas.Length == 0 ? 3 : preguntas.Length);
		//escribe enunciado
		enunciadoLabel.text = (enunciado == "" ? "enunciado" : enunciado);
		//crea lista de toggles correspondientes
		toggles = new UIToggle[nPreguntas];
		//construye las alternativas desde prefab, asigna toggles, escribe texto
		for(int i = 0; i < nPreguntas; i++)
		{
			GameObject go = (GameObject)Instantiate(alternativaPrefab,posicionPreguntas.transform.position,Quaternion.identity,this.transform);
			go.transform.localPosition = new Vector3(posicionPreguntas.localPosition.x,posicionPreguntas.localPosition.y - i*50f,posicionPreguntas.localPosition.z);
			go.transform.localRotation = Quaternion.identity;

			toggles[i] = go.GetComponent<UIToggle>();

			toggles[i].transform.Find("Label").GetComponent<UILabel>().text = (preguntas == null || preguntas.Length == 0 ? "default" : preguntas[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(usingPanel && puertaAsociada.doorLocked){
			if(Input.GetKeyDown("1") && toggles.Length > 0){
				toggles[0].value = true;
				selectedToggle = 0;
			}
			if(Input.GetKeyDown("2") && toggles.Length > 1){
				toggles[1].value = true;
				selectedToggle = 1;
			}
				
			if(Input.GetKeyDown("3") && toggles.Length > 2){
				toggles[2].value = true;
				selectedToggle = 2;
			}

			if(Input.GetKeyDown("4") && toggles.Length > 3){
				toggles[3].value = true;
				selectedToggle = 3;
			}

			if(Input.GetKeyDown("q"))
				EvaluarRespuesta(selectedToggle);
		}
	}

	public void EnablePanel(bool b){
		usingPanel = b;
		if(b){
			if(usarCubierta) cubierta.SetActive(false);
		}
		else{
			toggles[selectedToggle].value = false;
			if(puertaAsociada.doorLocked && usarCubierta)
				cubierta.SetActive(true);
		}
	}

	void EvaluarRespuesta(int respuesta){
		if(respuesta == respuestaCorrecta){
			Debug.Log("Correcto");
			puertaAsociada.doorLocked = false;
		}
		else{
			Debug.Log("Incorrecto");
			
		}
	}
}
