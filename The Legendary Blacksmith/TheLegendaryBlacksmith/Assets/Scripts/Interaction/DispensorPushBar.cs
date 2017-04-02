using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispensorPushBar : MonoBehaviour
{
    [SerializeField]
    Vector3 pivot;
    Rigidbody skeleton;
    [SerializeField]
    DispenserController dispenser;

	// Use this for initialization
	void Start ()
    {
        skeleton = GetComponent<Rigidbody>();
        skeleton.centerOfMass = pivot;
	}

    void OnTriggerEnter(Collider entity)
    {
        if (entity.name == "BarTrig")
        {
            dispenser.SpawnPuck();
        }
    } 
}
