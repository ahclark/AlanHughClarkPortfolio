using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPuck : MonoBehaviour
{
    [SerializeField]
    GameObject[] segments;
    [SerializeField]
    GameObject holder;
    [SerializeField]
    GameObject one;
    [SerializeField]
    GameObject two;
    [SerializeField]
    GameObject three;
    [SerializeField]
    GameObject four;
    [SerializeField]
    GameObject five;
    [SerializeField]
    Vector3[] desiredScale;
    [SerializeField]
    Vector3 desiredPosition;
    public bool onAnvil;
    [SerializeField]
    SwordDownLerp[] SDLs;
    [SerializeField]
    public int[] counters;


    void Start()
    {
        
    }
    public void SlamSegment(string segment)
    {
        //foreach (GameObject piece in segments)
        //{
        //    piece.transform.parent = null;
        //    if (segment == piece.name)
        //    {
        //        piece.transform.localScale = desiredScale;
        //    }
        //}
        //for(int i = segment.Length-1; i > 0; i--)
        //{
        //    segments[i].transform.parent = segments[i - 1].transform;
        //    segments[i].transform.localPosition = desiredPosition;
        //}
        one.transform.parent = null;
        two.transform.parent = null;
        three.transform.parent = null;
        four.transform.parent = null;
        five.transform.parent = null;
        if (segment == "one")
        {
            one.transform.localScale = desiredScale[counters[0]];
            if (counters[0] < 3)
            {
                SDLs[0].Start();
                SDLs[0].startLerpin = true;
            }
        }
        if (segment == "two")
        {
            two.transform.localScale = desiredScale[counters[1]];
            if (counters[1] < 3)
            {
                SDLs[1].Start();
                SDLs[1].startLerpin = true;
            }
        }
        if (segment == "three")
        {
            three.transform.localScale = desiredScale[counters[2]];
            if (counters[2] < 3)
            {
                SDLs[2].Start();
                SDLs[2].startLerpin = true;
            }
        }
        if (segment == "four")
        {
            four.transform.localScale = desiredScale[counters[3]];
            if (counters[3] < 3)
            {
                SDLs[3].Start();
                SDLs[3].startLerpin = true;
            }
        }
        if (segment == "five")
        {
            five.transform.localScale = desiredScale[counters[4]];
            if (counters[4] < 3)
            {
                SDLs[4].Start();
                SDLs[4].startLerpin = true;
            }
        }
        five.transform.parent = four.transform;
        five.transform.localPosition = desiredPosition;
        four.transform.parent = three.transform;
        four.transform.localPosition = desiredPosition;
        three.transform.parent = two.transform;
        three.transform.localPosition = desiredPosition;
        two.transform.parent = one.transform;
        two.transform.localPosition = desiredPosition;
        one.transform.parent = holder.transform;
        one.transform.localPosition = desiredPosition;

    }
    
    void OnTriggerStay(Collider col)
    {
        if (col.name == "anvil")
        {
            onAnvil = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "anvil")
        {
            onAnvil = false;
        }
    }
}
