using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Grind : MonoBehaviour {
    
    [SerializeField]
    CubeCount CC;
    
    ParticleSystem ParticleEffect;

    void Start()
    {
        GameObject temp = GameObject.Find("Sparks Subtle Grindstone");
        if (temp)
        ParticleEffect = temp.GetComponent<ParticleSystem>();
        else
        {
            CC.cubes++;
            gameObject.SetActive(false);

        }
    }
    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.name);
        if (col.name == "Grinder")
        {
            AudioSource audio = col.gameObject.GetComponent<AudioSource>();
            if (audio && !audio.isPlaying)
                AudioManager.instance.PlayGrindingSound(audio, AudioManager.AudioObjectType.Metal, false);
            CC.cubes++;
            ParticleEffect.transform.position = transform.position;
            ParticleEffect.Play();

            if (this.transform.parent.parent.parent.gameObject.layer == 11)
            {
                this.transform.parent.parent.parent.GetComponent<Hand>().controller.TriggerHapticPulse(500);
            }

            gameObject.SetActive(false);
        }
    }
}
