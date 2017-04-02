using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeColor : MonoBehaviour
{
    enum ColorLerp
    {
        Smooth,
        Linear
    }

    [SerializeField]
    Color beginning;
    [SerializeField]
    Color end;

    Renderer rend;

    bool fadeDown = true;

    [SerializeField]
    float speed = 1.0f;

    [SerializeField]
    ColorLerp lerp;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = beginning;
    }

    // Update is called once per frame
    void Update()
    {
        float t = 0.0f;
        if(lerp == ColorLerp.Smooth)
            t = (Mathf.Sin(Time.time * (speed*2.0f)) + 1) / 2.0f;
        if(lerp == ColorLerp.Linear)
            t = Mathf.PingPong(Time.time * (speed), 1.0f);
        rend.material.color = Color.Lerp(beginning, end, t);
    }
}
