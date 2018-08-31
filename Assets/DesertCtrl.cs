using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertCtrl : MonoBehaviour {
	public static DesertCtrl Instance {get{return _instance;}}
	static DesertCtrl _instance;
	DesertHouse [] desertHouse;
	public int roomCount;

	// Use this for initialization
	void Awake () {
		_instance = this;
	}

	void Start(){
		desertHouse = FindObjectsOfType<DesertHouse>();
		for(int i = 0; i < desertHouse.Length; i++)
			desertHouse[i].index = i;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EnteredRoom(int i){
		roomCount++;
		if(roomCount == desertHouse.Length){
			Debug.Log("Show note");
		}
	}
}
