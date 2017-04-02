using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class ArrowMaker : MonoBehaviour
{
    public GameObject justTheTip;
    public GameObject birdsOfAFeather;
    public GameObject flip;
    public bool buttOn = false;
    public bool tipOn = false;
    [SerializeField]
    GameObject fullArrow;
    public UnityEvent OnCreate;

    ParticleSystem PS;

    public void MakeTheArrow()
    {
        if (buttOn && tipOn)
        {
            if (transform.parent)
            {
                transform.parent.GetComponent<Hand>().DetachObject(gameObject);
            }

            GameObject arrow = Instantiate(fullArrow);
            PS = arrow.GetComponentInChildren<ParticleSystem>();
            PS.Play();
            if (OnCreate != null)
            {
                OnCreate.Invoke();
            }
            if (DialogueManager.dialogueInstance)
                DialogueManager.dialogueInstance.ActivateNewWeapon(fullArrow.name);
            arrow.name = fullArrow.name;
            arrow.transform.position = transform.position;
            arrow.transform.rotation = transform.rotation;
            Destroy(gameObject);
        }
    }
}
