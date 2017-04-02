//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MinionController : MonoBehaviour {


//     public float health = 80;
//     public float damage = 3;

//    Vector3 Velocity;
//    public Transform MoveTarget;
//    public int currWayIndex = 0;
//    public int index;
//    public LaneManager laneManager;
//    public bool Player = false;
//    bool attacking = false;
//    public int state = 0;
//    public float AttackRange = 1;
//    enum AI_Sates { Moving = 0, Attacking = 1, Capturing = 2}
//    MinionController attackTarget = null;
//    BaseController baseTarget = null;
//    OutpostCapture captureTarget = null;
//    public float attackSpeed = 2;
//    float attackTimer;

//    public float CheckTime = .75f;
//    public float updateTimer = 0.0f;
//    public bool dead = false;

//	// Use this for initialization
//	void Start () {
//        attackTimer = attackSpeed;
//        if(!Player)
//        {
//            currWayIndex = 8;
//            Move();
//        }
//	}
	
//	// Update is called once per frame
//	void Update () {
		
//	}

//    private void FixedUpdate()
//    {
//        //update

        
//        updateTimer += Time.deltaTime;
       
//        if (updateTimer >= CheckTime)
//        {
//            health -= Time.deltaTime * 10;
//            if (health <= 0)
//                Death();
//            updateTimer = 0;
//            CheckMe();
//        }
//        if(MoveTarget != null)
//        Velocity = (MoveTarget.position - transform.position);

//        switch (state)
//        {
//            case (int)AI_Sates.Moving:
//                {
//                    Move();
//                    break;
//                }
//            case (int)AI_Sates.Attacking:
//                {
//                    //attack
//                    attackTimer += Time.deltaTime;
//                    if (attackTimer >= attackSpeed)
//                    {
//                        attackTimer = 0;
//                        Attack();
//                    }
//                    break;
//                }
//            case (int)AI_Sates.Capturing:
//                {
//                    Move();
//                    break;
//                }
//        }


//    }

//    public void CheckMe()
//    {
//        //am i attacking already?
//        if(!attacking)
//        {
//            //is there a nearby enemy?
//            RadarBleep();

//        }
//        return;
//    }

//    public void RadarBleep()
//    {
//        //find closest target
//        MinionController closestCont = laneManager.GetClosestEnemy(Player, transform);
//        if(closestCont != null)
//        {
//            MoveTarget = closestCont.transform;
//            attacking = true;
//            attackTarget = closestCont;
//        }
//        else if(captureTarget == null)
//        {
//            //look for a base to capture
//            captureTarget = laneManager.GetClosestOutpost(Player, laneManager.index, transform);
//            if (captureTarget != null)
//            {
//                MoveTarget = captureTarget.transform;
//            }
//            else
//                print("null outpost check");
           
//        }
//        else
//        {
//            if((captureTarget.transform.position - transform.position).magnitude <= .05 && (state != (int)AI_Sates.Capturing))
//            {
//                state = (int)AI_Sates.Capturing;
//                if(!captureTarget.CheckIn(Player, this))
//                {
//                    captureTarget = null;
//                    CheckMe();
//                }
//            }
//        }

//    }

//    private void Death()
//    {
//        gameObject.SetActive(false);
//    }

//    public void TakeDamage(float damage)
//    {
//        health -= damage;
//        if(health <= 0)
//        {
//            //die
//            laneManager.RemoveUnit(index);
//            if(captureTarget != null)
//            {
//                captureTarget.CheckOut(this);
//            }
//            Death();
//        }
//    }
//    public void Attack()
//    {
        
//        if(attackTarget != null)
//        {
//            attackTarget.TakeDamage(damage);
//        }
//        else if(baseTarget != null)
//        {
//            baseTarget.AttackMe(damage, index);
//        }
//        else if(attacking)
//        {
//            //im attacking and my targets are null
//            print("bad attack");
//            attacking = false;
//            state = (int)AI_Sates.Moving;
//        }
//    }

//    public void Move()
//    {
//        //do we have a target?
//        if(MoveTarget == null)
//        {
//            //wait because we don't have a target
//            print("error, no move target");
//            return;
//        }
//        //are we moving to a waypoint?
        
//        if(attacking)
//        {
//            if (Velocity.magnitude <= AttackRange)
//            {
//                state = (int)AI_Sates.Attacking;
//            }
//        }
//        //move
//        transform.position = transform.position + Velocity.normalized * Time.deltaTime * 2;
//    }

//    public void SetMoveTarget(Transform target)
//    {
//        MoveTarget = target;
//    }

//    public void CaptureComplete()
//    {
//        state = (int)AI_Sates.Moving;
//        captureTarget.CheckOut(this);
//        captureTarget = null;
//        RadarBleep();
//    }

//    private void OnDestroy()
//    {
//        print("destroyed");
//    }
//}



