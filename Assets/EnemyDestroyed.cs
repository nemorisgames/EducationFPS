using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyed: MonoBehaviour {
	public bool isTeleporterGuard = false;
	public Teleporter teleporter;
	public PanelResumen resumen;

	void Awake(){
		if(isTeleporterGuard && teleporter == null)
			teleporter = FindObjectOfType<Teleporter>();
		resumen = FindObjectOfType<PanelResumen>();
	}

	void OnDestroy(){
		if(isTeleporterGuard && teleporter != null){
			teleporter.guards.Remove(this);
			teleporter.CheckActive();
		}
		if(resumen != null)
			resumen.EnemigoDestruido();
	}
}
