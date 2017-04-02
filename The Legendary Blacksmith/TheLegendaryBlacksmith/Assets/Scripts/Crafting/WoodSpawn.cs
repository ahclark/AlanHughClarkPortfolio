using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpawn : MonoBehaviour
{
    [SerializeField]
    bool pickedUp = false;

    // Use this for initialization
    void Start ()
    {
        this.name = "Wood";

        LevelBlockingTools.LevelBlockingMesh bark = GetComponentInChildren<LevelBlockingTools.LevelBlockingMesh>();
        if (bark)
        {
            DestroyImmediate(bark.meshFilter.sharedMesh);
            bark.CreateMesh();
            bark.UpdateMesh();
        }

    }
    
    public void PickUp()
    {
        if (pickedUp == false)
        {
            pickedUp = true;
        }
    }
}
