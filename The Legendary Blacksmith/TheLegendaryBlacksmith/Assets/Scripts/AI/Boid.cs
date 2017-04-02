using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    public Vector3 Velocity = new Vector3(0, 0, 1);
    public float MaxSpeed = 3.0f;
    public float SafeRadius = .5f;
    public float assignedY = 0;
    Vector3 pos = new Vector3(0, 0, 0);

    public void BoidUpdate(float dt)
    {
        pos = transform.position + Velocity * dt;
        pos.y = assignedY;
        //transform.position.Set(pos.x, pos.y, pos.z);
        transform.position = pos;
    }
    private void OnDisable()
    {
        assignedY = 0;

    }

}
