using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ManualHeight : MonoBehaviour
{
    [SerializeField]
    Transform manualHeight;
    [SerializeField]
    Transform eyeHeight;
    [SerializeField]
    SteamVR_Camera cam = null;

    private void OnEnable()
    {
        if (cam == null)
            cam = GetComponent<SteamVR_Camera>();
        //foreach (Hand hnd in Player.instance.hands)
        //{
        //    Vector3 newPosition = hnd.transform.position;
        //    newPosition.y = manualHeight.position.y;
        //    hnd.transform.position = newPosition;
        //}
        Vector3 newPosition = Player.instance.trackingOriginTransform.position;
        newPosition.y = manualHeight.position.y;
        Player.instance.trackingOriginTransform.position = newPosition;
        Vector3 newtempPosition = cam.head.position;
        newtempPosition.y = eyeHeight.position.y;
        cam.head.position = newtempPosition;
    }
    private void OnDisable()
    {
        if (cam == null)
            cam = GetComponent<SteamVR_Camera>();
        //foreach (Hand hnd in Player.instance.hands)
        //{
        //    Vector3 newPosition = hnd.transform.position;
        //    newPosition.y = manualHeight.position.y;
        //    hnd.transform.position = newPosition;
        //}
        Vector3 newPosition = Player.instance.trackingOriginTransform.position;
        newPosition.y = manualHeight.position.y;
        Player.instance.trackingOriginTransform.position = newPosition;
        Vector3 newtempPosition = cam.head.position;
        newtempPosition.y = eyeHeight.position.y;
        cam.head.position = newtempPosition;
    }

}
