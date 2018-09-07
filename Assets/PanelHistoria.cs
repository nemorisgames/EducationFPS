using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using UnityEngine.SceneManagement;

public class PanelHistoria : MonoBehaviour {
	UIPanel panel;
	UISprite imageSprite;
	UILabel textLabel;
	UIWidget container;
    public GameObject cargando;

	[System.Serializable]
	public struct Pantalla{
		public string img;
		public string txt;
	}

	public Pantalla [] pantallas;
	public int index;
	public string loadLevel;
	bool visible;
	TweenAlpha imageTween;
	TypewriterEffect typeWriter;
	AsyncOperation loadMain;
	AsyncOperation loadAdditive;

	void Awake(){
		panel = GetComponent<UIPanel>();
		container = transform.Find("Container").GetComponent<UIWidget>();
		imageSprite = container.transform.Find("imagen").GetComponent<UISprite>();
		textLabel = container.transform.Find("texto").GetComponent<UILabel>();
		imageTween = imageSprite.GetComponent<TweenAlpha>();
		typeWriter = textLabel.GetComponent<TypewriterEffect>();
	}

	void Start () {
		panel.alpha = 1;
		index = -1;
		NextScreen();
		container.alpha = 0;
		container.GetComponent<TweenAlpha>().PlayForward();
		visible = true;
		loadMain = SceneManager.LoadSceneAsync(loadLevel);
		loadMain.allowSceneActivation = false;
		if(loadLevel == "City"){
			loadAdditive = SceneManager.LoadSceneAsync(loadLevel+"Models",LoadSceneMode.Additive);
			loadAdditive.allowSceneActivation = false;
		}
		loadMain.priority = 0;
		if(loadAdditive != null)
			loadAdditive.priority = 1;
	}


	public void NextScreen(){
		if(changingScreen)
			return;
		if(writingText){
			typeWriter.Finish();
			return;
		}
		index++;
		if(index >= pantallas.Length){
			if(index == pantallas.Length){
                cargando.SetActive(true);
				loadMain.allowSceneActivation = true;
				if(loadAdditive != null)
					loadAdditive.allowSceneActivation = true;
			}
		}
		else{
			StartCoroutine(changeImage(index));
		}
	}

	IEnumerator AdditiveLoad(){
		while(loadMain.progress < 0.9 && loadAdditive.progress < 0.9){
			yield return null;
		}
		loadMain.allowSceneActivation = true;
		loadAdditive.allowSceneActivation = true;
	}

	void Update(){
		if(visible && Input.GetKeyDown(KeyCode.Q))
			NextScreen();
		
		if(loadMain != null && loadAdditive != null)
			Debug.Log(loadMain.progress +" | "+ loadAdditive.progress);
	}

	public bool changingScreen = false;
	public bool writingText = false;
	IEnumerator changeImage(int index){
		changingScreen = true;
		imageTween.PlayForward();
		yield return new WaitForSeconds(0.5f);
		imageSprite.spriteName = pantallas[index].img;
		textLabel.text = pantallas[index].txt;
		typeWriter.ResetToBeginning();
		writingText = true;
		imageTween.PlayReverse();
		yield return new WaitForSeconds(0.5f);
		changingScreen = false;
	}

	public void TextDisplayed(){
		writingText = false;
	}

}
