using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeController : MonoBehaviour
{
    Tutorials tutorial;
    private void Start()
    {
        tutorial = GameObject.FindObjectOfType<Tutorials>();
    }
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "ForgeItem")
        {
            HammerTime hTime = col.GetComponent<HammerTime>();
            if(hTime)
            hTime.inForge = true;
            tutorial.StopForgeTutorial();
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "ForgeItem")
        {
            HammerTime hTime = col.GetComponent<HammerTime>();
            if (hTime)
                hTime.inForge = false;
        }
    }
}
