using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShavingSelfDestruct : MonoBehaviour
{
    public bool doIt = false;
    float timer = 3f;
	
	// Update is called once per frame
	void Update ()
    {
        if (doIt)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime * 0.3f;
            }
            else
            {
                Suicide();
            }
        }
	}

    void Suicide()
    {
        if (!transform.parent)
        {
            Destroy(gameObject);
        }
    }
    public void STOOOOP()
    {
        doIt = false;
    }
    public void DoAFlip()
    {
        doIt = true;
    }
}
