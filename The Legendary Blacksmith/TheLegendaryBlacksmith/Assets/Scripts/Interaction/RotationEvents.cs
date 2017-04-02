using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class RotationEvents : MonoBehaviour
{
    [SerializeField]
    CircularDrive CircDrive;
    [SerializeField]
    LinearMapping LinMap;

    [SerializeField]
    bool doClockwise = false;
    public UnityEvent OnRotateClockwise;
    [SerializeField]
    bool doCounterClock = false;
    public UnityEvent OnRotateCounterClock;
    [SerializeField]
    bool doOutAngle = false;
    public UnityEvent OnOutAngle;
    [SerializeField]
    float target = 0;
    [SerializeField]
    float baseAngle;

    public bool resetter = false;
    public bool on = false;

	// Use this for initialization
	void Start ()
    {
        baseAngle = CircDrive.outAngle;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (on)
        {
            if (doClockwise)
            {
                ClockWise();
            }
            if (doCounterClock)
            {
                CounterClock();
            }
            if (doOutAngle)
            {
                OutAngle();
            }
            baseAngle = CircDrive.outAngle;
        }
	}

    public void TurnMeOn()
    {
        on = true;
    }

    public void TurnMeOff()
    {
        on = false;
    }

    public void ResetOutAngle()
    {
        CircDrive.outAngle = 0;
    }

    void ClockWise()
    {
        if (CircDrive.outAngle < baseAngle)
        {
            OnRotateClockwise.Invoke();
        }
    }

    void CounterClock()
    {
        if (CircDrive.outAngle > baseAngle)
        {
            OnRotateCounterClock.Invoke();
        }
    }

    void OutAngle()
    {
        if (target < 0 && CircDrive.outAngle <= target)
        {
            OnOutAngle.Invoke();
        }
        else if (target > 0 && CircDrive.outAngle >= target)
        {
            OnOutAngle.Invoke();
        }
        if (resetter)
        {
            CircDrive.outAngle = 0;
        }

    }


}
