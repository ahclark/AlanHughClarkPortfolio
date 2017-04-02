using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
public class MainMenuBounds : MonoBehaviour 
{
    [SerializeField]
    LayerMask itemstoCatch;
    [SerializeField]
    Vector3 spawnPos;

    private void OnTriggerExit(Collider other)
    {
        if (itemstoCatch == (itemstoCatch & 1 << other.gameObject.layer))
        {
        Transform temp = other.transform.parent;
        while(temp != null)
        {
            if (temp.gameObject.GetComponent<Hand>() || temp.gameObject.GetComponent<AudioListener>())
                return;
            temp = temp.transform.parent;
        }
            if (other.transform.parent == null)
            {
                other.transform.position = spawnPos;
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
            {
                other.transform.parent.position = spawnPos;
                other.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;

            }
        }
    }
}
