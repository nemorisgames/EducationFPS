using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelResumen : MonoBehaviour {
	UIPanel panel;
	UILabel tiempoLabel, puntajeLabel, rCorrectasLabel, rEquivocadasLabel, totalLabel;
	public float tiempo, total;
	public int puntaje, rCorrectas, rEquivocadas;
	public UILabel [] debug;
	public bool showDebug = false;
	public GameObject debugObject;
	Vector3 initialPos;
	Quaternion initialRot;
	GameObject player;
    public PlayMakerFSM fsm;
	AsyncOperation loadNext;

    // Use this for initialization
    void Awake () {
		tiempoLabel = transform.Find("background/tiempo").GetComponent<UILabel>();
		puntajeLabel = transform.Find("background/puntaje").GetComponent<UILabel>();
		rCorrectasLabel = transform.Find("background/rCorrectas").GetComponent<UILabel>();
		rEquivocadasLabel = transform.Find("background/rEquivocadas").GetComponent<UILabel>();
		totalLabel = transform.Find("background/total").GetComponent<UILabel>();
		panel = GetComponent<UIPanel>();
		panel.alpha = 0;
		player = GameObject.FindGameObjectWithTag("Player");
		initialPos = player.transform.position;
		initialRot = player.transform.rotation;
	}

	void Start(){
		loadNext = SceneManager.LoadSceneAsync("Story");
		loadNext.allowSceneActivation = false;
	}

	void Update(){
		//if(Input.GetKeyDown(KeyCode.Space))
		//	ObtenerTotal();
		tiempo = Time.timeSinceLevelLoad;
		debug[1].text = FormatoTiempo((int)tiempo);
		if(debugObject.activeInHierarchy && !showDebug || !debugObject.activeInHierarchy && showDebug)
			debugObject.SetActive(!debugObject.activeInHierarchy);

		if(panel.alpha != 0){
			if(Input.GetKeyDown(KeyCode.Return))
				restart();
			if(Input.GetKeyDown(KeyCode.Escape))
				exitGame();
		}
	}

	public void EnemigoDestruido(){
		puntaje += 1;
		debug[0].text = puntaje.ToString();
	}

	public void Respuesta(bool correcta){
		if(correcta){
			rCorrectas += 1;
			debug[2].text = rCorrectas.ToString();
		}
		else{
			rEquivocadas += 1;
			debug[3].text = rEquivocadas.ToString();
			//ReturnPlayer();
		} 
	}

	public void ObtenerTotal(){
		puntajeLabel.text = puntaje.ToString();
		tiempoLabel.text = FormatoTiempo((int)tiempo);
		rCorrectasLabel.text = rCorrectas.ToString();
		rEquivocadasLabel.text = rEquivocadas.ToString();
		GetComponent<TweenAlpha>().PlayForward();

        fsm.SendEvent("EndLevel");
    }

	public string FormatoTiempo(float tiempo){
		return ((Mathf.FloorToInt(tiempo/60f)).ToString()+":"+(tiempo%60f < 10 ? "0" : "")+(tiempo%60f).ToString());
	}

    public void exitGame()
    {
        Application.Quit();
    }

    public void restart()
    {
		string name = SceneManager.GetActiveScene().name;
        if(name == "City")
			PlayerPrefs.SetInt("Escena",1);
		else if(name == "Exterior 2")
			PlayerPrefs.SetInt("Escena",2);
		else if(name == "Tunnel")
			PlayerPrefs.SetInt("Escena",3);
		else if(name == "Interior")
			PlayerPrefs.SetInt("Escena",4);
		else
			PlayerPrefs.SetInt("Escena",0);
		loadNext.allowSceneActivation = true;
    }

	public void ReturnPlayer(){
		player.transform.position = initialPos;
		player.transform.rotation = initialRot;
	}
}
