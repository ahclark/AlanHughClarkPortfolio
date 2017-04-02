using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(CircularDrive))]
public class CircularDriveEvents : MonoBehaviour {


    public delegate void OnCircularMoveDelegate(float degrees, int direction);
    public event OnCircularMoveDelegate OnCircularMove;



    LinearMapping linearMap;
    CircularDrive cDrive;
    float storedValue = 0;
    float storedDegrees = 0;
    int currDir = 0;
    // Use this for initialization
    void Start()
    {
        if (cDrive == null)
        {
            cDrive = GetComponent<CircularDrive>();
            if (cDrive == null)
            {
                cDrive = gameObject.AddComponent<CircularDrive>();
            }
        }
        if (linearMap == null && cDrive)
        {
            linearMap = cDrive.linearMapping;
            if (linearMap == null)
            {
                linearMap = gameObject.AddComponent<LinearMapping>();
            }
            if(linearMap)
                storedValue = linearMap.value;
            
        }

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (linearMap)
        {
            if (linearMap.value != storedValue)
            {
                currDir = CheckDirection();
                float degree = GetDegrees();
                if (OnCircularMove != null)
                    OnCircularMove(degree, currDir);

                storedValue = linearMap.value;
            }
            else
            {
                currDir = 0;
            }
        }
    }



    int CheckDirection()
    {
        if (storedValue > linearMap.value)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    float GetDegrees()
    {
        if (cDrive)
        {
            if (cDrive.limited)
            {
                return Mathf.Lerp(cDrive.minAngle, cDrive.maxAngle, linearMap.value);
            }
            else
            {
                return Mathf.Lerp(0, 360, linearMap.value);
            }
        }
        return -1;
    }
}
