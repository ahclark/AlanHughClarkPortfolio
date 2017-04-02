using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VendorDanceAfterWin : MonoBehaviour
{

    static public VendorDanceAfterWin instance = null;

    [SerializeField]
    List<UnitAnimationController> vendorAnims;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void Winning()
    {
        foreach (UnitAnimationController anim in vendorAnims)
        {
            anim.SetType(8);
        }
    }

    public void Fighting()
    {
        foreach (UnitAnimationController anim in vendorAnims)
        {
            anim.SetType(4);
        }
    }

}
