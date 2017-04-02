using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JukeBoxManagement : MonoBehaviour
{
    [SerializeField]
    Image[] bars;

    [SerializeField]
    JukeBoxDial Dial;

    [SerializeField]
    TextMesh mText;

    [SerializeField]
    Color unactive;

    [SerializeField]
    Color Active;

    [SerializeField]
    VolumeControl.VolumeControls currType;

    [SerializeField]
    int activeBar = 0;

    int numberBars = 6;


    // Use this for initialization
    void Start()
    {
        numberBars = bars.Length;

        SetupBars();

        float currVol = AudioManager.instance.GetVolume(currType);
        Dial.SetUpDial(currVol, 1.0f / (numberBars - 1.0f));

    }

    void SetupBars()
    {
        float currVol = AudioManager.instance.GetVolume(currType);
        if (currVol == 0)
            activeBar = 0;
        else if (currVol == .25f)
            activeBar = 1;
        else if (currVol == .5f)
            activeBar = 2;
        else if (currVol == .75f)
            activeBar = 3;
        else if (currVol == 1.0f)
            activeBar = 4;

        for (int i = 0; i < numberBars; ++i)
        {
            if (i <= activeBar)
                bars[i].color = Active;
            else
                bars[i].color = unactive;
        }


    }

    public void LowerVolume()
    {
        activeBar -= 1;
        if (activeBar < 0)
            activeBar = 0;
        float currVol = AudioManager.instance.GetVolume(currType);
        currVol -= .25f;
        currVol = Mathf.Clamp01(currVol);
        AudioManager.instance.GetVolController().SetVolume(currType, currVol);
        SetupBars();
    }

    public void RaiseVolume()
    {
        activeBar += 1;
        if (activeBar >= numberBars)
            activeBar = numberBars - 1;
        float currVol = AudioManager.instance.GetVolume(currType);
        currVol += .25f;
        currVol = Mathf.Clamp01(currVol);
        AudioManager.instance.GetVolController().SetVolume(currType, currVol);
        SetupBars();
    }

    public void ChangeType(VolumeControl.VolumeControls newType)
    {
        currType = newType;
        mText.text = currType.ToString();
        SetupBars();
        float currVol = AudioManager.instance.GetVolume(currType);
        Dial.SetUpDial(currVol, 1.0f / (numberBars - 1.0f));
    }
}
