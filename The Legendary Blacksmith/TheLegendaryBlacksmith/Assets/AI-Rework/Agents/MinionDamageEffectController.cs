using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDamageEffectController : MonoBehaviour {

    public ParticleSystem damageEffect1, damageEffect2;

    public void Play()
    {
        if (damageEffect1)
        {
           // if(!damageEffect1.isPlaying)
            damageEffect1.Play();
        }
        if (damageEffect2)
        {
           // if (!damageEffect2.isPlaying)
                damageEffect2.Play();
        }
    }


}
