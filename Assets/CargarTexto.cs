using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargarTexto : MonoBehaviour {
	public int index;
	public Escena [] escenas;
	PanelHistoria panelHistoria;

	// Use this for initialization
	void Awake () {
		panelHistoria = GetComponent<PanelHistoria>();
		int escena = PlayerPrefs.GetInt("Escena",0);
		escena = Mathf.Clamp(escena,0,escenas.Length);

		//hardcodear escena a cargar
		if(index > -1)
			escena = index;

		panelHistoria.pantallas = escenas[escena].pantallas;

		switch(escena){
			case 0:
			panelHistoria.loadLevel = "City";
			break;
			case 1:
			panelHistoria.loadLevel = "Exterior 2";
			break;
			case 2:
			panelHistoria.loadLevel = "Tunnel";
			break;
			case 3:
			panelHistoria.loadLevel = "Inside";
			break;
			case 4:
			panelHistoria.loadLevel = "Title";
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[System.Serializable]
	public struct Escena{
		public PanelHistoria.Pantalla [] pantallas;
	}
}
