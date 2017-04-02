using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ScytheRepair : MonoBehaviour {
    MeshFilter MF;
    MeshRenderer MR;
    [SerializeField]
    Mesh Repair;
    [SerializeField]
    Mesh Finished;
    public bool onAnvil = false;
    public bool heatedScrap = false;
    [SerializeField]
    string heatedScrapName;
    bool HammerBuffer = true;
    public int meshCounter = 0;
    [SerializeField]
    Material Red;
    [SerializeField]
    string FinishedName;
    [SerializeField]
    Material finishedMat;
    heatedNameChange HeatedScrapBar;
    [SerializeField]
    ParticleSystem PS;
    AudioSource audio;
    public UnityEvent OnCreate;


    // Use this for initialization
    void Start () {
        MF = gameObject.GetComponent<MeshFilter>();
        MR = gameObject.GetComponent<MeshRenderer>();
        audio = GetComponent<AudioSource>();
	}
	
	void OnTriggerStay(Collider col)
    {
        if (col.name == "Top")
        {
            onAnvil = true;
        }

        if (col.name == heatedScrapName)
        {
            HeatedScrapBar = col.gameObject.GetComponent<heatedNameChange>();
            heatedScrap = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Top")
        {
            onAnvil = false;
        }

        if (col.name == heatedScrapName)
        {
            heatedScrap = false;
        }

        if (col.name == "Head")
        {
            HammerBuffer = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Head" && HammerBuffer && onAnvil && heatedScrap)
        {
            if (audio)
                AudioManager.instance.PlayCollisionSound(audio, AudioManager.AudioInteractionType.Bang, AudioManager.AudioObjectType.Metal, AudioManager.AudioObjectType.Metal);
            switch (meshCounter)
            {
                case 0:
                    {
                        MF.mesh = Repair;
                        MR.material = Red;
                        HeatedScrapBar.DestroyScrap();
                        heatedScrap = false;
                        meshCounter++;
                        HammerBuffer = false;
                        break;
                    }
                case 1:
                    {
                        MF.mesh = Finished;
                        MR.material = finishedMat;
                        gameObject.name = FinishedName;
                        if (DialogueManager.dialogueInstance)
                            DialogueManager.dialogueInstance.ActivateNewWeapon(FinishedName);
                        PS.Play();
                        if (OnCreate != null)
                        {
                            OnCreate.Invoke();
                        }
                        HeatedScrapBar.DestroyScrap();
                        HammerBuffer = false;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
