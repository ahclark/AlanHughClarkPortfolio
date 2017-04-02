using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSegments : MonoBehaviour
{
    [SerializeField]
    SwordPuck parent;
    [SerializeField]
    SwordPuck SP;
    [SerializeField]
    ForgeHeat FH;
    [SerializeField]
    SwordDownLerp SDL;
    [SerializeField]
    HammerCheck HC;
    AudioSource audio;
    [SerializeField]
    int whichSegment;
    bool Stop;
    
    [SerializeField]
    GameObject m_hammer;
    HammerHitController m_hammerHitController;

    private void Start()
    {
        Stop = false;
        audio = GetComponent<AudioSource>();

        m_hammer = GameObject.Find("Hammer");
        if (m_hammer)
            m_hammerHitController = m_hammer.GetComponent<HammerHitController>();
    }
    void OnTriggerEnter(Collider entity)
    {
        if (entity.name == "Head" && SP.onAnvil && FH.Heated == true && SDL.brittle == false /*&& HC.HammerReady*/ && !Stop)
        {
            if (m_hammerHitController)
            {
                Debug.Log("Hammer Hit Controller");
                if (m_hammerHitController.CheckVelocity(0.0f))
                {
                    Debug.Log("Check Velocity");
                    m_hammerHitController.PlaySparks();
                    m_hammerHitController.VibrateController(3999);
                }
                else
                    return;
            }

            //HC.HammerReady = false;
            if (SP.counters[whichSegment] >= 3)
            {
                SDL.hit = true;
                Debug.Log("previous line is hit activation");
                Stop = true;
                
            }
            if(audio)
                AudioManager.instance.PlayCollisionSound(audio, AudioManager.AudioInteractionType.Bang, AudioManager.AudioObjectType.Metal, AudioManager.AudioObjectType.Metal);
            parent.SlamSegment(gameObject.name);
            SP.counters[whichSegment]++;
        }
    }
}
