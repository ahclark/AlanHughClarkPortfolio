using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSetter : MonoBehaviour
{
    [SerializeField]
    string boolOne;

    public void TurnOnBool()
    {
            GetComponent<Animator>().SetBool(boolOne, true);
    }
    public void TurnOffBool()
    {
            GetComponent<Animator>().SetBool(boolOne, false);
    }
}
