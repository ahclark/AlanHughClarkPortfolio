using UnityEngine;
using System.Collections;

public class Destroy2 : MonoBehaviour {
    public GameObject Obj;
    public float DestroyTime;
    private float timer = 0.0f;
	
	// Update is called once per frame
	void FixedUpdate () {
        timer += Time.deltaTime;
        if (timer >= DestroyTime)
        {
            Destroy(Obj);
        }
	}
}
