using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moraleBar : MonoBehaviour {


    public GameObject slider, Sad, Happy;
    public Transform StartPos, EndPos;
    public BattleSlider bSlider;
    public float value = .75f;
    float prevValue = 0.0f;
    // Use this for initialization
    float timer = 0;

    bool gameisPlaying = true;
    float startValue = 0.0f;

    private void Start()
    {
        prevValue = value;
        startValue = value;
    }

    private void FixedUpdate()
    {
        if (gameisPlaying)
        {
            ConstantDecay();
            UpdateBar();
            if (bSlider)
            {
                bSlider.SetSliderPos(value);
            }
            if (value >= .75f && prevValue < .75f)
            {
                if (DialogueManager.dialogueInstance)
                    DialogueManager.dialogueInstance.Moraleat75();
                prevValue = value;
            }
            else if (value <= .25f && prevValue > .25f)
            {
                if (DialogueManager.dialogueInstance)
                    DialogueManager.dialogueInstance.Moraleat25();
                prevValue = value;
            }
            else if ((value <= .5f && prevValue > .5f) || (value >= .5f && prevValue < .5f))
            {
                if (DialogueManager.dialogueInstance)
                    DialogueManager.dialogueInstance.Moraleat50();
                prevValue = value;
            }
        }
    }

    void ConstantDecay()
    {
        //1.6 morale repeating per minute to reach 0 from 100 in an hour
        //1.2 at 75%
        timer += Time.deltaTime;
        if(timer >= 60)
        {
            timer = 0;
            SubtractMorale(.01f);
        }
    }


    public void AddMorale(float increment)
    {
        //Benjamin Ousley
        //Check for prevValue for use with dialogue
        if (!gameisPlaying)
            return;
        prevValue = value;
        value += increment;
        UpdateBar();
    }

    public void SubtractMorale(float decrement)
    {
        //Benjamin Ousley
        //Check for prevValue for use with dialogue
        if (!gameisPlaying)
            return;
        prevValue = value;
        value -= decrement;
       // UpdateBar();
    }




    private void UpdateBar()
    {
        if (value < 0)
            value = 0;
        if (value > 1)
            value = 1;

        slider.transform.position = 
            Vector3.Lerp(StartPos.position, EndPos.position, value);
        Sad.transform.localScale = new Vector3(Sad.transform.localScale.x, Sad.transform.localScale.y, value);
        Happy.transform.localScale = new Vector3(Happy.transform.localScale.x, Happy.transform.localScale.y, 1 - value);

        Sad.transform.position = Vector3.Lerp(StartPos.position, slider.transform.position, .5f);
        Happy.transform.position = Vector3.Lerp(EndPos.position, slider.transform.position, .5f);

    }

    public float GetValue()
    {
        return value;
    }

    public bool GetPlaying()
    {
        return gameisPlaying;
    }

    public void ResetandPlay()
    {
        value = startValue;
        prevValue = value;
        UpdateBar();
        gameisPlaying = true;
    }

    public void SetPlaying(bool newPlay)
    {
        gameisPlaying = newPlay;
    }
}
