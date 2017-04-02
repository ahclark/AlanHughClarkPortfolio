using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetShaverThing : MonoBehaviour
{
    [SerializeField]
    WoodShaving resetter;

    public void ResetThing()
    {
        resetter.thing = null;
    }
}
