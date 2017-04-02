using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBoxButtons : MonoBehaviour 
{

    [SerializeField]
    ButtonPush BP;

    [SerializeField]
    VolumeControl.VolumeControls type;

    [SerializeField]
    JukeBoxManagement JukeBox;

    void OnTriggerExit(Collider Entity)
    {
        if (BP.buttonOn == true)
        {
            JukeBox.ChangeType(type);
        }
    }
}
