using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ForgeHeat : MonoBehaviour {

    public bool Heated = false;
    float timer = 0;
    float lerpTimer;
    bool start = false;
    [SerializeField]
    public Material StartMat;
    [SerializeField]
    Material EndMat;
    [SerializeField]
    Color startColor;
    [SerializeField]
    Color endColor;
    [SerializeField]
    MeshRenderer[] Mats;
    [SerializeField]
    Color DullOrange;
   void Start()
    {
        if(StartMat)
        startColor = StartMat.color;
        if(EndMat)
        endColor = EndMat.color;
        for (int i = 0; i < Mats.Length; i++)
        {
            Mats[i].material = new Material(StartMat);
            Mats[i].material.color = startColor;
        }
    }
	
    void FixedUpdate()
    {
        if (start)
        {
            timer += Time.fixedDeltaTime;
            lerpTimer += Time.fixedDeltaTime * 0.2f;
            for (int i = 0; i < Mats.Length; i++)
            {
                Mats[i].material.color = Color.Lerp(startColor, DullOrange, lerpTimer);
            }
        }
        if (timer >= 5.0f)
        {
            for (int i = 0; i < Mats.Length; i++)
            {
                Mats[i].material.color = endColor;
            }
            timer = 0;
            Heated = true;
            start = false;
            ReAttachToHand();
        }


    }

    void OnTriggerStay(Collider col)
    {
        if (col.name == "Forge" && !Heated)
        {
            start = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Forge" && Heated == false)
        {
            start = false;
            timer = 0;
            for (int i = 0; i < Mats.Length; i++)
            {
                Mats[i].material.color = startColor;
            }

        }
    }


    void ReAttachToHand()
    {
        if (gameObject.tag == "Puck")
            gameObject.tag = "HeatedPuck";




        if(gameObject.transform.parent)
        {
            Hand theHand = gameObject.transform.parent.GetComponent<Hand>();
            if(theHand)
            {
                Throwable thrower = gameObject.GetComponent<Throwable>();
                theHand.DetachObject(gameObject);
                if(thrower)
                theHand.AttachObject(gameObject, thrower.attachmentFlags);
            }
        }
    }
	

    public void ImCoolNow()
    {
        if(gameObject.tag == "HeatedPuck")
        {
            gameObject.tag = "Puck";
        }
    }

}
