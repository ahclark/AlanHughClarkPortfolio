using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleLighting : MonoBehaviour
{
    [SerializeField]
    float realminutesinDay = 1.0f;

    float timer;
    float dayPercentage;
    float turnSpeed;

    Light mLight;
    // Use this for initialization
    void Start()
    {
        timer = 0.0f;
        mLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeofDay();

        turnSpeed = 360.0f / (realminutesinDay * 60) * Time.deltaTime;
        transform.RotateAround(transform.position, transform.right, turnSpeed);
        if (NightTime())
        {
            if (mLight.intensity > 0.0f)
                mLight.intensity -= 0.05f;
        }

        else
        {
            if (mLight.intensity < 1.0f)
                mLight.intensity += 0.05f;
        }

    }

    void TimeofDay()
    {
        timer += Time.deltaTime;
        dayPercentage = timer / (realminutesinDay * 60.0f);
        if (timer > realminutesinDay * 60.0f)
        {
            timer = 0.0f;
        }
    }

    bool NightTime()
    {
        bool check = false;
        if (dayPercentage > 0.5f)
            check = true;
        return check;
    }
}
