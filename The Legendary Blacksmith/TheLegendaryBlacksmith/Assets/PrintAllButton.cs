using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PrintAllButton : MonoBehaviour {
    [SerializeField]
    PrintAllController PAC;

    [SerializeField]
    ButtonPush BP;
    [SerializeField]
    GameObject Lid;

    Quaternion closeLid;
    CircularDrive CD;

    //Ben
    //Particle access
    [SerializeField]
    UnitSpawnParticles particles;
    //

    // Use this for initialization
    void Start () {
        CD = Lid.gameObject.GetComponent<CircularDrive>();
        closeLid.x = 0;
        closeLid.y = 0;
        closeLid.z = 0;
    }

    ///Edited
    /// </Benjamin Ousley>
    /// Made the chest/particles close/activate exclusively when a unit is printed
    /// Did this to make the chest close and the particles activate exclusively when a unit is actually printed
    /// Portions of this edit marked with 
    /// //Ben (Bool)
    /// content
    /// //
    /// </1/27/2017>
    void OnTriggerExit(Collider Entity)
    {
        if (BP.buttonOn == true)
        {
            //Ben(Bool)
            if (PAC.PrintAll())
            {
                Lid.gameObject.transform.rotation = closeLid;
                CD.outAngle = 0;
                particles.StartParticles();
            }
            //
        }
    }
}
