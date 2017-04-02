using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilController : MonoBehaviour
{
    private GameObject LockObject;

    void Start()
    {
        LockObject = null;
    }

    void Update()
    {
        //if (LockObject)
        //{
        //    if (LockObject.GetComponent<HammerTime>().isMalleable == false)
        //    {
        //        DropObject();
        //    }
        //}

        if (LockObject == null)
        {
            DropObject();
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (LockObject == null && col.transform.parent == null && col.tag == "ForgeItem" &&
            (col.GetComponent<HammerTime>().isMalleable || col.GetComponent<HammerTime>().isBrittle))
        {
            PickupObject(col.gameObject);
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (LockObject == null && col.transform.parent == null && col.tag == "ForgeItem" &&
            (col.GetComponent<HammerTime>().isMalleable || col.GetComponent<HammerTime>().isBrittle))
        {
            PickupObject(col.gameObject);
        }
    }
    public void Show()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }
    void Hide()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
    }

    void PickupObject(GameObject obj)
    {
        //Hide();

        LockObject = obj;
        LockObject.layer = 0;
        LockObject.GetComponent<Rigidbody>().useGravity = false;
        LockObject.GetComponent<Rigidbody>().isKinematic = true;
        LockObject.transform.parent = this.transform;
        LockObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        LockObject.transform.rotation = new Quaternion();
        LockObject.GetComponent<HammerTime>().OnAnvil = true;
    }
    void DropObject()
    {
        //Show();

        //LockObject.GetComponent<Rigidbody>().useGravity = true;
        //LockObject.GetComponent<Rigidbody>().isKinematic = false;
        //LockObject.transform.parent = null;
        //LockObject.GetComponent<HammerTime>().OnAnvil = false;
        //LockObject = null;
    }
}
