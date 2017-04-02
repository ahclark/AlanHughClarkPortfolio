using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitContainer : MonoBehaviour {
    public List<GameObject> units = new List<GameObject>();
    // Use this for initialization
    float timer = 0;
    public float bufferTime = 300;
    bool spawned = false;

    private void Start()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if(units[i])
            {
                units[i].SetActive(false);
            }
        }
    }


    private void FixedUpdate()
    {
        if(!spawned)
        {
            timer += Time.deltaTime;
            if(timer >= bufferTime)
            {
                spawned = true;
                StartUnits();
            }
        }
    }

    void StartUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i])
            {
                units[i].SetActive(true);
            }
        }
    }

    public bool AddUnit(GameObject unit)
    {
        if(unit)
        {
            units.Add(unit);
            return true;
        }
        return false;
    }
}
