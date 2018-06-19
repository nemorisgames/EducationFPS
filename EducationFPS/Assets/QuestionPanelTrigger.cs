using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPanelTrigger : MonoBehaviour {
	QuestionPanel qp;

	// Use this for initialization
	void Start () {
		qp = GetComponentInParent<QuestionPanel>();
	}

	private void OnTriggerEnter(Collider other){
		if(other.tag == "Player")
			qp.EnablePanel(true);
	}

	private void OnTriggerExit(Collider other){
		if(other.tag == "Player")
			qp.EnablePanel(false);
	}
}
