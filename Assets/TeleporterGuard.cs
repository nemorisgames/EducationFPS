using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterGuard : MonoBehaviour {
	public Teleporter teleporter;

	void OnDestroy(){
		if(teleporter == null)
			teleporter = FindObjectOfType<Teleporter>();
		teleporter.guards.Remove(this);
		teleporter.CheckActive();
	}
}
