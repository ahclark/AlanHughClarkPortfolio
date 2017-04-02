using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationController : MonoBehaviour {

    Animator myAnimator;

    public int type = 0;
    public int ReactionType = -1;

    float gimmeASec = 0;

    public void SetType(int _type)
    {
        type = _type;
    }



    // Use this for initialization
    void Start() {
        myAnimator = GetComponent<Animator>();
        if (!myAnimator)
        {
            gameObject.SetActive(false);
        }



    }

    private void FixedUpdate()
    {
        gimmeASec += Time.deltaTime;
        if (gimmeASec >= 2)
        {
            gimmeASec = 0;
            switch (type)
            {
                case 0:
                    Stab();
                    break;
                case 1:
                    TwoHanded();
                    break;
                case 2:
                    Shoot();
                    break;
                case 3:
                    HorseIdle();
                    break;
                case 4:
                    Idle();
                    break;
                case 5:
                    Walk();
                    break;
                case 6:
                    {
                        //horse Neigh
                        HorseNeigh();
                        break;
                    }
                case 7:
                    {
                        //salute
                        Dance();
                        break;
                    }
                case 8:
                    {
                        WinningDance();
                        break;
                    }

            }
        }
    }

    void CheckWatch()
    {
        if(!IsPlaying(3))
        {
            myAnimator.SetInteger("Animation_int", 3);
            SetType(4);
            
        }
    }
   
    void Salute()
    {
        if(!IsPlaying(3))
        {
            myAnimator.SetInteger("Animation_int", 6);
            SetType(4);
        }
    }

    void Dance()
    {
        if(!IsPlaying(3))
        {
            myAnimator.SetInteger("Animation_int", 4);
            SetType(4);
        }
    }

    void HorseIdle()
    {
        if (!IsPlaying(0))
        {
            SetSpeed(.09f);
            myAnimator.SetBool("Eat_b", true);
        }
    }

    void HorseNeigh()
    {
        if (!IsPlaying(0))
        {
            myAnimator.SetBool("Eat_b", false);
            SetType(3);
        }
    }

    void SetSpeed(float speed)
    {
        // myAnimator.speed = speed;
        //figure out how to set Speed_f
        myAnimator.SetFloat("Speed_f", speed);
    }

    public bool Shoot()
    {
        if(!IsPlaying(0))
        {
            SetSpeed(.2f);
           myAnimator.Play("BowShoot", 0);
            myAnimator.Play("Bow_Shoot", 6);
            return true;
        }
        return false;

    }


    public void Idle()
    {
        if (!IsPlaying(0))
        {
            SetSpeed(.2f);
            //myAnimator.StopPlayback();
            myAnimator.Play("Idle", 0);
        }
        if(!IsPlaying(3))
        {
            myAnimator.SetInteger("Animation_int", 0);
            ReactionType = -1;

        }
    }

    public bool TwoHanded()
    {
        if (!IsPlaying(6))
        {
            SetSpeed(.2f);
            myAnimator.Play("Melee_TwoHanded", 0);
            myAnimator.Play("Melee_TwoHanded", 6);

            return true;
        }
        return false;

    }

    public bool Stab()
    {
        if (!IsPlaying(6))
        {
            SetSpeed(.2f);
            myAnimator.Play("Melee_Stab", 0);
            myAnimator.Play("Melee_Stab", 6);

            return true;
        }
            return false;
    }

    public void Walk()
    {
        SetSpeed(.3f);

        if (!IsPlaying(0))
        {
            myAnimator.Play("Walk_Static", 0);
        }
    }

    public void Reload()
    {
        if(!IsPlaying(6))
        {
            SetSpeed(.2f);
            myAnimator.Play("Bow_Load", 6);
        }
    }


    bool IsPlaying(int layer)
    {
        if (myAnimator)
        {
            return myAnimator.GetCurrentAnimatorStateInfo(layer).length >
                   myAnimator.GetCurrentAnimatorStateInfo(layer).normalizedTime &&
                   !myAnimator.IsInTransition(layer);
        }
        return true;
        
    }

    public void WinningDance()
    {
        myAnimator.SetInteger("Animation_int", 10);
    }







}
