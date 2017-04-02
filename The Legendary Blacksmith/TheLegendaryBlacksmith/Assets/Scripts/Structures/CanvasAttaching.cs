using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class CanvasAttaching : MonoBehaviour
{


    Camera camMain;
    [SerializeField]
    float offset = 0.0f;
    [SerializeField]
    bool LookAt = false;
    // Use this for initialization
    private void Start()
    {
        camMain = Camera.main;
    }

    private void LateUpdate()
    {
        Ray camRay = camMain.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 temp = camRay.GetPoint(offset);
        temp.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, temp, Time.deltaTime * 10.0f);
        Vector3 tempLooking = 2 * transform.position - camMain.transform.position;
        //if (camMain.transform.rotation.x >= 0.5f)
        //    tempLooking *= -1;
        if (!LookAt)
            tempLooking.y = transform.position.y;

        if (LookAt)
            transform.LookAt(tempLooking, camMain.transform.up);
        else
            transform.LookAt(tempLooking);
    }



}
