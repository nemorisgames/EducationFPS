using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    bool showingMenu = false;
    public TweenAlpha menuPanel;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1f;
	}

    public void Resume()
    {
        menuPanel.PlayReverse();
        showingMenu = false;
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
            /*
            if (showingMenu)
            {
                Time.timeScale = 1f;
                menuPanel.PlayReverse();
                showingMenu = false;
            }
            else
            {
                Time.timeScale = 0f;
                menuPanel.PlayForward();
                showingMenu = true;
            }*/
        }
	}
}
