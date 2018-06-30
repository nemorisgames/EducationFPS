using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelResumen : MonoBehaviour {
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

    // Use this for initialization
    void Awake () {
		tiempoLabel = transform.Find("tiempo").GetComponent<UILabel>();
		puntajeLabel = transform.Find("puntaje").GetComponent<UILabel>();
		rCorrectasLabel = transform.Find("rCorrectas").GetComponent<UILabel>();
		rEquivocadasLabel = transform.Find("rEquivocadas").GetComponent<UILabel>();
		totalLabel = transform.Find("total").GetComponent<UILabel>();
		GetComponent<UIPanel>().alpha = 0;
		player = GameObject.FindGameObjectWithTag("Player");
		initialPos = player.transform.position;
		initialRot = player.transform.rotation;
	}

	void Update(){
		//if(Input.GetKeyDown(KeyCode.Space))
		//	ObtenerTotal();
		tiempo = Time.timeSinceLevelLoad;
		debug[1].text = FormatoTiempo((int)tiempo);
		if(debugObject.activeInHierarchy && !showDebug || !debugObject.activeInHierarchy && showDebug)
			debugObject.SetActive(!debugObject.activeInHierarchy);
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
        Application.LoadLevel("Exterior");
    }

	public void ReturnPlayer(){
		player.transform.position = initialPos;
		player.transform.rotation = initialRot;
	}
}
