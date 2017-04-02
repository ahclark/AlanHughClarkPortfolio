using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostController : MonoBehaviour {




    public int side = 0;
    public float captureAmount = 0;
    public float captureLimit = 0;


    [Header("Materials")]
    [SerializeField]
    Material Red;
    [SerializeField]
    Material Blue;
    [SerializeField]
    Material White;

    [SerializeField]
    MeshRenderer toChangeMesh;

    public GameObject portal = null;




    public bool CaptureMe(float _amount, int _side)
    {
        captureAmount += (_amount * _side);
        if(Mathf.Abs(captureAmount) >= captureLimit)
        {
            captureAmount = captureLimit * _side;
            
            if (_side == 1 && side == 0)
            {
                toChangeMesh.material = Red;
            }
            else if(_side == -1 && side == 0)
            {
                toChangeMesh.material = Blue;
            }
            side = _side;

            if (portal)
            {
                portal.SetActive(true);
            }
            return true;
        }
        else
        {
            side = 0;

            toChangeMesh.material = White;


            if (portal)
            {
                portal.SetActive(false);
            }
            return false;
        }
    }

}
