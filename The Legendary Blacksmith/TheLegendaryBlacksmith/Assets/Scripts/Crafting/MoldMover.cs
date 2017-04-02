using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MoldMover : MonoBehaviour
{
    [SerializeField]
    GameObject start;
    [SerializeField]
    GameObject end;
    [SerializeField]
    LinearMapping LinMap;
    
    public ScrapPress press;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localPosition = Vector3.Lerp(start.transform.localPosition, end.transform.localPosition, LinMap.value);
    }
}
