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
	TweenAlpha imageTween;

	void Awake(){
		panel = GetComponent<UIPanel>();
		container = transform.Find("Container").GetComponent<UIWidget>();
		imageSprite = container.transform.Find("imagen").GetComponent<UISprite>();
		textLabel = container.transform.Find("texto").GetComponent<UILabel>();
		imageTween = imageSprite.GetComponent<TweenAlpha>();
	}
	void Start () {
		panel.alpha = 1;
		index = -1;
		NextScreen();
		container.alpha = 0;
		container.GetComponent<TweenAlpha>().PlayForward();
		visible = true;
	}

	public void NextScreen(){
		if(changingScreen)
			return;
		index++;
		if(index >= pantallas.Length){
			if(index == pantallas.Length)
				Debug.Log("end");
				//GetComponent<TweenAlpha>().PlayForward();
		}
		else{
			StartCoroutine(changeImage(index));
		}
	}

	void Update(){
		if(visible && Input.GetKeyDown(KeyCode.Q))
			NextScreen();
	}

	bool changingScreen = false;
	IEnumerator changeImage(int index){
		changingScreen = true;
		imageTween.PlayForward();
		yield return new WaitForSeconds(0.5f);
		imageSprite.spriteName = pantallas[index].img;
		textLabel.text = pantallas[index].txt;
		imageTween.PlayReverse();
		yield return new WaitForSeconds(0.5f);
		changingScreen = false;
	}
}
