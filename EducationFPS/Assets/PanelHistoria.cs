using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

public class PanelHistoria : MonoBehaviour {
	UIPanel panel;
	UISprite imageSprite;
	UILabel textLabel;
	UIWidget container;

	[System.Serializable]
	public struct Pantalla{
		public string img;
		public string txt;
	}

	public Pantalla [] pantallas;
	public int index;
	bool visible;
	GameObject player;

	void Awake(){
		panel = GetComponent<UIPanel>();
		container = transform.Find("Container").GetComponent<UIWidget>();
		imageSprite = container.transform.Find("imagen").GetComponent<UISprite>();
		textLabel = container.transform.Find("texto").GetComponent<UILabel>();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	void Start () {
		player.SetActive(false);
		panel.alpha = 1;
		index = -1;
		NextScreen();
		container.alpha = 0;
		container.GetComponent<TweenAlpha>().PlayForward();
		visible = true;
	}

	public void NextScreen(){
		index++;
		if(index >= pantallas.Length){
			if(index == pantallas.Length)
				player.SetActive(true);
				GetComponent<TweenAlpha>().PlayForward();
		}
		else{
			imageSprite.spriteName = pantallas[index].img;
			textLabel.text = pantallas[index].txt;
		}
	}

	void Update(){
		if(visible && Input.GetKeyDown(KeyCode.Q))
			NextScreen();
	}
}
