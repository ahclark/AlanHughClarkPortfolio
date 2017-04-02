using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class LinearUpDown : MonoBehaviour
{
    [SerializeField]
    LinearMapping Linmap;
    [SerializeField]
    float upSpeed = 1;
    [SerializeField]
    float downSpeed = 1;
    float speed = 0;
    public bool up = false;
    public bool down = false;
    public UnityEvent OnZero;
    public UnityEvent OnOne;
    [SerializeField]
    float test;

	// Use this for initialization
	void Start ()
    {
        if (up)
        {
            speed = upSpeed;
            down = false;
        }
        else if (down)
        {
            speed = downSpeed;
        }
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        test = Linmap.value;
		if (up || down)
        {
            if (up)
                Linmap.value += Time.deltaTime * speed;
            if (down)
                Linmap.value -= Time.deltaTime * speed;
        }
        if (Linmap.value > 1)
        {
            SetOff();
            Linmap.value = 1;
            OnOne.Invoke();
        }
        if (Linmap.value < 0)
        {
            SetOff();
            Linmap.value = 0;
            OnZero.Invoke();
        }
	}

    public void SetUp()
    {
        up = true;
        down = false;
        speed = upSpeed;
    }
    public void SetDown()
    {
        up = false;
        down = true;
        speed = downSpeed;
    }
    public void SetOff()
    {
        up = false;
        down = false;
        speed = 0;
    }
    public void Resetter()
    {
        Linmap.value = 0;
    }
}
