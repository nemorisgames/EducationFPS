using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {
	public List<TeleporterGuard> guards;
	bool active = false;
	public PanelResumen panelResumen;

	// Use this for initialization
	void Awake () {
		foreach(TeleporterGuard g in guards)
			g.teleporter = this;
		if(panelResumen == null)
			panelResumen = GameObject.FindObjectOfType<PanelResumen>();
	}

	public void CheckActive(){
		if(guards.Count == 0)
			active = true;
	}

	void OnTriggerStay(Collider c){
		if(c.tag == "Player" && active){
			//do stuff
			if(panelResumen != null) panelResumen.ObtenerTotal();
			active = false;
		}
		else
			return;
	}
}
