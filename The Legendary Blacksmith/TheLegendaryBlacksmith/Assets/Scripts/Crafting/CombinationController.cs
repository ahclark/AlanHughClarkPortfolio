using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CombinationController : MonoBehaviour
{
    private int numAttached = 0;
    [SerializeField]
    GameObject[] Attachers;
    [SerializeField]
    GameObject FullObject;

    public void CheckforFull()
    {
        numAttached++;
        if (numAttached >= Attachers.Length)
        {
            if (this.gameObject.transform.parent)
                this.gameObject.transform.parent.GetComponent<Hand>().DetachObject(this.gameObject);

            GameObject newObject = Instantiate(FullObject);
            newObject.name = FullObject.name;
            newObject.transform.position = this.transform.position;
            newObject.transform.rotation = this.transform.rotation;
            if (DialogueManager.dialogueInstance)
                DialogueManager.dialogueInstance.ActivateNewWeapon(newObject.name);
            //this.gameObject.transform.parent.GetComponent<Hand>().AttachObject(newObject);

            //Destroy(this.gameObject);
            //Debug.Log("Before Hide");
            this.gameObject.SetActive(false);
            //Debug.Log("After Hide");
        }
    }
}
