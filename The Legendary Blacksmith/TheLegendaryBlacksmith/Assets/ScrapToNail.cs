using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class ScrapToNail : MonoBehaviour {

    [SerializeField]
    GameObject Nail;
    [SerializeField]
    string sharpenName;
    float timer = 0;
    bool sharpening = false;
    Hand testHand;
    GameObject createdNail;

    bool first = false;
    PencilSharpener pencil;
    // Use this for initialization
    void Start () {
		
	}
	
	void FixedUpdate()
    {
        if (sharpening)
        {
            timer += Time.fixedDeltaTime;
        }
    }
	
    void OnTriggerStay(Collider col)
    {
        if (col.name == sharpenName)
        {
            pencil = col.GetComponent<PencilSharpener>();
            if(pencil && !first)
            {
                pencil.ActivateSharpening(true);
                first = true;
            }
            sharpening = true;
        }
        if (timer >= 2.0)
        {
            if (pencil)
                pencil.ActivateSharpening(false);
            SpawnNail();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == sharpenName)
        {
            timer = 0;
            sharpening = false;
            first = false;
        }
    }

    private void SpawnNail()
    {
        if (gameObject.transform.parent)
        {
            testHand = gameObject.transform.parent.gameObject.GetComponent<Hand>();
            if (testHand)
            {
                testHand.DetachObject(gameObject);
            }
        }
        createdNail = Instantiate(Nail, gameObject.transform.position, gameObject.transform.rotation);
        if (DialogueManager.dialogueInstance)
            DialogueManager.dialogueInstance.ActivateNewWeapon(Nail.name);
        if (testHand)
        {
            testHand.AttachObject(createdNail);
        }
        gameObject.SetActive(false);
    }
}
