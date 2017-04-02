using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ChickenController : MonoBehaviour
{
    [SerializeField]
    Hand TheHand;
    [SerializeField]
    GameObject feather;
    Animator chicken;
    public GameObject seed;
    public bool food = false;

    [SerializeField]
    AudioClip[] chickenIdle;

    [SerializeField]
    AudioClip chickenPissed;

    AudioSource audio;

    [SerializeField]
    int numberOfPluckings = 5;

    int pluckedFeathers = 0;
    bool eat, pet, upset = false;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        chicken = GetComponent<Animator>();
        TheHand = null;
    }

    public void PluckChicken()
    {
        if(TheHand && !upset)
        {
            if (audio)
                audio.PlayOneShot(chickenPissed);
            chicken.SetTrigger("Pluck");
            GameObject newObject = Instantiate(feather);
            newObject.name = "Arrow_Feathers";
            TheHand.AttachObject(newObject);
            //Benjamin Ousley
            //Determine whether chicken is upset
            pluckedFeathers++;
            if (pluckedFeathers >= numberOfPluckings)
            {
                upset = true;
                chicken.SetBool("Upset", upset);
                if (DialogueManager.dialogueInstance)
                    DialogueManager.dialogueInstance.UpsettheChicken();
            }
        }
    }

    private void OnHandHoverEnd(Hand hand)
    {
        TheHand = null;
    }

    private void OnHandHoverBegin(Hand hand)
    {
        TheHand = hand;
    }

    public void CalmChicken()
    {
        chicken.SetBool("Calm", true);
        if (audio && !audio.isPlaying)
            audio.PlayOneShot(chickenIdle[Random.Range(0, chickenIdle.Length - 1)]);
        //Benjamin Ousley
        //Determine if chicken is happy again
        if (upset)
        {
            pet = true;
            if(pet && eat)
            {
                pet = eat = false;
                chicken.SetBool("Upset", false);
                upset = false;
                pluckedFeathers = 0;
            }
        }
    }
    public void UnCalmChicken()
    {
        chicken.SetBool("Calm", false);
    }
    public void FeedChicken()
    {
        chicken.SetBool("Eat", true);
        if (audio && !audio.isPlaying)
            audio.PlayOneShot(chickenIdle[Random.Range(0, chickenIdle.Length - 1)]);
        //Benjamin Ousley
        //Determine if chicken is happy again
        if (upset)
        {
            eat = true;
            if (pet && eat)
            {
                pet = eat = false;
                chicken.SetBool("Upset", false);
                upset = false;
                pluckedFeathers = 0;
            }
        }
    }
    public void UnFeedChicken()
    {
        chicken.SetBool("Eat", false);
    }
    public void Consume()
    {
        if (seed.transform.parent)
        {
            seed.transform.parent.gameObject.GetComponent<Hand>().DetachObject(seed);
        }
        seed.SetActive(false);
        Destroy(seed);
        UnFeedChicken();
    }
}
