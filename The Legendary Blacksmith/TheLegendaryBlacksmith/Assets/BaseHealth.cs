using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour {

    public GameObject[] HealthBar;
    public float health = 10;


    public void Damage()
    {
        if (health <= 0)
        {
            return;
        }
        health -= 1;
        Destroy(HealthBar[(int)health]);


    }
}
