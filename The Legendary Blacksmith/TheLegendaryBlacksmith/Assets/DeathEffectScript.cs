using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffectScript : MonoBehaviour {

    public GameObject effectObj = null;
    public int numObj = 4;
    private void Start()
    {
        if(effectObj != null)
        {
            for (int i = 0; i < numObj; i++)
            {
                Instantiate(effectObj, transform.position, transform.rotation);
            }
        }
    }
}
