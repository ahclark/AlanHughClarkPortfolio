using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreBucket : MonoBehaviour
{
    [SerializeField]
    MineShaft cart;
    [SerializeField]
    GameObject bucket;
    Vector3 snapPos;

	// Use this for initialization
	void Start ()
    {
        snapPos = bucket.transform.position;
	}

    void OnTriggerStay(Collider entity)
    {
        if (entity.name == "OreBucket")
        {
            bucket.transform.position = snapPos;
            cart.inPlace = true;
        }
    }

}
