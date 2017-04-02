using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSwitchController : ButtonScrollParent {

    [SerializeField]
    GameObject[] Screens;
    // Use this for initialization
    protected override void Start()
    {
        
        base.Start();
        limit = Screens.Length;
        Activate();
    }

    protected override void Activate()
    {
       foreach(GameObject screen in Screens)
        {
            screen.SetActive(false);
        }
        Screens[counter].SetActive(true);
    }


}
