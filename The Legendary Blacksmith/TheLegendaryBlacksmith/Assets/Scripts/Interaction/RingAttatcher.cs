using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class RingAttatcher : MonoBehaviour
{
    bool host = false;
    [SerializeField]
    ShieldRingStuff ring;
    [SerializeField]
    GameObject child;

    void OnTriggerEnter(Collider entity)
    {
        if (!host && entity.name == "LogSlice")
        {
            if (entity.transform.parent)
            {
                entity.transform.parent.gameObject.GetComponent<Hand>().DetachObject(entity.gameObject);
            }
            //child.transform.position = entity.transform.position;
            //child.transform.rotation = entity.transform.rotation;
            child.transform.parent = entity.transform;
            ring.shield = entity.GetComponent<ShieldMaker>();
            //child.transform.localPosition = new Vector3(0, 0, 0);
            //child.transform.localScale = new Vector3(.51f, .51f, .51f);
            //child.transform.localRotation = new Quaternion (-90,0,0,1);
            ring.ResetPosition();
            entity.gameObject.name = "Shield_Unrimmed";
            
            
            
            ring.canDoTheThing = true;
            host = true;
            if (transform.parent)
            {
                transform.parent.gameObject.GetComponent<Hand>().DetachObject(gameObject);
            }
            Destroy(gameObject);
        }

    }
}
