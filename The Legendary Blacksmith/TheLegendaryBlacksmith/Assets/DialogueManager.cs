using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class DialogueManager : MonoBehaviour 
{

    //static bool firstAnvil = true;
    //static bool firstbrokenMetal = true;
    static bool firstUpsetChicken = true;
    static bool firstCool = true;
    static bool firstWork = true;
    static bool firstMonocle = true;
    static bool firstBook = true;
    static bool firstWoodShaver = true;
    static bool[] newWeapons;

    [SerializeField]
    AudioSource MilestheFunnygod;

    [SerializeField]
    AudioClip startofLevel, morale25, morale50, morale75, upsetChicken, swordSharpen, workOrders, monocle, Win, Lost, Shroom, OpponentIntro, Dragon, Phoenix, Spider, Golem, Book, WoodShaver, newWeapon;

    [Tooltip("Each array item must exactly match the corresponding clip in newWeaponClips")]
    [SerializeField]
    List<string> newWeaponPrefabs;


    public delegate void mDelegate();

    public static DialogueManager dialogueInstance = null;

    private void Awake()
    {
        if (dialogueInstance == null)
            dialogueInstance = this;
        else if (dialogueInstance != this)
            Destroy(gameObject);

        newWeapons = new bool[newWeaponPrefabs.Count];
        for (int i = 0; i < newWeapons.Length; ++i)
            newWeapons[i] = true;
    }

    private void Start()
    {
        EnterShop();
        BeginBattle();
    }

    public void EnterShop()
    {
        StartCoroutine(PlayAudio(startofLevel));
    }

    public void Moraleat25()
    {
        StartCoroutine(PlayAudio(morale25));

    }

    public void Moraleat50()
    {
        StartCoroutine(PlayAudio(morale50));

    }

    public void Moraleat75()
    {
        StartCoroutine(PlayAudio(morale75));

    }

    public void UpsettheChicken()
    {
        if (firstUpsetChicken)
        {
            firstUpsetChicken = false;
            StartCoroutine(PlayAudio(upsetChicken));
        }
    }

    public void SharpentheSword()
    {
        if (firstCool)
        {
            firstCool = false;
            StartCoroutine(PlayAudio(swordSharpen));
        }
    }

    public void NoticeworkOrders()
    {
        if(firstWork)
        {
            firstWork = false;
            StartCoroutine(PlayAudio(workOrders));
        }
    }

    public void GrabMonocle()
    {
        if (firstMonocle)
        {
            firstMonocle = false;
            StartCoroutine(PlayAudio(monocle));
        }
    }

    public void Winning()
    {
        StartCoroutine(PlayAudio(Win));
    }

    public void Losing()
    {
        StartCoroutine(PlayAudio(Lost));
    }

    public void GrabShroom()
    {
        StartCoroutine(PlayAudio(Shroom));
    }

    public void BeginBattle()
    {
        StartCoroutine(PlayAudio(OpponentIntro));
    }

    public void DragonIntro()
    {
        StartCoroutine(PlayAudio(Dragon));
    }

    public void PhoenixIntro()
    {
        StartCoroutine(PlayAudio(Phoenix));
    }

    public void SpiderIntro()
    {
        StartCoroutine(PlayAudio(Spider));
    }

    public void GolemIntro()
    {
        StartCoroutine(PlayAudio(Golem));
    }

    public void GrabBook()
    {
        if (firstBook)
        {
            firstBook = false;
            StartCoroutine(PlayAudio(Book));
        }
    }

    public void GrabWoodShaver()
    {
        if (firstWoodShaver)
        {
            firstWoodShaver = false;
            StartCoroutine(PlayAudio(WoodShaver));
        }
    }

    public void ActivateNewWeapon(string newObjectName)
    {
        if(newWeaponPrefabs.Contains(newObjectName))
        {
            int index = newWeaponPrefabs.IndexOf(newObjectName);
            if(newWeapons[index])
            {
                newWeapons[index] = false;
                StartCoroutine(PlayAudio(newWeapon));
            }
        }
    }


    IEnumerator PlayAudio(AudioClip cliptoPlay, float timeToWait = 1.0f)
    {
        if (!cliptoPlay)
            yield break;
        while (true)
        {
            while (MilestheFunnygod.isPlaying)
                yield return null;
            yield return new WaitForSeconds(timeToWait);
            if (MilestheFunnygod.isPlaying)
                yield return null;
            else
                break;
        }
        MilestheFunnygod.PlayOneShot(cliptoPlay);
    }

}
