using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenController : MonoBehaviour
{
    public GameObject[] wood;
    public int woodCount = 0;

	// Use this for initialization
	void Start ()
    {
		
	}
	
    public void IncreaseCount()
    {
        woodCount++;
        if(woodCount >= wood.Length)
        {
            woodCount = 0;
        }
    }
}
