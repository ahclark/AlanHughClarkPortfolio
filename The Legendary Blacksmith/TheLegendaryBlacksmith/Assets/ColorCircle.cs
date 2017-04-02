using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCircle : MonoBehaviour 
{
    [SerializeField]
    float duration = 1;
    float startTime;
    float timer;


    [SerializeField]
    DayNightCycle.ColorLerp lerp;
    Image imag;
    [SerializeField]
    float startColor;
	// Use this for initialization
	void Start()
    {
        imag = GetComponent<Image>();
        startTime = Time.time;
        timer = 0.0f;
    }
	// Update is called once per frame
	void FixedUpdate () 
	{
        float t = 0.0f;
        if (lerp == DayNightCycle.ColorLerp.Smooth)
            t = (Mathf.Sin(Mathf.PI * (Time.time - startTime) / duration) + 1) / 2.0f;
        if (lerp == DayNightCycle.ColorLerp.Linear)
            t = Mathf.PingPong((Time.time - startTime), duration) / duration;
        imag.fillAmount = Mathf.InverseLerp(0, 1.0f, t);
	}
}
