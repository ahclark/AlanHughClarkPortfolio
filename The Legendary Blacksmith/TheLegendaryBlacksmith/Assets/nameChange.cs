using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nameChange : MonoBehaviour {
    [SerializeField]
    collideEnableDestroy CED;
    bool changed = false;
	
	// Update is called once per frame
	void Update () {
		if (CED.Enabled && !changed)
        {
            changed = true;
            gameObject.name = "RepairedScythe";
        }
	}
}
