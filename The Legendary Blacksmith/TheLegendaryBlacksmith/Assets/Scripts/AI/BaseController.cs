using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {


   public float health = 300;
    public GameObject[] Portals = new GameObject[3];
    enum LaneIndex { Left = 0, Mid, Right};
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool AttackMe(float damage, int unitID)
    {
        //take damage
        health -= damage;
        //does it kill me?
        if(health <= 0)
        {
            //die
            return true;
        }

        return true;
    }
}
