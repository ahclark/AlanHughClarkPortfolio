using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class AttacherController : MonoBehaviour
{
    public string Name;
    public GameObject Representative;
    public bool attaching = false;
    [SerializeField]
    GameObject tutorial;
    [SerializeField]
    Material hiddenMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (!attaching)
            Do(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!attaching)
            Do(other);
    }

    private void Do(Collider other)
    {
        if (other.name == Name/* && other.transform.parent == null*/)
        {
            if (other.transform.parent)
            {
                other.transform.parent.GetComponent<Hand>().DetachObject(other.gameObject);
            }
            if (this.transform.parent)
            {
                attaching = true;
                //Destroy(other.gameObject);
                other.gameObject.SetActive(false);
                GameObject newObject = Instantiate(Representative);
                newObject.transform.parent = this.transform.parent;
                newObject.transform.localPosition = Vector3.zero;
                newObject.transform.localRotation = Quaternion.identity;
                if (tutorial)
                    tutorial.GetComponent<MeshRenderer>().material = hiddenMaterial;
                SendMessageUpwards("CheckforFull");
            }
        }
    }

    public void ActivateTutorial()
    {
        if (!attaching)
            tutorial.SetActive(!tutorial.activeInHierarchy);
    }
}
