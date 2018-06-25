using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelResumen : MonoBehaviour {
	UILabel tiempoLabel, puntajeLabel, rCorrectasLabel, rEquivocadasLabel, totalLabel;
	public float tiempo, total;
	public int puntaje, rCorrectas, rEquivocadas;

	// Use this for initialization
	void Awake () {
		tiempoLabel = transform.Find("tiempo").GetComponent<UILabel>();
		puntajeLabel = transform.Find("puntaje").GetComponent<UILabel>();
		rCorrectasLabel = transform.Find("rCorrectas").GetComponent<UILabel>();
		rEquivocadasLabel = transform.Find("rEquivocadas").GetComponent<UILabel>();
		totalLabel = transform.Find("total").GetComponent<UILabel>();
		GetComponent<UIPanel>().alpha = 0;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space))
			ObtenerTotal();
	}

	public void SetValues(float tiempo, int puntaje, int rCorrectas, int rEquivocadas){
		this.tiempo = tiempo;
		this.puntaje = puntaje;
		this.rCorrectas = rCorrectas;
		this.rEquivocadas = rEquivocadas;
	}

	void ObtenerTotal(){
		GetComponent<TweenAlpha>().PlayForward();
		tiempoLabel.text = ((Mathf.FloorToInt(tiempo/60f)).ToString()+":"+(tiempo%60f < 10 ? "0" : "")+(tiempo%60f).ToString());
	}
}
