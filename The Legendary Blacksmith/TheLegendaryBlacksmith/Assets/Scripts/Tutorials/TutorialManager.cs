using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialObjects;
    public TutorialMovement[] tutorialScripts;
    public bool glasses = false;

	// Use this for initialization
	void Start ()
    {
		
	}

    public void GlassesOn()
    {
        glasses = true;
    }

    public void GlassesOff()
    {
        glasses = false;
    }
}
