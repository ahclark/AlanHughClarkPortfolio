using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonControl : MonoBehaviour 
{
    [SerializeField]
    float distance = 1000.0f;
    [SerializeField]
    float scale = 15.0f;
	// Use this for initialization
	void Start () 
	{
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - distance);
        transform.localScale = new Vector3(scale, scale, scale);
	}
}
