using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilTopController : MonoBehaviour
{
    private void OnTriggerEnter(Collider entity)
    {
        if (entity.tag == "ForgeItem")
        {
            HammerTime temp = entity.GetComponent<HammerTime>();
            if (temp)
            {
                if((temp.isMalleable || temp.isBrittle))
                temp.OnAnvil = true;
            }
        }
    }

    private void OnTriggerExit(Collider entity)
    {
        if (entity.tag == "ForgeItem")
        {
            HammerTime temp = entity.GetComponent<HammerTime>();
            if (temp)
            {
                if ((temp.isMalleable || temp.isBrittle))
                    temp.OnAnvil = false;
            }
        }
    }
}
