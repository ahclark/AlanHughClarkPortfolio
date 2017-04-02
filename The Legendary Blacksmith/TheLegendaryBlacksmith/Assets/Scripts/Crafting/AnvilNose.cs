using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilNose : MonoBehaviour
{

    private void OnTriggerEnter(Collider entity)
    {
        if (entity.GetComponent<ScrapBar>())
        {
            entity.GetComponent<ScrapBar>().CanBeHammered = true;
        }
    }
    private void OnTriggerExit(Collider entity)
    {
        if (entity.GetComponent<ScrapBar>())
        {
            entity.GetComponent<ScrapBar>().CanBeHammered = false;
        }
    }
}
