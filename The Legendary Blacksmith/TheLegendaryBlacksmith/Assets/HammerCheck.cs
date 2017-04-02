using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerCheck : MonoBehaviour
{
    public bool HammerReady = false;


    void OnTriggerExit(Collider col)
    {
        if (col.name == "Head")
        {
            HammerReady = true;
        }
    }
}