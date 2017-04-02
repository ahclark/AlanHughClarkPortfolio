using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScrollParent : MonoBehaviour
{
    public int counter = 0;
    public int limit = 3;
    virtual protected void Start()
    {
        counter = 0;
    }

    virtual public void ScrollLeft()
    {
        --counter;
        if (counter < 0)
            counter = limit - 1;
        Activate();
    }

    virtual public void ScrollRight()
    {
        ++counter;
        if (counter >= limit)
            counter = 0;
        Activate();
    }

    virtual protected void Activate()
    {
    }
}
