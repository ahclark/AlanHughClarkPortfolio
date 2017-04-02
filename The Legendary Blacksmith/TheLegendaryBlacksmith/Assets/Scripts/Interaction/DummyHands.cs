using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHands : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            anim.SetTrigger("TriggerDown");
            anim.SetBool("TriggerPressed", true);
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            anim.SetTrigger("TriggerUp");
            anim.SetBool("TriggerPressed", false);
        }
        if (!Input.GetKey(KeyCode.N))
        {
            anim.SetTrigger("TriggerUp");
            anim.SetBool("TriggerPressed", false);
        }
    }
}
