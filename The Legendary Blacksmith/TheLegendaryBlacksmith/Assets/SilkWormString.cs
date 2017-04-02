using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SilkWormString : MonoBehaviour {
    public SilkWormGrower Grower;
    public GameObject silkString;
    public LineRenderer stringRenderer = null;
    public Hand currHand;
    public bool down = false;
    bool stringTime = false;
    public GameObject stringToSpawn;
	// Use this for initialization
	void Start () {
        if(silkString)
        {
            GameObject obj = Instantiate(silkString, transform);
            if(obj)
                stringRenderer = GetComponentInChildren<LineRenderer>();
            if (stringRenderer)
            {
                stringRenderer.SetPosition(0, transform.position);
                stringRenderer.SetPosition(1, transform.position);

            }
        }
	}
	public void SetDown()
    {
        down = true;
    }
    public void SetUp()
    {
        down = false;
    }
    // Update is called once per frame
    private void OnHandHoverBegin(Hand hand)
    {
        currHand = hand;
    }

    private void OnHandHoverEnd(Hand hand)
    {
        if (!down)
            currHand = null;
    }

   public  void StartString()
    {
        if (!stringRenderer || !currHand)
            return;
        stringRenderer.SetPosition(1, currHand.transform.position);
        stringTime = true;
    }

   public  void EndString()
    {

    }

    public void CutString()
    {
        stringTime = false;
        down = false;

        if (stringToSpawn && currHand)
        {
            print("Spawned");
            GameObject temp = Instantiate(stringToSpawn, currHand.transform.position, currHand.transform.rotation);
            currHand.AttachObject(temp);
        }
        currHand = null;
        stringRenderer.SetPosition(1, transform.position);
        if (Grower)
        {
            Grower.SetGrowth();
        }
    }

    void FixedUpdate()
    {
        if(Grower)
        {
            if (Grower.growing)
                return;
        }
        if(stringRenderer && currHand && stringTime)
        {
            stringRenderer.SetPosition(1, currHand.transform.position);
            if(CheckDistance())
            {
                CutString();
            }
        }
    }
    bool CheckDistance()
    {
        if (!stringRenderer)
            return false;
        Vector3 dist = stringRenderer.GetPosition(0) - stringRenderer.GetPosition(1);
        if (dist.magnitude >= 1)
        {
            return true;
        }
        return false;
    }
}
