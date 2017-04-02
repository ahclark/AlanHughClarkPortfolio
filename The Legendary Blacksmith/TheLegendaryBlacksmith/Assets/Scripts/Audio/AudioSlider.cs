using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class AudioSlider : MonoBehaviour
{

    AudioManager AudioManager;
    [SerializeField]
    VolumeControl.VolumeControls volType;
    [SerializeField]
    LinearMapping currvalue;
    [SerializeField]
    TextMesh mText;
    float prevValue = 0.0f;

    protected void Awake()
    {
        AudioManager = AudioManager.instance;
        currvalue.value = AudioManager.GetVolume(volType);
        AudioManager.instance.GetVolController().SetVolume(volType, currvalue.value);
        SetText(((int)(currvalue.value * 100)).ToString());
        prevValue = currvalue.value;
    }

    private void OnEnable()
    {
        currvalue.value = AudioManager.instance.GetVolume(volType);
        AudioManager.instance.GetVolController().SetVolume(volType, currvalue.value);
        SetText(((int)(currvalue.value * 100)).ToString());
        prevValue = currvalue.value;
    }

    public void FixedUpdate()
    {
        if (prevValue != currvalue.value)
        {
            AudioManager.instance.GetVolController().SetVolume(volType, currvalue.value);
            SetText(((int)(currvalue.value * 100)).ToString());
        }
        prevValue = currvalue.value;
    }

    void SetText(string newText)
    {
        if (mText)
            mText.text = newText;
    }
}
