using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshLbms : MonoBehaviour
{

    // Use this for initialization
    private void Start()
    {
        LevelBlockingTools.LevelBlockingMesh mLBM = gameObject.GetComponent<LevelBlockingTools.LevelBlockingMesh>();
        if (mLBM)
        {
            DestroyImmediate(mLBM.meshFilter.sharedMesh);
            mLBM.CreateMesh();
            mLBM.UpdateMesh();
        }
        int children = gameObject.transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            LevelBlockingTools.LevelBlockingMesh lbm = child.GetComponent<LevelBlockingTools.LevelBlockingMesh>();
            if (lbm)
            {
                DestroyImmediate(lbm.meshFilter.sharedMesh);
                lbm.CreateMesh();
                lbm.UpdateMesh();
            }
        }
    }
}
