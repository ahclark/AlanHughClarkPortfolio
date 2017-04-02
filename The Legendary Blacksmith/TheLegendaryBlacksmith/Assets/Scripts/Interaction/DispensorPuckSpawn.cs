using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispensorPuckSpawn : MonoBehaviour
{
    [SerializeField]
    DispenserController Dispenser;

    private void OnTriggerEnter(Collider entity)
    {
        if (entity.tag == "dispenserbar")
        {
            Dispenser.SpawnPuck();
        }
    }
}
