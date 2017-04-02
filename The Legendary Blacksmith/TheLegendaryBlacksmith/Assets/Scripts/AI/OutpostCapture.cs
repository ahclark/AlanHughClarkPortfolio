using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OutpostCapture : MonoBehaviour
{
    public float captureAmount = 0;
    public float captureLimit = 30;
    public int captureSideModifier = 0;
    public int numUnits = 0;
    public int currType = 0;
    public MeshRenderer toChangeMesh;
    public Material Red, Blue, White;
    Material[] materials = new Material[3];
    public bool capturing = false;
    public BasePortalController Portal = null;
    public List<Flock> captureFlocks;
    private void Start()
    {
        materials[0] = Red;
        materials[1] = Blue;
        materials[2] = White;
        


    }
  // private void Update()
  // {
  //     if(capturing)
  //     {
  //         toChangeMesh.material = White;
  //     }
  // }

    public void Capture(int captureModifier)
    {
        if(capturing)
        {
            toChangeMesh.material = White;
            captureAmount += Time.deltaTime * captureModifier;
            if(Mathf.Abs(captureAmount) >= captureLimit)
            {
                currType = captureModifier;
                captureAmount = captureLimit * currType;
                capturing = false;
                if(currType == 1)
                    toChangeMesh.material = materials[0];
                else if(currType == -1)
                    toChangeMesh.material = materials[1];
                if (Portal != null)
                Portal.TurnMeOn();
                for (int i = 0; i < captureFlocks.Count; i++)
                {
                    
                    captureFlocks[i].targetPos = Vector3.zero;
                    
                }
                captureFlocks.Clear();
            }
        }
    }


    public bool StartCapture(Flock _flock)
    {
        //eliminate all same side checking
        if (_flock.side == currType)
            return false;
        if (_flock.currOutpost == null)
            _flock.currOutpost = this;
        if (captureFlocks != null)
        {
            if (captureFlocks.Contains(_flock))
                return false;
        }
        switch (currType)
        {
            case 0:
                {
                    //no one

                    //is it being captured?
                    if (capturing)
                    {
                        if (captureSideModifier == _flock.side)
                        {
                            //help capture

                            capturing = true;
                            captureSideModifier = _flock.side;
                            captureFlocks.Add(_flock);
                            _flock.targetPos = transform.position;
                            _flock.currOutpost = this;
                        }
                        //else {
                            //attack enemies
                            //ai will handle in it's own update so do nothing for now
                        //}
                    }
                    else
                    {
                        //start capturing
                        capturing = true;
                        captureSideModifier = _flock.side;
                        captureFlocks.Add(_flock);
                        _flock.targetPos = transform.position;
                        _flock.currOutpost = this;

                    }
                    break;
                }
            case 1:
                {
                    //red side
                    StopCapturing();
                    break;
                }
            case -1:
                {
                    //blue side
                    StopCapturing();
                    break;
                }
        }

        return true;


        

    }


    public void StopCapturing()
    {
        currType = 0;
        capturing = false;
        captureSideModifier = 0;
        for (int i = 0; i < captureFlocks.Count; i++)
        {

            captureFlocks[i].currAI_State = (int)Flock.AI_States.Moving;
            captureFlocks[i].targetPos = Vector3.zero;
            
        }
        captureFlocks.Clear();
    }
}
