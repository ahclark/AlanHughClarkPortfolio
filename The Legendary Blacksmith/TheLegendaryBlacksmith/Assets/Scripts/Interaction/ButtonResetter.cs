using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonResetter : MonoBehaviour
{
    [SerializeField]
    GameObject button1;
    [SerializeField]
    GameObject button2;
    Vector3 startPos1;
    Vector3 startPos2;
	// Use this for initialization
	void Start ()
    {
        startPos1 = button1.transform.position;
        startPos2 = button2.transform.position;
	}
    private void OnTriggerEnter(Collider entity)
    {
        if (entity.tag == "Button")
        {
            button1.transform.position = startPos1;
            button2.transform.position = startPos2;
        }
    }
}
