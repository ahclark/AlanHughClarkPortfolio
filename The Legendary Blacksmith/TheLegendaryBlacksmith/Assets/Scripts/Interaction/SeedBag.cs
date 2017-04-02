using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SeedBag : MonoBehaviour
{

    [SerializeField]
    Hand TheHand;
    [SerializeField]
    GameObject seed;

    // Use this for initialization
    void Start()
    {
        TheHand = null;
    }
    public void GetSeed()
    {
        if (TheHand)
        {
            GameObject newObject = Instantiate(seed);
            newObject.name = "Seed";
            TheHand.AttachObject(newObject);
        }
    }
    private void OnHandHoverBegin(Hand hand)
    {
        TheHand = hand;
    }

    private void OnHandHoverEnd(Hand hand)
    {
        TheHand = null;
    }
}
