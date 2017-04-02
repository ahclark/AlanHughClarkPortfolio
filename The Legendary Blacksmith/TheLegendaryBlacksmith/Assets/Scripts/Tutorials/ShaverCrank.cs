using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShaverCrank : MonoBehaviour
{
    [SerializeField]
    CircularDrive mine;
    [SerializeField]
    float spin = 1;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        mine.outAngle += Time.deltaTime * spin;
        mine.UpdateValues();
	}
}
