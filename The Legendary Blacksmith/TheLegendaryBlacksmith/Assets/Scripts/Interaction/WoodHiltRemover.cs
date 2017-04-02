using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class WoodHiltRemover : MonoBehaviour
{
    [SerializeField]
    bool grindMe = false;
    [SerializeField]
    float timer = 5f;
    [SerializeField]
    GameObject end;
    [SerializeField]
    GameObject start;
    [SerializeField]
    GameObject wood;
    float startTime;
    [SerializeField]
    float speed = 1;
    Hand tempHand;
    [SerializeField]
    UnityEvent AtEnd;

    AudioSource audio;
    bool firstActivate = false;

    [SerializeField]
    bool gobbyTutorial = false;


    void Start()
    {
        startTime = timer;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (grindMe)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime * speed;

               //// If the arrow is being held, Vibrate the controller
               //// Written By: Alan Clark
               //if (this.transform.parent.parent.gameObject)
               //{
               //    if (this.transform.parent.parent.gameObject.layer == 11)
               //    {
               //        this.transform.parent.parent.GetComponent<Hand>().controller.TriggerHapticPulse(500);
               //    }
               //}
               //// End of Snippet ///////////////////////////////////
                wood.transform.localScale = Vector3.Lerp(end.transform.localScale, start.transform.localScale, timer/startTime);

                if (this.transform.parent.gameObject.layer == 11)
                {
                    this.transform.parent.GetComponent<Hand>().controller.TriggerHapticPulse(500);
                }
            }
            else
            {
                AtEnd.Invoke();
                if (!gobbyTutorial) 
                    ReleaseMe();
            }
        }
    }

    void OnTriggerStay(Collider entity)
    {

        if (entity.name == "Grinder")
        {
            grindMe = true;
            if(audio && !firstActivate)
            {
                AudioManager.instance.PlayGrindingSound(audio, AudioManager.AudioObjectType.Wood, true);
                firstActivate = true;
            }

        }
    }
    void OnTriggerExit(Collider entity)
    {
        if (entity.name == "Grinder")
        {
            grindMe = false;
            if (audio && firstActivate)
            {
                AudioManager.instance.StopGrinding_SlicingSound(audio);
                firstActivate = false;
            }
        }
    } 
    public void ReleaseMe()
    {
        MeshCollider temp = GetComponent<MeshCollider>();
        if(temp)
        temp.enabled = false;
        if (transform.parent)
        {
            tempHand = transform.parent.GetComponent<Hand>();
            if (tempHand)
            transform.parent.gameObject.GetComponent<Hand>().DetachObject(gameObject);
        }
        transform.parent = null;
        GetComponent<InstantiateObject>().MakeTheThing();
        GetComponent<DestroyMe>().DoItSissy();
    }

    public void ResetMe()
    {
        timer = startTime;
    }
}
