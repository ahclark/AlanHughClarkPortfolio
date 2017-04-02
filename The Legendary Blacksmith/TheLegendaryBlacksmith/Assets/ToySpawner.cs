using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToySpawner : MonoBehaviour {
    public int type = 0;
	// Use this for initialization
	void Start () {
        //Tutorials.tutorialinstance.Tutorial(gameObject);
        DayNightCycle.dayInstance.StartDay();
	}
	
    public void StopTutorial()
    {
        Tutorials.tutorialinstance.StopUnitTutorial();
    }
}
