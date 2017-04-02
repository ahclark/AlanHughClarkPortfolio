using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ArrowHead : MonoBehaviour
{
    //parent with arrow making script
    [SerializeField]
    ArrowMaker arrow;
    
    //in-sync with both scripts
    [SerializeField]
    ArrowHead tip;
    //in-sync with both scripts
    [SerializeField]
    ArrowHead butt;
    //transform holder
    [SerializeField]
    GameObject scrapReference;
    //scrap input holder
    public GameObject scrap;
    //can be sharpened
    public bool ready = false;
    //can have items placed in its collider
    bool canAcceptTribute = true;

    //starts sharpening the arrow
    bool sharpenMe = false;
    [SerializeField]
    float timer = 5f;
    
    //distinguish which collider this is
    public bool isTheTip = false;

    private Hand hand = null;

    bool first = false;
    PencilSharpener pencil;


    void Start ()
    {
        tip.isTheTip = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (ready)
        {
            scrap.transform.localPosition = new Vector3(0, 0, 0);
            scrap.transform.localRotation = scrapReference.transform.localRotation;
        }
        if (sharpenMe)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime * 1;

                // If the arrow is being held, Vibrate the controller
                // Written By: Alan Clark
                if (this.transform.parent.parent.gameObject.layer == 11)
                {
                    this.transform.parent.parent.GetComponent<Hand>().controller.TriggerHapticPulse(500);
                }
                // End of Snippet ///////////////////////////////////
            }
            else
            {
                ready = false;
                Destroy(scrap);
                arrow.justTheTip.SetActive(true);
                arrow.tipOn = true;
                arrow.MakeTheArrow();
                sharpenMe = false;                
                if (pencil)
                    pencil.ActivateSharpening(false);
            }
        }
	}

    void OnTriggerEnter(Collider entity)
    {
        if (canAcceptTribute)
        {
            if (entity.name == "Arrow_Feathers" && butt.canAcceptTribute)
            {
                arrow.birdsOfAFeather.SetActive(true);
                if (isTheTip)
                {
                    DoTheFlip();
                }
                GetRidOfIt(entity.gameObject);
                butt.canAcceptTribute = false;
                arrow.buttOn = true;
                arrow.MakeTheArrow();
            }
            if (entity.name == "Scrap" && tip.canAcceptTribute)
            {
                if (!isTheTip)
                {
                    DoTheFlip();
                }

                tip.scrap = entity.gameObject;
                if (tip.scrap.transform.parent)
                {
                    tip.scrap.transform.parent.gameObject.GetComponent<Hand>().DetachObject(tip.scrap);
                }
                tip.scrap.GetComponent<MeshCollider>().enabled = false;
                tip.scrap.GetComponent<Rigidbody>().isKinematic = true;
                tip.scrap.transform.parent = tip.transform;
                tip.scrap.name = "Sharpen";
                tip.ready = true;
                tip.canAcceptTribute = false;
                
            }
        }
    }

    void OnTriggerStay(Collider entity)
    {
        if (ready && entity.name == "SharpenTrig")
        {
            pencil = entity.GetComponent<PencilSharpener>();
            if (pencil && !first)
            {
                pencil.ActivateSharpening(true);
                first = true;
            }
            sharpenMe = true;
        }
    }

    void OnTriggerExit(Collider entity)
    {
        if (entity.name == "SharpenTrig")
        {
            sharpenMe = false;
            first = false;
        }
    } 

    void DoTheFlip()
    {
        arrow.gameObject.transform.position = arrow.flip.transform.position;
        arrow.gameObject.transform.rotation = arrow.flip.transform.rotation;
    } 

    void GetRidOfIt(GameObject entity)
    {
        if (entity.transform.parent)
        {
            entity.transform.parent.gameObject.GetComponent<Hand>().DetachObject(entity.gameObject);
        }
        entity.gameObject.SetActive(false);
        Destroy(entity);
    }
}
