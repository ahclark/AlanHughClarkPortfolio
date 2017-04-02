using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTriggerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        //this.GetComponent<Rigidbody>().SendMessage("OnTriggerEnter", collision);
        this.transform.parent.transform.parent.GetComponent<Rigidbody>().SendMessage("OnTriggerEnter", collision);
    }
}
