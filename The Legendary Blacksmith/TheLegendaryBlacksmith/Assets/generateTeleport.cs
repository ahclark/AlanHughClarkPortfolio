using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
public class generateTeleport : MonoBehaviour {





	// Use this for initialization
	void Start () {

        GameObject temp = Instantiate(new GameObject(), transform.position,transform.rotation,this.gameObject.transform);
        if (temp)
        {
            temp.AddComponent<MeshFilter>();
            temp.AddComponent<MeshRenderer>();
            temp.AddComponent<TeleportArea>();
            BoxCollider box = temp.AddComponent<BoxCollider>();
            BoxCollider parentCollider = gameObject.GetComponent<BoxCollider>();
            if (parentCollider)
            {
                box.size = parentCollider.size;
                box.center = parentCollider.center;
                parentCollider.size = Vector3.zero;
            }
            temp.SetActive(true);
        }
	}
	

}
