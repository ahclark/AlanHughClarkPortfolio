using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class collideEnableDestroy : MonoBehaviour {
    [SerializeField]
    GameObject Enable;
    [SerializeField]
    GameObject NameCheck;
    public bool Enabled = false;
	void OnTriggerEnter(Collider col)
    {
        if (col.name == NameCheck.name)
        {
            if (col.transform.parent)
            {
                Hand thehand = col.transform.parent.gameObject.GetComponent<Hand>();
                if(thehand)
                    thehand.DetachObject(col.gameObject);//EDITED TO BE COL.GAMEOBJECT DONE BY MILES
            }
            col.gameObject.SetActive(false);
            Enable.SetActive(true);
            Enabled = true;
            //EDITS BEING MADE BY MILES TO FIX THE CONTROL DISAPPEARING
            if (transform.parent)
            {
                Hand thehand = transform.parent.gameObject.GetComponent<Hand>();
                if (thehand)
                    thehand.DetachObject(gameObject);
            }
            //NOTHING MORE TO SEE HERE
            gameObject.SetActive(false);
        }
    }

}
