using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRingHitBoxThingies : MonoBehaviour
{
    [SerializeField]
    ShieldRingStuff ring;

    [SerializeField]
    AudioSource audio;
	// Use this for initialization
    void OnTriggerEnter(Collider entity)
    {
        if (entity.name == "Head" && ring.canDoTheThing)
        {
            if (audio)
                AudioManager.instance.PlayCollisionSound(audio, AudioManager.AudioInteractionType.Thud, AudioManager.AudioObjectType.Metal, AudioManager.AudioObjectType.Metal);
            ring.DoLongListOfSorrow(gameObject.name);
        }
    } 
}
