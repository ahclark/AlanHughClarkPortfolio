using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Character_Net : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if(!isClient)
        {
            gameObject.SetActive(false);
        }
	}
	

}
