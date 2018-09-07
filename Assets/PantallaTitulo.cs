using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantallaTitulo : MonoBehaviour {
	AsyncOperation loadScene;
	bool loadingScene = false;

	// Use this for initialization
	void Start () {
		loadScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Story");
		loadScene.allowSceneActivation = false;
		loadingScene = false;
	}
	
	public void NewGame(){
		if(loadingScene)
			return;
		loadingScene = true;
		PlayerPrefs.SetInt("Escena",0);
		loadScene.allowSceneActivation = true;
	}

	public void Continue(){
		if(loadingScene)
			return;
		loadingScene = true;
		loadScene.allowSceneActivation = true;
	}

	public void ExitGame(){
		Application.Quit();
	}
}
