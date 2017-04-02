using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class Tutorials : MonoBehaviour
{
    [SerializeField]
    GameObject forgeTutorial;
    [SerializeField]
    GameObject unitTutorial;

    static bool firstForge = true;
    static bool forgeRunning = false;

    static bool firstUnit = true;
    static bool unitRunning = false;

    public static Tutorials tutorialinstance = null;

    Coroutine coroutine = null;

    private void Awake()
    {
        if (tutorialinstance == null)
            tutorialinstance = this;
        else if (tutorialinstance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        if(forgeTutorial)
        {
            forgeTutorial.GetComponent<Animator>().SetBool("Play", true);
            forgeRunning = true;
        }
    }

    public void Tutorial(GameObject attachedObject)
    {
        if (attachedObject.tag == "ForgeItem")
        {
            if (firstForge && !forgeRunning)
            {
                forgeTutorial.SetActive(true);
                forgeTutorial.GetComponent<Animator>().SetBool("Play", true);
                forgeRunning = true;
                firstForge = false;
            }
        }
        else if (attachedObject.tag == "SpawnObject")
        {
            if(firstUnit && !unitRunning)
            {
                unitTutorial.SetActive(true);
                unitTutorial.GetComponent<Animator>().SetBool("Play", true);
                unitRunning = true;
                firstUnit = false;
            }
        }

    }

    public void StopForgeTutorial()
    {
        if (forgeRunning)
        {
            forgeRunning = false;
            forgeTutorial.GetComponent<Animator>().SetBool("Play", false);
            forgeTutorial.SetActive(false);
        }
    }

    public void StopUnitTutorial()
    {
        if(unitRunning)
        {
            unitRunning = false;
            unitTutorial.GetComponent<Animator>().SetBool("Play", false);
            unitTutorial.SetActive(false);
        }
    }



}
