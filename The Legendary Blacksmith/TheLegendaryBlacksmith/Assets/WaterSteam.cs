using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSteam : MonoBehaviour 
{
    [SerializeField]
    ParticleSystem steam;
    [SerializeField]
    AudioSource audio;


    public void ActivateSteam()
    {
        steam.Play();
        if (!audio.isPlaying)
            audio.Play();
    }
	
}
