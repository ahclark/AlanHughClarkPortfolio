using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour {

    public OutpostController[] outposts;
    public GameObject bluePortal, redPortal;
    private void Start()
    {
        
        bluePortal.SetActive(false);
        redPortal.SetActive(false);
    }


    public bool AllOutpostsCaptured(int side)
    {
        int same = 0;
        for (int i = 0; i < outposts.Length; i++)
        {
            same += outposts[i].side;
        }

        if(same == outposts.Length && side == 1)
        {
            if (!bluePortal.activeInHierarchy)
            {
                bluePortal.SetActive(true);
            }
            return true;
        }
        else if(same == -(outposts.Length) && side == -1)
        {
            if (!redPortal.activeInHierarchy)
            {
                redPortal.SetActive(true);
            }
            return true;
        }



        return false;

    }
    




}
