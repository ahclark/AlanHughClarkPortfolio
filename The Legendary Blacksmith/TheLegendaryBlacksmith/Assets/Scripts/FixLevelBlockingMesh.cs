using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixLevelBlockingMesh : MonoBehaviour
{
    public bool DoUpdate = true;

	// Use this for initialization
	void Start ()
    {
        LevelBlockingTools.LevelBlockingMesh showThis = GetComponent<LevelBlockingTools.LevelBlockingMesh>();
        LevelBlockingTools.LevelBlockingMesh showAll = GetComponentInChildren<LevelBlockingTools.LevelBlockingMesh>();
        if (showAll)
        {
            DestroyImmediate(showAll.meshFilter.sharedMesh);
            showAll.CreateMesh();
            showAll.UpdateMesh();
        }
        if (showThis)
        {
            DestroyImmediate(showThis.meshFilter.sharedMesh);
            showThis.CreateMesh();
            showThis.UpdateMesh();
        }
    }
    void Update()
    {
        if (DoUpdate)
        {
            GetComponent<LevelBlockingTools.LevelBlockingMesh>().UpdateMesh();
        }
    }
}
