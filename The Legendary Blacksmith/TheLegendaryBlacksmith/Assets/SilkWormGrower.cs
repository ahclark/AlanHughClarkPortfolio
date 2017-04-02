using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilkWormGrower : MonoBehaviour {

    Vector3 startScale;
    public SilkWormString stringScript;
    public float timeOfRegrowth = 10;
    float timer = 0;
    public bool growing = false;

    private void Start()
    {
        startScale = transform.localScale;
        transform.localScale = new Vector3(0.4684176f, 0.4684177f, 0.9183879f);
        if(stringScript)
        stringScript.Grower = this;
    }
    private void FixedUpdate()
    {
        if (growing)
        {
            float dt = Time.deltaTime;
            transform.localScale = transform.localScale + new Vector3(0, 0, dt * .03f);
            timer += Time.deltaTime;
            if (timer >= timeOfRegrowth)
            {
                stringScript.enabled = true;
                growing = false;
                timer = 0;
            }
        }
    }
    public void SetGrowth()
    {
        growing = true;
        transform.localScale = startScale;
    }
}
