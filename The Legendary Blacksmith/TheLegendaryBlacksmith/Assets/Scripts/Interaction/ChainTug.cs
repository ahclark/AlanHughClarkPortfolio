using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ChainTug : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField]
    GameObject endPos;
    public bool up = false;
    public bool down = false;
    [SerializeField]
    LinearMapping linMap;
    [SerializeField]
    float upSpeed = 1;
    [SerializeField]
    float downspeed = 1;
    public bool canGoUp = false;

	// Use this for initialization
	void Start ()
    {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (up)
        {
            GoUp();
        }
        if (down)
        {
            GoDown();
        }
	}
    void GoUp()
    {
        if (linMap.value > 1)
        {
            linMap.value = 0;
        }
        linMap.value += upSpeed;
        transform.position = Vector3.Lerp(startPos, endPos.transform.position, linMap.value);
    }
    void GoDown()
    {
        if (linMap.value < 0)
        {
            linMap.value = 1;
        }
        linMap.value -= downspeed;
        transform.position = Vector3.Lerp(startPos, endPos.transform.position, linMap.value);
    }
    public void GetItUp()
    {
        up = true;
        down = false;
    }
    public void DownBoy()
    {
        down = true;
        up = false;
    }
    public void Stop()
    {
        up = false;
        down = false;
    }
    public void CanGetUp()
    {
        canGoUp = true;
    }
    public void CantGetUp()
    {
        canGoUp = false;
    }

    public void CrankUp()
    {
        if (canGoUp)
        {
            GoUp();
        }
    }
    public void CrankDown()
    {
        if (canGoUp)
        {
            GoDown();
        }
    }
}
