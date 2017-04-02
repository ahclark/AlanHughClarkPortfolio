using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LogSliceKiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider entity)
    {
        if (entity.name == "LogSlice")
        {
            
            if (entity.transform.parent)
            {
                Hand temporary = entity.transform.parent.gameObject.GetComponent<Hand>();
                if (temporary)
                {
                    temporary.DetachObject(entity.gameObject);
                }
            }
            entity.gameObject.SetActive(false);
        }
    } 
}
