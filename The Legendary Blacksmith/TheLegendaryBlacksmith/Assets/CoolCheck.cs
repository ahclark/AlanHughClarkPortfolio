using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class CoolCheck : MonoBehaviour {
    [SerializeField]
    SwordDownLerp[] SDL;
    [SerializeField]
    GameObject SwordInStone;
    Hand testHand;
    GameObject createdSwordInStone;
    [SerializeField]
    GameObject spawnPoint;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(SDL[0].cool && SDL[1].cool && SDL[2].cool && SDL[3].cool && SDL[4].cool)
        {
            if (gameObject.transform.parent)
            {
                testHand = gameObject.transform.parent.gameObject.GetComponent<Hand>();
                if (testHand)
                {
                   testHand.DetachObject(gameObject);
                }
            }
            createdSwordInStone = Instantiate(SwordInStone, spawnPoint.transform.position, spawnPoint.transform.rotation);
            createdSwordInStone.transform.rotation = spawnPoint.transform.rotation;
            if (DialogueManager.dialogueInstance)
                DialogueManager.dialogueInstance.SharpentheSword();
            //if (testHand)
            //{
            //    testHand.AttachObject(createdSwordInStone);
            //}
            gameObject.SetActive(false);
        }
	}
}
