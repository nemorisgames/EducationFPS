using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickArrow : MonoBehaviour {
	bool collided = false;
	Transform arrow;

	// Use this for initialization
	void Awake () {
		arrow = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(!collided){
			Debug.Log(other.transform.parent+"/"+other.transform);
			if(other.tag == "Arrow" || other.tag == "Player"){
				Physics.IgnoreCollision(other,GetComponent<Collider>(),true);
				return;
			}
			collided = true;
			arrow.parent = other.transform;
		}
		
	}
}
