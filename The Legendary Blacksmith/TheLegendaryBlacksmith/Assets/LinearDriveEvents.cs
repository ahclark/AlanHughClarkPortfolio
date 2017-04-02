using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;


/// <summary>
/// Author: David Scarlett
/// Date: 2/25/2017 3:21 PM
/// Project: The Legendary Blacksmith
/// Company: BelieVRs
/// Version: 1.0
/// 
/// This is an addition to Valve's VR Interaction System.  
/// They have functionality for LinearDrives, but they only function inward with math.
/// The purpose of this script is to deliberate between Steam's code and your code.
/// There are 3 public events that are the only purpose of this script and their descritions are below:
/// 
/// Basic Info:
///     Linear maps are "bars" or "sliders" in essence, but can be used for much more.
///     It keeps track of the position of an object in relation to two other objects which are defined as "start" and "end".
///     It's current position in relativity to "start" and "end" is measured by a floating point number ranging from 0 to 1.
///     
/// 
/// 
/// OnLinearZero
///     This event is called when the Linear Map's value hit's 0.
///     
/// OnLinearOne
///     Similar to above, but when the value is 1.
///     
/// OnLinearMove(float value)
///     This will be called in looping fashion, only when the linear map has changed value since the last FixedUpdate().
///     "value" is the floating point value that represents the position of the object in relation to the "start" and "end".
///     This is so you may react based on certain "ticks" or "markers" in the line and not only the "start" and "end".
///     
/// 
/// Note:
/// You will notice the line directly below that says "[RequireComponent(typeof(LinearDrive))]"
/// You need a LinearDrive for any of this to work, as well as a Linear Map, on your object.
/// 
/// </summary>


[RequireComponent(typeof(LinearDrive))]
    public class LinearDriveEvents : MonoBehaviour
    {
        public UnityEvent OnLinearZero;

        public UnityEvent OnLinearOne;

        public delegate void OnLinearMoveDelegate(float value);
        public event OnLinearMoveDelegate OnLinearMove;

        public UnityEvent OnMove;

        LinearDrive linearDrive;
        float storedValue = 0;

        private void Start()
        {
            if(linearDrive == null)
            {
                linearDrive = GetComponent<LinearDrive>();
                if(linearDrive == null)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                //Change by Benjamin
                //Fix for Null Reference exception; sometimes this component will load before the linearMapping is set up
                if(linearDrive.linearMapping)
                    storedValue = linearDrive.linearMapping.value;
                }
            }

        }

        private void FixedUpdate()
        {
            if(linearDrive)
            {
            if (linearDrive.linearMapping)
            {
                if (linearDrive.linearMapping.value != storedValue)
                {
                    storedValue = linearDrive.linearMapping.value;
                    CheckOne();
                    CheckZero();
                    if (OnLinearMove != null)
                        OnLinearMove(storedValue);
                    if (OnMove != null)
                        OnMove.Invoke();
                }
            }
            }
        }

        void CheckOne()
        {
            if(storedValue == 1 && OnLinearOne != null)
            {
                OnLinearOne.Invoke();
            }
        }

        void CheckZero()
        {
            if (storedValue == 0 && OnLinearZero != null)
            {
                OnLinearZero.Invoke();
            }
        }



    }

