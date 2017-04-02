using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class LeverHandler : MonoBehaviour
{ 
    [SerializeField]
    bool up = false;
    [SerializeField]
    CircularDrive circDrive;
    [SerializeField]
    LinearMapping linMap;
    [SerializeField]
    float speed = 1;
    public UnityEvent GotUp;
    public UnityEvent GotDown;

    private bool upOnce = false;
    private bool downOnce = false;
    

    private void Start()
    {

    }

    private void Update()
    {
       if (up && circDrive.outAngle < circDrive.maxAngle)
        {
            circDrive.outAngle += speed;
            circDrive.UpdateValues();
        }
       if (linMap.value >= 1 && !upOnce)
        {
            GotUp.Invoke();
            linMap.value = 1;
            upOnce = true;
            downOnce = false;
        }
       else if (linMap.value <= 0 && !downOnce)
        {
            GotDown.Invoke();
            linMap.value = 0;
            downOnce = true;
            upOnce = false;
        }
    }

    public void GetItUp()
    {
        up = true;
    }
    public void ProblemsGettingItUp()
    {
        up = false;
    }

    public void ForceDown()
    {
        GotDown.Invoke();
        linMap.value = 0;
        downOnce = true;
        upOnce = false;
        circDrive.outAngle = -37;
        circDrive.UpdateValues();
    }
}
