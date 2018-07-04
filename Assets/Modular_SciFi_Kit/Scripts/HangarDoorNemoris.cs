using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarDoorNemoris : MonoBehaviour
{
    public bool doorLocked;
    public GameObject doorFrame;

    public bool doorCanClose = false;
    public bool doorCanOpen = true;

    private void Start()
    {
        if (doorLocked) {
            Renderer renderer = doorFrame.GetComponent<Renderer>();
            Material mat = renderer.material;
            Color finalColor = new Color(3f, 0f, 0f);
            mat.SetColor("_EmissionColor", finalColor);
        }
    }

    public void LockDoors()
    {
        Renderer renderer = doorFrame.GetComponent<Renderer>();
        Material mat = renderer.material;
        Color finalColor = new Color(3f, 0f, 0f);
        mat.SetColor("_EmissionColor", finalColor);
        doorLocked = true;
    }

    public void UnlockDoors()
    {
        Renderer renderer = doorFrame.GetComponent<Renderer>();
        Material mat = renderer.material;
        Color finalColor = new Color(0f, 3f, 0f);
        mat.SetColor("_EmissionColor", finalColor);
        doorLocked = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!doorLocked && other.tag == "Player") 
        {
            StartCoroutine(OpenDoor());
        }
        else if(other.tag == "Arrow")
        {
            other.GetComponent<Rigidbody>().useGravity = true;
            other.transform.Find("Collider").gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player")
            return;
        if (!doorLocked && !doorCanOpen)
        {
            doorCanClose = true;
        }
    }

    void OnTriggerStay(Collider other){
        if(other.tag != "Player" || doorLocked)
            return;
        if(doorCanOpen)
            StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        while (!doorCanOpen)
        {
            yield return null;
        }

        doorCanOpen = false;

        GetComponent<Animation>().Play("HangarDoor1Open");
        yield return new WaitForSeconds(1);
            
        StartCoroutine(CloseDoor());
    }

    IEnumerator CloseDoor()
    {
        while (!doorCanClose)
        {
            yield return null;
        }

        GetComponent<Animation>().Play("HangarDoor1Close");
        yield return new WaitForSeconds(1);

        doorCanClose = false;
        doorCanOpen = true;
    }

}
