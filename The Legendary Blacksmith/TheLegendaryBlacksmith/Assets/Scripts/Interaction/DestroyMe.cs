using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class DestroyMe : MonoBehaviour
{
    [SerializeField]
    GameObject sacrificialObject;

	// Use this for initialization
	void Start ()
    {
        if (!sacrificialObject)
            sacrificialObject = gameObject;
	}

    public void DoItSissy()
    {
        if (sacrificialObject.transform.parent)
        {
            sacrificialObject.transform.parent.gameObject.GetComponent<Hand>().DetachObject(sacrificialObject);
        }
        //sacrificialObject.SetActive(false);
        Destroy(sacrificialObject);
    }
}
