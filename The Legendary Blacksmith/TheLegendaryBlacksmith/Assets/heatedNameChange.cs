using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class heatedNameChange : MonoBehaviour {
    ForgeHeat FH;
    Hand testHand;
    bool once = false;
	// Use this for initialization
	void Start () {
        FH = gameObject.GetComponent<ForgeHeat>();	
	}
	
	// Update is called once per frame
	void Update () {
		if (FH.Heated && !once)
        {
            gameObject.name = "HeatedScrapBar";
            once = true;
        }
	}
    
    public void DestroyScrap()
    {
        if (gameObject.transform.parent)
        {
            testHand = gameObject.transform.parent.gameObject.GetComponent<Hand>();
            if (testHand)
            {
                testHand.DetachObject(gameObject);
            }
        }
        gameObject.SetActive(false);
    }
}
