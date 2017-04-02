using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkController : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_FireworkPrefabs;
    
    public float m_timeToTravel;
    float m_timer;
    bool m_buffer;
    
    public float m_speed;

    [SerializeField]
    ParticleSystem m_particleSystem;

    public AudioSource m_audioSource;
    [SerializeField]
    AudioClip m_whistle;
    [SerializeField]
    AudioClip m_bang;

    private void Start()
    {
        m_timer = 0.0f;
        m_buffer = true;
        //m_particleSystem = GetComponent<ParticleSystem>();
        if (m_particleSystem)
            m_particleSystem.Play();

        m_audioSource = this.GetComponent<AudioSource>();
        if (m_audioSource)
        {
            m_audioSource.clip = m_whistle;
            m_audioSource.pitch += Random.Range(-0.5f, 0.5f);
            m_audioSource.volume = 0.5f;
            m_audioSource.Play();
        }
    }

    private void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= m_timeToTravel && m_buffer)
        {
            m_buffer = false;
            int temp = Random.Range(0, m_FireworkPrefabs.Length);
            m_FireworkPrefabs[temp].SetActive(true);
            Debug.Log("Pause");
            if (m_particleSystem)
                m_particleSystem.Stop();
            if (m_audioSource)
            {
                m_audioSource.clip = m_bang;
                m_audioSource.pitch += Random.Range(-0.5f, 0.5f);
                m_audioSource.volume = 0.15f;
                m_audioSource.Play();
            }
        }
        else if (m_buffer)
        {
            this.transform.Translate(0.0f, 0.0f, m_speed * Time.deltaTime);
        }
    }
}
