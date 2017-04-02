using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCount : MonoBehaviour {
    public int cubes = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (cubes >= 44)
        {
            gameObject.name = "Prefab_Blade";
        }
	}
}
