using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour {
    [SerializeField]
    GameObject StartPos;
    [SerializeField]
    GameObject EndPos;
    public bool Moving;
    Vector3 Distance;
    Vector3 Stop;
	// Use this for initialization
	void Start () {
        Stop.x = 0.1f;
        Stop.y = 0.1f;
        Stop.z = 0.1f;
        Moving = true;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Distance = EndPos.transform.position - transform.position;
        if (Distance.x <= Stop.x && Distance.y <= Stop.y && Distance.z <= Stop.z)
        {
            Moving = false;
        }
		else if (Moving)
        {
            transform.position += Distance * Time.fixedDeltaTime;
        }
	}
}
