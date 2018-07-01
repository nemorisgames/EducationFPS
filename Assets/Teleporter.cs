using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {
	public List<EnemyDestroyed> guards;
	bool active = false;
	public PanelResumen panelResumen;

    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;

    // Use this for initialization
    void Awake () {
		if(guards != null && guards.Count > 0)
			foreach(EnemyDestroyed g in guards)
				g.teleporter = this;
		if(panelResumen == null)
			panelResumen = GameObject.FindObjectOfType<PanelResumen>();
	}

	public void CheckActive(){
        if (guards.Count == 0)
        {
            active = true;
            foreach (GameObject g in objectsToActivate)
            {
                g.SetActive(true);
            }
            foreach (GameObject g in objectsToDeactivate)
            {
                g.SetActive(false);
            }
        }
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
