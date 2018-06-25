using UnityEngine;
using System.Collections;

public class InheritedGrenade : vp_Grenade {
	private vp_Timer.Handle Timer = new vp_Timer.Handle();
	private Rigidbody rb;

	void OnDestroy(){
		//Debug.Log("Destroyed");
		Timer.Cancel();
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}
	void Start () {
		Timer = getTimer();
		rb = GetComponent<Rigidbody>();
	}
}
