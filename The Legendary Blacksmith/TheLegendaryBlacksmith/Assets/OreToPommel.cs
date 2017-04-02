using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class OreToPommel : MonoBehaviour {
    ForgeHeat FH;
    public bool onAnvil = false;
    public bool finished = false;
    public int hitCounter = 0;
    public bool onlyOnce = true;
    [SerializeField]
    Mesh TransitionOne;
    [SerializeField]
    Mesh TransitionTwo;
    MeshFilter MF;
    [SerializeField]
    GameObject Pommel;
    MeshRenderer MR;
    Color stuff;
    Mesh moarstuff;
    Hand testHand;
    GameObject PommelInstantiate;

    AudioSource audio;

	// Use this for initialization
	void Start () {
        FH = gameObject.GetComponent<ForgeHeat>();
        MF = gameObject.GetComponent<MeshFilter>();
        MR = gameObject.GetComponent<MeshRenderer>();
        stuff = MR.material.color;
        moarstuff = MF.mesh;
        audio = GetComponent<AudioSource>();
	}

    void OnTriggerStay(Collider col)
    {
        if (col.name == "Top" && FH.Heated)
        {
            onAnvil = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Top")
        {
            onAnvil = false;
        }
        if (col.name == "Head")
        {
            onlyOnce = true;
        }
    }

    
    void OnTriggerEnter(Collider entity)
    {
        if (entity.name == "Head" && onlyOnce == true && onAnvil == true)
        {
            if (audio)
            {
                AudioManager.AudioObjectType temp;
                if (hitCounter >= 1)
                    temp = AudioManager.AudioObjectType.Rock;
                else
                    temp = AudioManager.AudioObjectType.Metal;
                AudioManager.instance.PlayCollisionSound(audio, AudioManager.AudioInteractionType.Thud, AudioManager.AudioObjectType.Metal, temp);

            }
            DoSwitch();
        }
    }

    void DoSwitch()
    {
        switch (hitCounter)
        {
            case 0:
                MF.mesh = TransitionOne;
                hitCounter++;
                onlyOnce = false;
                break;
            case 1:
                MF.mesh = TransitionTwo;
                hitCounter++;
                onlyOnce = false;
                break;
            case 2:

                testHand = gameObject.transform.parent.gameObject.GetComponent<Hand>();
                if (testHand)
                {
                    testHand.DetachObject(gameObject);
                }
                PommelInstantiate = Instantiate(Pommel, this.transform.position, this.transform.rotation);
                PommelInstantiate.name = Pommel.name;
                if (testHand)
                {
                    testHand.AttachObject(PommelInstantiate);
                }

                //Instantiate(Pommel, this.transform.position, this.transform.rotation).name = Pommel.name;
                finished = true;
                ResetShape();

                //if (gameObject.transform.parent)
                //{
                //    gameObject.transform.parent.gameObject.GetComponent<Hand>().DetachObject(gameObject);
                //}
                gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void ResetShape()
    {
        MF.mesh = moarstuff;
        MR.material.color = stuff;
    }
}
