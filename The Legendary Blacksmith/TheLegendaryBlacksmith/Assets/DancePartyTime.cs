using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancePartyTime : MonoBehaviour
{
    [SerializeField]
    GameObject[] partygoers;

    bool firstTime = true;

    float gimmeASec = 0.0f;

    [SerializeField]
    int type = 4;

    // Use this for initialization
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (firstTime)
        {
            gimmeASec += Time.deltaTime;
            if (gimmeASec >= 2)
            {
                gimmeASec = 0;
                switch (type)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        foreach (GameObject partyperson in partygoers)
                        {
                            Animator temp = partyperson.GetComponent<Animator>();
                            Idle(temp);
                        }
                        break;
                    case 5:
                        break;
                    case 6:
                        {
                            //horse Neigh
                            break;
                        }
                    case 7:
                        {
                            //salute
                            foreach (GameObject partyperson in partygoers)
                            {
                                Animator temp = partyperson.GetComponent<Animator>();
                                Dance(temp);
                            }
                            break;
                        }
                }
                type = 7;
            }
        }
    }

    bool IsPlaying(Animator myAnimator, int layer)
    {
        if (myAnimator)
        {
            return myAnimator.GetCurrentAnimatorStateInfo(layer).length >
                   myAnimator.GetCurrentAnimatorStateInfo(layer).normalizedTime &&
                   !myAnimator.IsInTransition(layer);
        }
        return true;

    }

    void SetSpeed(Animator myAnimator, float speed)
    {
        myAnimator.SetFloat("Speed_f", speed);
    }

    public void Idle(Animator myAnimator)
    {
        if (!IsPlaying(myAnimator, 0))
        {
            SetSpeed(myAnimator, .2f);
            //myAnimator.StopPlayback();
            myAnimator.Play("Idle", 0);
        }
        if (!IsPlaying(myAnimator,3))
        {
            myAnimator.SetInteger("Animation_int", 0);
        }
    }

    void Dance(Animator myAnimator)
    {
        if (!IsPlaying(myAnimator, 3))
        {
            myAnimator.SetInteger("Animation_int", 4);
            firstTime = false;
        }
    }
}