using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShieldRingStuff : MonoBehaviour
{
    [SerializeField]
    GameObject SolidTop;
    [SerializeField]
    GameObject[] flatTops;
    [SerializeField]
    GameObject[] angleRs;
    [SerializeField]
    GameObject[] angleLs;
    [SerializeField]
    GameObject[] flatBots;
    [SerializeField]
    GameObject SolidBot;
    [SerializeField]
    GameObject[] hitPoints;
    [SerializeField]
    Transform measurements;
    public ShieldMaker shield;

    public bool canDoTheThing = false;

    int hitThing;

    [SerializeField]
    int partsDone = 0;

    void Start()
    {
        
        foreach (GameObject disable in angleRs)
        {
            disable.SetActive(false);
        }
        foreach (GameObject disable in angleLs)
        {
            disable.SetActive(false);
        }
        foreach (GameObject disable in flatBots)
        {
            disable.SetActive(false);
        }
        
        SolidBot.SetActive(false);

    }

    public void DoLongListOfSorrow(string thing)
    {
        for (int i = 0; i < hitPoints.Length; i++)
        {
            if (thing == hitPoints[i].name)
            {
                hitThing = i;
            }
        }
        switch (hitThing)
        {
            case 0:
                DoLeftOnes();
                break;
            case 1:
                DoRightOnes();
                break;
            case 2:
                DoLeftOnes();
                break;
            case 3:
                DoRightOnes();
                break;
            case 4:
                DoLeftOnes();
                break;
            case 5:
                DoRightOnes();
                break;
            case 6:
                DoLeftOnes();
                break;
            case 7:
                DoRightOnes();
                break;
            case 8:
                DoLeftOnes();
                break;
            case 9:
                DoRightOnes();
                break;
            case 10:
                DoLeftOnes();
                break;
            case 11:
                if (flatTops[hitThing].activeInHierarchy == true)
                {
                    flatTops[hitThing].SetActive(false);
                    angleRs[hitThing].SetActive(true);
                }
                else
                {
                    angleRs[hitThing].SetActive(false);
                    angleLs[hitThing].SetActive(false);
                    flatBots[hitThing].SetActive(true);
                    partsDone++;

                }
                if (flatTops[0].activeInHierarchy == true)
                {
                    flatTops[0].SetActive(false);
                    angleRs[0].SetActive(true);
                }
                else
                {
                    angleRs[0].SetActive(false);
                    angleLs[0].SetActive(false);
                    flatBots[0].SetActive(true);
                    partsDone++;

                }
                hitPoints[hitThing].SetActive(false);
                break;
            default:
                break;

        }
        LastChance();

    }
    void DoLeftOnes()
    {

        if (flatTops[hitThing].activeInHierarchy == true)
        {
            flatTops[hitThing].SetActive(false);
            angleLs[hitThing].SetActive(true);
        }
        else
        {
            angleLs[hitThing].SetActive(false);
            angleRs[hitThing].SetActive(false);
            flatBots[hitThing].SetActive(true);
            partsDone++;

        }
        if (flatTops[hitThing + 1].activeInHierarchy == true)
        {
            flatTops[hitThing + 1].SetActive(false);
            angleLs[hitThing + 1].SetActive(true);
        }
        else
        {
            angleLs[hitThing + 1].SetActive(false);
            angleRs[hitThing + 1].SetActive(false);
            flatBots[hitThing + 1].SetActive(true);
            partsDone++;
        }
        hitPoints[hitThing].SetActive(false);
    }
    void DoRightOnes()
    {
        if (flatTops[hitThing].activeInHierarchy == true)
        {
            flatTops[hitThing].SetActive(false);
            angleRs[hitThing].SetActive(true);
        }
        else
        {
            angleRs[hitThing].SetActive(false);
            angleLs[hitThing].SetActive(false);
            flatBots[hitThing].SetActive(true);
            partsDone++;

        }
        if (flatTops[hitThing + 1].activeInHierarchy == true)
        {
            flatTops[hitThing + 1].SetActive(false);
            angleRs[hitThing + 1].SetActive(true);
        }
        else
        {
            angleRs[hitThing + 1].SetActive(false);
            angleLs[hitThing + 1].SetActive(false);
            flatBots[hitThing + 1].SetActive(true);
            partsDone++;
        }
        hitPoints[hitThing].SetActive(false);
    }
    public void ResetPosition()
    {
        transform.localPosition = measurements.localPosition;
        transform.localRotation = measurements.localRotation;
        transform.localScale = measurements.localScale;
    }
    void Terminate()
    {
        Debug.Log("terminate");
        if (shield.gameObject.transform.parent)
        {
            shield.gameObject.transform.parent.gameObject.GetComponent<Hand>().DetachObject(shield.gameObject);
        }
        shield.GetComponent<Rigidbody>().isKinematic = false;
        Destroy(gameObject);
    }
    public void LastChance()
    {
        
        if (partsDone == 12)
        {
            //SolidBot.SetActive(true);
            if (shield.gameObject.transform.parent)
            {
                shield.gameObject.transform.parent.gameObject.GetComponent<Hand>().DetachObject(shield.gameObject);
            }
            SolidBot.transform.parent = shield.gameObject.transform;
            shield.gameObject.name = "Shield_Rimmed";
            shield.RimJob();
            //Terminate();
        }
    }
}
