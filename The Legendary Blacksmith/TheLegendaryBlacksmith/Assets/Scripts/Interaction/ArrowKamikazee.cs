using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowKamikazee : MonoBehaviour
{
    float timer = 0.1f;
    

	// Use this for initialization
	void Start ()
    {
	}
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime * 1;
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.parent = null;
            }
            Destroy(gameObject);
        }
    }
}
