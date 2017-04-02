using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenButtonScrollLeft : MonoBehaviour
{

    [SerializeField]
    InventoryManager InvMan;
    [SerializeField]
    ButtonPush BP;
    [SerializeField]
    WeaponStorageController[] Spheres;
    float timer;


    void Start()
    {
        timer = 0;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }

    private void OnTriggerExit(Collider entity)
    {
        if (BP.buttonOn == true && timer > 1)
        {
            timer = 0;
            InvMan.screen--;
            if (InvMan.screen < 0)
            {
                InvMan.screen = InvMan.Screens.Length-1;
            }
            foreach (WeaponStorageController thing in Spheres)
            {
                thing.Render();
            }
            InvMan.RenderNumber();
        }
    }
}
