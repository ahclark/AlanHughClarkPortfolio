using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WoodGrabbyThing : MonoBehaviour
{
    [SerializeField]
    WoodShaving activation;
    [SerializeField]
    GameObject wood;
    [SerializeField]
    GameObject display;
    public bool accepting = false;

	// Use this for initialization
	void Start ()
    {
        display.SetActive(true);
        activation.ShaveWood();
	}

    private void OnTriggerEnter(Collider entity)
    {
        if (entity.tag == "Wood" && entity.name == "Wood" && accepting)
        {
            wood = entity.gameObject;
            
            if (wood.transform.parent)
            {
                Hand temporary = wood.transform.parent.gameObject.GetComponent<Hand>();
                if (temporary)
                {
                    temporary.DetachObject(wood);
                }
            }
            TurnOffWood(wood);
            TurnOnWood(display);
            activation.ShaveWood();
            accepting = false;
        }
        if (entity.name == "Wood_Hilt" && accepting)
        {
            wood = entity.gameObject;
            if (wood.transform.parent)
            {
                Hand temporary = wood.transform.parent.gameObject.GetComponent<Hand>();
                if (temporary)
                {
                    temporary.DetachObject(wood);
                }
            }
            activation.ShaveWood();
            activation.PutHandleBack(entity.gameObject);
            accepting = false;
        }
    }

    public void TurnOffWood(GameObject entity)
    {
        //foreach(MeshRenderer picture in entity.transform)
        //{
        //    picture.enabled = false;
        //}
        entity.gameObject.SetActive(false);
    }
    public void TurnOnWood(GameObject entity)
    {
        //foreach(MeshRenderer picture in entity.transform)
        //{
        //    picture.enabled = true;
        //}
        entity.gameObject.SetActive(true);
    }
}
