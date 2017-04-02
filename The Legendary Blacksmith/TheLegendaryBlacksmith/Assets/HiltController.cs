using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HiltController : MonoBehaviour {
    [SerializeField]
    collideEnableDestroy[] Check;
    [SerializeField]
    GameObject dependentAttach;
    bool swordActive = false;
    bool nameset = false;
    [SerializeField]
    BoxCollider swordCollider;
    [SerializeField]
    ParticleSystem PS;
    public UnityEvent OnCreate;
	// Use this for initialization
	void Start () {
        swordCollider = gameObject.GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update () {

        //Benjamin Ousley
        //Set Sword Collision sound to metal after attaching object to hilt
        if(Check[0].Enabled || Check[1].Enabled)
        {
            CollisionSFX temp = GetComponent<CollisionSFX>();
            if (temp)
                temp.soundType = AudioManager.AudioObjectType.Metal;
        }
        //Benjamin Ousley
        //If the Crossguard has been attached and a sword hasn't
		if (Check[0].Enabled/* && Check[1].Enabled */&& !Check[2].Enabled)
        {
            dependentAttach.SetActive(true);
            
            
        }
        if (Check[0].Enabled && Check[1].Enabled && Check[2].Enabled && nameset == false)
        {
            PS.Play();
            if (OnCreate != null)
            {
                OnCreate.Invoke();
            }
            gameObject.name = "GreatSword";
            gameObject.tag = "Equippable";
            Debug.Log("Name has been set");
            nameset = true;
            if (DialogueManager.dialogueInstance)
                DialogueManager.dialogueInstance.ActivateNewWeapon(gameObject.name);
            
            if(swordCollider)
                swordCollider.gameObject.SetActive(true);

        }
    }
}
