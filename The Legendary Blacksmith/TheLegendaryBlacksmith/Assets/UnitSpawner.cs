using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour {

    public Transform[] spawnPoints = new Transform[3];
    public float spawnRate = 10;
    float timer = 0;
    public UnitContainer unitCont;
    public GameObject[] unitPrefabs = new GameObject[3];
    float onetime = 0;

    private void FixedUpdate()
    {
        
        if(onetime < 300)
            onetime += Time.deltaTime;
        else
        {
            timer += Time.deltaTime;
            if (timer >= spawnRate)
            {
                timer = 0;
                Spawn();
            }
        }
    }


    void Spawn()
    {
        int randPos, randType;
        randPos = Random.Range(0, 3);
        randType = Random.Range(0, 3);
        unitCont.AddUnit(Instantiate(unitPrefabs[randType], spawnPoints[randPos].position, Quaternion.identity, unitCont.transform));
    }
}
