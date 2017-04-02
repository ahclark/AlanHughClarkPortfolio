using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ChickenFeed : MonoBehaviour
{
    [SerializeField]
    ChickenController chicken;
    [SerializeField]
    bool eating = false;

    // Use this for initialization
    void Start ()
    {
		
	}

    private void EatSomething()
    {
        eating = true;
        chicken.FeedChicken();
        eating = false;
    }

    void OnTriggerEnter(Collider entity)
    {
        if (!eating && entity.name == "Seed")
        {
            chicken.seed = entity.gameObject;
            EatSomething();
        }
    }

    void OnTriggerStay(Collider entity)
    {
        if (!eating && entity.name == "Seed")
        {
            chicken.seed = entity.gameObject;
            EatSomething();
        }
    }
}
