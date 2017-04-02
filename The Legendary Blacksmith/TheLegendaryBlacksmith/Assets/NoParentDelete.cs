using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoParentDelete : MonoBehaviour {

    GameObject test;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        test = transform.parent.gameObject;
        if (!test)
        {
            Destroy(gameObject);
        }
	}
}
