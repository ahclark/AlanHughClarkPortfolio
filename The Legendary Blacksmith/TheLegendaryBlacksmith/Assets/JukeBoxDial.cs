using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class JukeBoxDial : MonoBehaviour 
{
    public UnityEvent onDialUp;
    public UnityEvent onDialDown;

    [SerializeField]
    CircularDrive CD;

    [SerializeField]
    float prevAngle = 0.0f;

    [SerializeField]
    float volmultiplier = 0.0f;
	// Use this for initialization
	void Start () 
	{
        if (CD == null)
            CD = GetComponent<CircularDrive>();
    }

    public void SetUpDial(float volume, float multiplier)
    {
        //Vector3 newRot = Vector3.zero;
        //switch(CD.axisOfRotation)
        //{
        //    case CircularDrive.Axis_t.XAxis:
        //        {
        //            newRot.x = CD.maxAngle * volume;
        //            break;
        //        }
        //
        //    case CircularDrive.Axis_t.YAxis:
        //        {
        //            newRot.y = CD.maxAngle * volume;
        //            break;
        //        }
        //
        //    case CircularDrive.Axis_t.ZAxis:
        //        {
        //            newRot.z = CD.maxAngle * volume;
        //            break;
        //        }
        //
        //    default:
        //        break;
        //}
        //CD.gameObject.transform.localEulerAngles = newRot;
        //CD.outAngle = CD.maxAngle * volume;
        //
        prevAngle = Mathf.Round(CD.outAngle * 10) / 10;
        volmultiplier = CD.maxAngle * multiplier;
    }
	// Update is called once per frame
	void FixedUpdate () 
	{
        if (prevAngle - CD.outAngle >= volmultiplier)
        {
            prevAngle = Mathf.Round(CD.outAngle * 10)/10;
            onDialDown.Invoke();
        }
        else if (prevAngle - CD.outAngle <= -volmultiplier)
        {
            prevAngle = Mathf.Round(CD.outAngle * 10) / 10;
            onDialUp.Invoke();
        }




	}
}
