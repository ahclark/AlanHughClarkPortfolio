using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class SnapToHand : MonoBehaviour
{
    bool m_held;
    Hand m_hand;
    Interactable m_interactable;
    HandButtonEvents m_handButtonEvents;
    Rigidbody m_rigidbody;

    [SerializeField]
    GameObject m_smileSprite;
    [SerializeField]
    GameObject m_giggleSprite;
    [SerializeField]
    GameObject m_screamSprite;

    AudioSource m_audioSource;
    [SerializeField]
    AudioClip m_smileSound;
    [SerializeField]
    AudioClip m_giggleSound;
    [SerializeField]
    AudioClip m_screamSound;
    bool m_screaming;
    bool m_giggling;

    [SerializeField]
    GameObject m_Firework;
    [SerializeField]
    Transform m_firePosition;

    public UnityEvent TriggerPulled;

    private void Start()
    {
        m_held = false;
        m_hand = null;
        m_interactable = this.GetComponent<Interactable>();
        m_handButtonEvents = this.GetComponent<HandButtonEvents>();
        m_rigidbody = this.GetComponent<Rigidbody>();
        m_audioSource = this.GetComponent<AudioSource>();
        m_screaming = false;
        m_giggling = false;
    }

    private void Update()
    {
        if (m_hand)
        {
            if (m_hand.controller != null)
            {
                if (m_hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
                {
                    TriggerPulled.Invoke();
                    SnapOrShoot();
                }
                if (m_hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
                {
                    Drop();
                }
            }
        }

        if (m_screaming)
        {
            if (!m_audioSource.isPlaying)
            {
                int temp = Random.Range(0, 3);
                if (temp == 0)
                {
                    Giggle();
                }
                else
                {
                    Smile();
                }
            }
        }
        if (m_giggling)
        {
            if (!m_audioSource.isPlaying)
            {
                Smile();
            }
        }
    }

    private void OnHandHoverBegin(Hand hand)
    {
        if (!m_hand)
        {
            m_hand = hand;
        }
    }

    private void OnHandHoverEnd(Hand hand)
    {
        if (hand == m_hand && !m_held)
        {
            m_hand = null;
        }
    }

    public void SnapOrShoot()
    {
        if (!m_held)
        {
            m_held = true;

            if (m_hand)
            {
                m_hand.AttachObject(this.gameObject, Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.ParentToHand);
                //this.transform.parent = m_hand.transform;
                //this.transform.localPosition = Vector3.zero;
                //this.transform.localRotation = Quaternion.identity;
            }
            if (m_rigidbody)
            {
                m_rigidbody.useGravity = false;
                m_rigidbody.isKinematic = true;
            }
        }
        else
        {
            Shoot();
        }
    }

    public void Drop()
    {
        m_held = false;

        if (m_hand)
        {
            m_hand.DetachObject(this.gameObject, false);
            m_hand = null;
        }
        //this.transform.parent = null;
        if (m_rigidbody)
        {
            m_rigidbody.useGravity = true;
            m_rigidbody.isKinematic = false;
        }
    }

    void Shoot()
    {
        Instantiate(m_Firework, m_firePosition.position, m_firePosition.rotation);
        Scream();
    }

    void Smile()
    {
        m_smileSprite.SetActive(true);
        m_screamSprite.SetActive(false);
        m_giggleSprite.SetActive(false);

        m_screaming = false;
        m_giggling = false;
        m_audioSource.clip = m_smileSound;
    }

    void Scream()
    {
        m_smileSprite.SetActive(false);
        m_screamSprite.SetActive(true);
        m_giggleSprite.SetActive(false);

        m_screaming = true;
        m_giggling = false;
        m_audioSource.clip = m_screamSound;
        m_audioSource.Play();
    }

    void Giggle()
    {
        m_smileSprite.SetActive(false);
        m_screamSprite.SetActive(false);
        m_giggleSprite.SetActive(true);

        m_screaming = false;
        m_giggling = true;
        m_audioSource.clip = m_giggleSound;
        m_audioSource.Play();
    }

    public void SetHand(Hand hand)
    {
        m_hand = hand;
    }
}
