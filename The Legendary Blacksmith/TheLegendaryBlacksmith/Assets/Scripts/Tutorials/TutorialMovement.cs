using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialMovement : MonoBehaviour
{
    [SerializeField]
    TutorialManager manager;
    public bool on = true;
    [SerializeField]
    bool slerp = false;
    [SerializeField]
    bool lerp = false;
    [SerializeField]
    GameObject startPos;
    [SerializeField]
    GameObject endPos;
    [SerializeField]
    float xHeight = 0;
    [SerializeField]
    float yHeight = 0.3f;
    [SerializeField]
    float zHeight = 0;
    [SerializeField]
    float speed = 1;
    [SerializeField]
    float maxTime = 1;
    [SerializeField]
    float timer = 0;
    [SerializeField]
    bool delay = false;
    [SerializeField]
    float delayTime = 0;
    [SerializeField]
    bool pause = false;
    [SerializeField]
    float pauseTime = 0;
    [SerializeField]
    bool rotateMe = false;
    [SerializeField]
    GameObject startRotate;
    [SerializeField]
    GameObject endRotate;

    public UnityEvent EndAnim;

    float numberValue = 0;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (on && manager.glasses)
        {
            timer += Time.deltaTime * speed;
            if (delay)
            {
                if (timer >= delayTime)
                {
                    timer = 0;
                    delay = false;
                }
                return;
            }
            numberValue = timer/maxTime;
            if (timer > maxTime)
            {
                if (pause)
                {
                    numberValue = 1;
                    if (slerp)
                    {
                        SlerpItUp();
                    }
                    if (lerp)
                    {
                        LerpItDown();
                    }
                    if (timer > maxTime + pauseTime)
                    {
                        ResetMe();
                    }
                }
                else
                {
                    ResetMe();
                }
            }
            else
            {
                if (slerp)
                {
                    SlerpItUp();
                }
                if (lerp)
                {
                    LerpItDown();
                }
                if (rotateMe && startRotate != null && endRotate != null)
                {
                    SpinMeRightRoundBaby();
                }
            }
        }
	}

    public void SlerpItUp()
    {
        Vector3 point = (startPos.transform.position + endPos.transform.position) * 0.5f;
        point -= new Vector3(xHeight, yHeight, zHeight);
        Vector3 Rise = startPos.transform.position - point;
        Vector3 Lower = endPos.transform.position - point;
        transform.position = Vector3.Slerp(Rise, Lower, numberValue);
        transform.position += point;
    }

    public void LerpItDown()
    {
        transform.position = Vector3.Lerp(startPos.transform.position, endPos.transform.position, numberValue);
    }

    public void SpinMeRightRoundBaby()
    {
        transform.rotation = Quaternion.Lerp(startRotate.transform.rotation, endRotate.transform.rotation, numberValue);
    }

    public void ResetMe()
    {
        EndAnim.Invoke();
        timer = 0;
        numberValue = 0;
    }

    public void SetStartPosition(GameObject pos)
    {
        startPos = pos;
    }

    public void TurnMeOn()
    {
        on = true;
    }
    public void TurnMeOff()
    {
        on = false;
    }
}
