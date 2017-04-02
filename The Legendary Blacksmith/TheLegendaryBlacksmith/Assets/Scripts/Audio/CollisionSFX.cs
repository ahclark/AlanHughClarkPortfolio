using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Phonon.PhononEffect))]

public class CollisionSFX : MonoBehaviour
{

    [SerializeField]
    public AudioManager.AudioObjectType soundType;

    private AudioSource audio;

    [SerializeField]
    AudioMixerGroup mixer;

    [SerializeField]
    string specialItem;

    [SerializeField]
    AudioClip specialSound;
    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        if (audio)
            audio.outputAudioMixerGroup = mixer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (audio)
        {
            if (!audio.isPlaying)
            {
                if (specialSound)
                {
                    audio.PlayOneShot(specialSound);
                    return;
                }
                if (collision.gameObject.name == specialItem)
                {
                    if (audio.clip != null)
                        audio.Play();
                    return;
                }
                if (collision.gameObject.GetComponent<CollisionSFX>())
                {
                    AudioManager.AudioObjectType temp = collision.gameObject.GetComponent<CollisionSFX>().soundType;
                    Debug.Log(collision.gameObject.name + ": " + temp);
                    AudioManager.instance.PlayCollisionSound(audio, AudioManager.AudioInteractionType.Thud, soundType, temp);
                }
                else
                {
                    Phonon.PhononMaterial tempMat = collision.gameObject.GetComponent<Phonon.PhononMaterial>();
                    Debug.Log(collision.gameObject.name + ": " + tempMat);
                    if (tempMat)
                    {
                        AudioManager.AudioObjectType temp;
                        #region PhononMaterial
                        switch (tempMat.Preset)
                        {
                            case Phonon.PhononMaterialPreset.Wood:
                                {
                                    temp = AudioManager.AudioObjectType.Wood;
                                    break;
                                }
                            case Phonon.PhononMaterialPreset.Metal:
                                {
                                    temp = AudioManager.AudioObjectType.Metal;
                                    break;
                                }
                            case Phonon.PhononMaterialPreset.Rock:
                                {
                                    temp = AudioManager.AudioObjectType.Rock;
                                    break;
                                }
                            case Phonon.PhononMaterialPreset.Gravel:
                                {
                                    temp = AudioManager.AudioObjectType.Rock;
                                    break;
                                }
                            case Phonon.PhononMaterialPreset.Concrete:
                                {
                                    temp = AudioManager.AudioObjectType.Rock;
                                    break;
                                }
                            case Phonon.PhononMaterialPreset.Brick:
                                {
                                    temp = AudioManager.AudioObjectType.Rock;
                                    break;
                                }
                            case Phonon.PhononMaterialPreset.Ceramic:
                                {
                                    temp = AudioManager.AudioObjectType.Rock;
                                    break;
                                }
                            case Phonon.PhononMaterialPreset.Carpet:
                                {
                                    temp = AudioManager.AudioObjectType.Terrain;
                                    break;
                                }
                            default:
                                {
                                    temp = AudioManager.AudioObjectType.Building;
                                    break;
                                }
                        }
                        #endregion
                        AudioManager.instance.PlayCollisionSound(audio, AudioManager.AudioInteractionType.Thud, soundType, temp);
                    }
                    else
                        AudioManager.instance.PlayCollisionSound(audio, AudioManager.AudioInteractionType.Thud, soundType);
                }
            }
        }
    }


}
