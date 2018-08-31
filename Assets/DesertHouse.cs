using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertHouse : MonoBehaviour {
	GameObject houseObjects;
	public int index;
	public bool enabledObjects = false;

	// Use this for initialization
	void Start () {
		houseObjects = transform.Find("Objects").gameObject;
		if(houseObjects.activeInHierarchy)
			houseObjects.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void EnableObjects(bool b){
		houseObjects.SetActive(b);
		enabledObjects = b;
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Player" && !enabledObjects){
			EnableObjects(true);
			DesertCtrl.Instance.EnteredRoom(index);
		}
	}
}
