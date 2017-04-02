using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilSharpener : MonoBehaviour {


    AudioSource audio;
    [SerializeField]
    AudioClip sharpening, sharpeningDone;
    bool sharpeningNow = false;

    [SerializeField]
    private GameObject m_MaterialObject;
    private MeshRenderer m_MeshRenderer;
    [SerializeField]
    private Material m_OriginalMaterial;
    [SerializeField]
    private Material m_WiggleMaterial;

    public bool Occupied = false;
    bool SomethingInsideCheck = false;

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        m_MeshRenderer = m_MaterialObject.GetComponent<MeshRenderer>();
    }

    public void ActivateSharpening(bool temp)
    {
        sharpeningNow = temp;
        StartSharpeningSound(temp);
        
        if (m_MeshRenderer)
        {
            if (temp)
                m_MeshRenderer.material = m_WiggleMaterial;
            else
                m_MeshRenderer.material = m_OriginalMaterial;
        }
    }
    public void StartSharpeningSound(bool sharpen)
    {
        audio.loop = true;
        //if (audio.isPlaying)
        //    audio.Stop();
        if (sharpen)
            audio.clip = sharpening;
        else
        {
            audio.clip = sharpeningDone;
        }
        audio.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        StopSharpening();
    }

    public void StopSharpening()
    {
        audio.Stop();

        if (m_MeshRenderer)
        {
            m_MeshRenderer.material = m_OriginalMaterial;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        SomethingInsideCheck = true;
    }

    private void LateUpdate()
    {
        Occupied = SomethingInsideCheck;
        if (!Occupied)
        {
            if(audio.loop)
                audio.loop = false;
        }

        SomethingInsideCheck = false;
    }
}
