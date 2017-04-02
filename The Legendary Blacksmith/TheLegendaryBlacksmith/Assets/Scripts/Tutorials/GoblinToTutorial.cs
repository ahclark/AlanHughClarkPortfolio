using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoblinToTutorial : MonoBehaviour
{
    public UnityEvent one;
    public UnityEvent two;
    public UnityEvent three;
    public UnityEvent four;
    public UnityEvent five;

    public void ActivateOne()
    {
        one.Invoke();
    }
    public void ActivateTwo()
    {
        two.Invoke();
    }
    public void ActivateThree()
    {
        three.Invoke();
    }
    public void ActivateFour()
    {
        four.Invoke();
    }
    public void ActivateFive()
    {
        five.Invoke();
    }
}
