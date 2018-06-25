using UnityEngine;
using System.Collections;
 
public class visibilityArrow : MonoBehaviour {
 
    public GameObject arrow;
    vp_FPPlayerEventHandler m_Player;
    void Awake(){
        m_Player = this.GetComponentInParent<vp_FPPlayerEventHandler> ();

    }
 
    void OnEnable(){
        ArrowOn ();
        if (m_Player != null)
            m_Player.Register (this);
    }
    void OnDisable(){
        ArrowOff ();
        if (m_Player != null)
            m_Player.Unregister (this);
    }
    public void ArrowOn(){
        if(arrow!=null)
            arrow.SetActive (true);
    }
    public void ArrowOff(){
        if(arrow!=null)
            arrow.SetActive (false);
    }
    protected void OnAttempt_Fire(){
        ArrowOff ();
        Debug.Log ("Fired the Arrow, so Arrow Off");
    }
}
