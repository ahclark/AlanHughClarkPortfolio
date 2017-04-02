using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ScrapBar : MonoBehaviour
{
    public bool upR = false;
    public bool downR = false;
    public bool upL = false;
    public bool downL = false;
    public bool CanBeHammered;
    [SerializeField]
    GameObject shieldHandle;
    [SerializeField]
    GameObject upsidedown;
    [SerializeField]
    GameObject Hilt;
    bool onAnvil = false;
    [SerializeField]
    ForgeHeat FH;

    AudioSource audio;

    void Start()
    {
        FH = gameObject.GetComponent<ForgeHeat>();
        audio = GetComponent<AudioSource>();
    }
    public void HammerMe()
    {
        if (upR && upL && !FH.Heated)
        {
            Instantiate(shieldHandle, transform.position, upsidedown.transform.rotation).name = "Shield_Handle";
            Hand temp = transform.parent.gameObject.GetComponent<Hand>();
            if (temp)
            {
                temp.DetachObject(gameObject);
            }
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        if (downR && downL && !FH.Heated)
        {
            Instantiate(shieldHandle, transform.position, transform.rotation).name = "Shield_Handle";
            Hand temp = transform.parent.gameObject.GetComponent<Hand>();
            if (temp)
            {
                temp.DetachObject(gameObject);
            }
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Head" && onAnvil && !FH.Heated)
        {
            if (audio)
                AudioManager.instance.PlayCollisionSound(audio, AudioManager.AudioInteractionType.Bang, AudioManager.AudioObjectType.Metal, AudioManager.AudioObjectType.Metal);
            if (transform.parent)
            {
                Hand temp = transform.parent.gameObject.GetComponent<Hand>();
                if (temp)
                {
                    temp.DetachObject(gameObject);
                }
            }
            Instantiate(Hilt, gameObject.transform.position, gameObject.transform.rotation).name = Hilt.name;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.name == "Top")
        {
            onAnvil = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Top")
        {
            onAnvil = false;
        }
    }
}
