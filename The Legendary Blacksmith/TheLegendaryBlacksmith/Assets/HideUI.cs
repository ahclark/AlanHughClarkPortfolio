using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HideUI : MonoBehaviour 
{

    // Use this for initialization
    Hand hand;
    bool active = true;
    [SerializeField]
    GameObject playerUI;

    private void Start()
    {
        hand = GetComponent<Hand>();
    }

    private void Update()
    {
        if (hand.controller != null && 
            hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            playerUI.SetActive(active);
            active = !active;
        }
    }

}
