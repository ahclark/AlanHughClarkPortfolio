using UnityEngine;
using System.Collections;

public class UnitAnimator : MonoBehaviour
{
    public Vector3 velocity = new Vector3(0, 1, 0);
     float time = 0;
    public float limit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate()
    {
        float dt = Time.deltaTime;
        transform.position = transform.position + velocity * dt;
        time += dt;
        if(time >= limit)
        {
            time = 0;
            velocity *= -1;
        }
    }
}
