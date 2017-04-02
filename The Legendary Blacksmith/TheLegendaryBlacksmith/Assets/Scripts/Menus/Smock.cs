using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Smock : MonoBehaviour
{
    [SerializeField]
    bool on = false;

    void OnTriggerEnter(Collider entity)
    {
        if (entity.name == "Face" && on)
        {
            SteamVR_LoadLevel.Begin("Main");
        }
    }

    public void TurnOn()
    {
        on = true;
    }
    public void TurnOff()
    {
        on = false;
    }
}
