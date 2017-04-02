using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GlassesTrigger : MonoBehaviour
{
    private GameObject LockObject;

    void Lock(Collider other)
    {
        if (!LockObject && other.tag == "Glasses")
        {
            LockObject = other.gameObject;
            if (LockObject.transform.parent/*.gameObject.layer == 11 /* "Hand" Layer */)
                LockObject.transform.parent.GetComponent<Hand>().DetachObject(LockObject);
            LockObject.transform.parent = this.transform;
            LockObject.transform.localPosition = Vector3.zero;
            LockObject.transform.localRotation = Quaternion.identity;
            LockObject.GetComponent<Rigidbody>().useGravity = false;
            LockObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Unlock(Collider other)
    {
        if (LockObject && other.tag == "Glasses")
        {
            //LockObject.transform.parent = null;
            if (LockObject.transform.parent/*.gameObject.layer == 11 /* "Hand" Layer */)
                LockObject.transform.parent.GetComponent<Hand>().DetachObject(LockObject);
            LockObject.GetComponent<Rigidbody>().useGravity = true;
            LockObject.GetComponent<Rigidbody>().isKinematic = false;
            LockObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Lock(other);
    }

    private void OnTriggerStay(Collider other)
    {
        Lock(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Unlock(other);
    }
}
