using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BaseSpawner : MonoBehaviour {

    public GameObject BasePos;
    public GameObject UnitPreFab;
    public float spawnRate;
    float timer = 0;
    
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            timer = 0;
            GameObject temp = (GameObject)Instantiate(UnitPreFab, transform.position, transform.rotation);


        }
    }


}
