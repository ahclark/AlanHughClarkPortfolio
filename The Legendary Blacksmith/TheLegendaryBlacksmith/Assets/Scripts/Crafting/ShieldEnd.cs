using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class ShieldEnd : MonoBehaviour
{
    [SerializeField]
    GameObject shield;
    [SerializeField]
    GameObject handle;
    bool accepting = true;
    [SerializeField]
    ParticleSystem PS;
    public UnityEvent OnCreate;


    void OnTriggerEnter(Collider entity)
    {
        if (entity.name == "Shield_Handle" && accepting)
        {
            shield.tag = "Equippable";
            shield.name = "FullShield 1";
            accepting = false;
            handle.SetActive(true);
            PS.Play();
            if (OnCreate != null)
            {
                OnCreate.Invoke();
            }
            if (DialogueManager.dialogueInstance)
                DialogueManager.dialogueInstance.ActivateNewWeapon(shield.name);
            if (entity.transform.parent)
            {
                entity.transform.parent.gameObject.GetComponent<Hand>().DetachObject(entity.gameObject);
            }
            Destroy(entity.gameObject);
            Destroy(gameObject);

        }
    } 
}
