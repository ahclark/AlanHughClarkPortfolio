using UnityEngine;
using System.Collections;

public class BagofHolding : MonoBehaviour
{

    public GameObject bag;
    public GameObject otherBag;

    public void Activate()
    {
        if (otherBag)
        {
            objectPlacement[] chil = otherBag.GetComponentsInChildren<objectPlacement>();
            foreach (objectPlacement obj in chil)
                obj.ActivateText(false);
            otherBag.SetActive(false);
        }
        if (bag)
        {
            if (bag.activeInHierarchy)
            {
                objectPlacement[] chil = bag.GetComponentsInChildren<objectPlacement>();
                foreach (objectPlacement obj in chil)
                    obj.ActivateText(!bag.activeInHierarchy);
            }
            bag.SetActive(!bag.activeInHierarchy);
        }
    }
    public void SetObject(int index)
    {
        if (GameObject.Find(bag.name))
        {
            Destroy(GameObject.Find(bag.name));
        }
        if (bag.GetComponent<StructurePlacing>())
        {
            Destroy(GameObject.Find(bag.GetComponent<StructurePlacing>().RealObject.name));
        }
        GameObject new_object = Instantiate<GameObject>(bag);
        //if (Camera.main.gameObject.GetComponent<SteamVR_TrackedObject>())
        //{
        //    if (bag.GetComponent<StructurePlacing>())
        //    {
        //        SteamVR_Controller.Device dev = GameObject.FindObjectOfType<VRInputModule>().GetComponent<VRInputModule>().Hands[
        //            GameObject.FindObjectOfType<VRInputModule>().GetComponent<VRInputModule>().GetCurrentController()].
        //            GetComponent<InteractionController>().GetDevice();
        //        new_object.GetComponent<StructurePlacing>().device = dev;
        //    }
        //    if (bag.transform.tag == "Selectable")
        //    {
        //        SteamVR_Controller.Device dev = GameObject.FindObjectOfType<VRInputModule>().GetComponent<VRInputModule>().Hands[
        //            GameObject.FindObjectOfType<VRInputModule>().GetComponent<VRInputModule>().GetCurrentController()].
        //            GetComponent<InteractionController>().GetDevice();
        //        Transform deviceTrans = null;
        //        SteamVR_TrackedObject[] trackedControllers = FindObjectsOfType<SteamVR_TrackedObject>();
        //        foreach (SteamVR_TrackedObject tracked in trackedControllers)
        //        {
        //            if (((int)tracked.index) == dev.index)
        //            {
        //                deviceTrans = tracked.transform;
        //            }
        //        }
        //        deviceTrans.gameObject.GetComponent<InteractionController>().SetObject(new_object.transform);
        //    }
        //}

        new_object.name = bag.name;
        transform.parent.parent.parent.parent.transform.gameObject.SetActive(false);
    }



}
