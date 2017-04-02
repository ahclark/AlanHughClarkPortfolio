using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimate : MonoBehaviour
{
    Animator myAnimator;

	// Use this for initialization
	void Start ()
    {
        myAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            
            myAnimator.Play("Salute");
        }
	}


}
