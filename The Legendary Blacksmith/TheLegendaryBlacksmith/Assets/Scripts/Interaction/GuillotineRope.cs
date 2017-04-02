using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GuillotineRope : MonoBehaviour
{
    public delegate void ChopDelegate();
    public static event ChopDelegate OnChop;




    [SerializeField]
    GameObject start;
    [SerializeField]
    GameObject end;
    [SerializeField]
    LinearMapping LinearMap;
    [SerializeField]
    GameObject otherRope;
    [SerializeField]
    GameObject otherStart;
    [SerializeField]
    GameObject otherEnd;
    public bool grabbed = false;
    [SerializeField]
    float speed = 1;
    [SerializeField]
    GuillotineLog Log;
    [SerializeField]
    float gobbySpeed = -3;
    [SerializeField]
    bool gobbyGrabbed = false;

	// Use this for initialization
	void Start ()
    {

	}

    // Update is called once per frame
    void Update()
    {
        if (!grabbed && !gobbyGrabbed)
        {
            if (LinearMap.value > 0.01)
                LinearMap.value -= Time.deltaTime * speed;
            else
                LinearMap.value = 0;
        }
        if (gobbyGrabbed)
        {
            if (LinearMap.value < 1)
                LinearMap.value += Time.deltaTime * gobbySpeed;
            else
            {
                SetGrabbage();
                gobbyGrabbed = false;
                LinearMap.value = 1;
                UnSetGrabbage();
                gobbyGrabbed = false;
            }
            
        }
        transform.localScale = Vector3.Lerp(start.transform.localScale, end.transform.localScale, LinearMap.value);
        otherRope.transform.localScale = Vector3.Lerp(otherStart.transform.localScale, otherEnd.transform.localScale, LinearMap.value);
    }

    public void SetGrabbage()
    {
        grabbed = true;
    }
    public void UnSetGrabbage()
    {
        if(LinearMap.value != 1)
        {
            grabbed = false;
        }
        else
        {
            Log.ChangePositions();
        }
    }
    public void OffWithHisHead()
    {
        grabbed = false;
        if(OnChop != null)
        OnChop();
        
    }
    public void GobbyGrab()
    {
        gobbyGrabbed = true;
    }
}
