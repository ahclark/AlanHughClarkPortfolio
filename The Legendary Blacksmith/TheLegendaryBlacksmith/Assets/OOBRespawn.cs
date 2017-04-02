using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class OOBRespawn : MonoBehaviour {
    [SerializeField]
    GameObject RespawnLocation;

    private void Start()
    {
        RespawnLocation = GameObject.Find("WorkOrderRespawn");
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.name == "WestWall" || col.name == "EastWall" || col.name == "NorthWall" || col.name == "SouthWall")
        {
            if (transform.parent)
            {
                Hand thehand = transform.parent.gameObject.GetComponent<Hand>();
                if (thehand)
                    thehand.DetachObject(gameObject);
            }
            gameObject.transform.position = RespawnLocation.transform.position;
        }
    }
}
