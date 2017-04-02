using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WorkOrderController : MonoBehaviour {


    public bool isComplete = false;
    string objectName;
    [SerializeField]
    GameObject Icon;
    [SerializeField]
    Transform IconTrans;
    [SerializeField]
    Material completeMaterial;


    public void NewWorkOrder(GameObject yourIcon, string yourObjectName)
    {
        objectName = yourObjectName;
       GameObject toChange = Instantiate(yourIcon, Icon.transform);
        if(toChange && IconTrans)
        {
            toChange.transform.position = Icon.transform.position;
        }
     
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.name == objectName)
        {
            CompleteOrder();
            if (other.gameObject.transform.parent)
            {
                Hand thehand = other.gameObject.transform.parent.gameObject.GetComponent<Hand>();
                if (thehand)
                    thehand.DetachObject(other.gameObject);
            }
            other.gameObject.SetActive(false);
        }
    }

    void CompleteOrder()
    {
        isComplete = true;
        MeshRenderer meshRend = Icon.GetComponentInChildren<MeshRenderer>();
        if(meshRend)
        {
            meshRend.material = completeMaterial;
        }
    }
}
