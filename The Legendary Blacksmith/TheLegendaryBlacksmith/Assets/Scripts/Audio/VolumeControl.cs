using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class VolumeControl : MonoBehaviour
{
    public enum VolumeControls
    {
        Master,
        SFX,
        Music,
        Voice,
        Ambience
    }

    [SerializeField]
    AudioMixer mixer;


    public void SetVolume(VolumeControls type, float value, bool SaveVolume = true)
    {
        value = Mathf.Floor(value * 100.0f) / 100.0f;
        Mathf.Clamp(value, 0.0f, 1.0f);
        if(SaveVolume)
            AudioManager.instance.SaveVolume(type, value);
        value = LineartoDecibel(value);
        switch(type)
        {
            case VolumeControls.Master:
                {
                    SetMaster(value);
                    break;
                }
            case VolumeControls.SFX:
                {
                    SetSFX(value);
                    break;
                }
            case VolumeControls.Music:
                {
                    SetMusic(value);
                    break;
                }
            case VolumeControls.Ambience:
                {
                    SetAmbience(value);
                    break;
                }
            case VolumeControls.Voice:
                {
                    SetVoice(value);
                    break;
                }
            default:
                break;
        }
    }
    public void SetSFX(float newVal)
    {
        mixer.SetFloat("sfxVol", newVal);
    }

    public void SetMusic(float newVal)
    {
        mixer.SetFloat("musicVol", newVal);
    }

    public void SetAmbience(float newVal)
    {
        mixer.SetFloat("ambVol", newVal);
    }

    public void SetMaster(float newVal)
    {
        mixer.SetFloat("masterVol", newVal);
    }

    public void SetVoice(float newVal)
    {
        mixer.SetFloat("voiceVol", newVal);
    }

    public float DecibeltoLinear(float decibel)
    {
        float linear = Mathf.Pow(10.0f, decibel / 20.0f);

        return linear;
    }

    public float LineartoDecibel(float linear)
    {
        float decibel = 0.0f;
        if (linear != 0)
            decibel = 20.0f * Mathf.Log10(linear);
        else
            decibel = -80.0f;

        return decibel;
    }

}
