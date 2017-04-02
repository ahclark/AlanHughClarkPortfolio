using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TargetFinder : MonoBehaviour {
    public UnitAnimationController animCont;

    UnitContainer enemyunits = null;
    public GameObject moveToObj = null;
    public int side = 0;
    public int state = -1;
    enum states { Moving = 0, Attacking, Dying}
    float attacktimer = 0;
    public float attackspeed = 1;
    NavMeshAgent agent;
    public int type = -1;
    public float attackRange = 2;
    UnitHealth enemyhealth = null;
	// Use this for initialization
	void Start () {
        GameObject temp = null;
        if(side == 1)
        {
            temp = GameObject.Find("BlueUnitContainer");
        }
        else
        {
            temp = GameObject.Find("RedUnitContainer");
        }
        if(temp)
        {
            enemyunits = temp.GetComponent<UnitContainer>();
            if(!enemyunits)
            {
                gameObject.SetActive(false);
            }
        }
        agent = GetComponent<NavMeshAgent>();

    }

    private void FixedUpdate()
    {
        if(moveToObj)
        {
            transform.LookAt(moveToObj.transform);

        }
        switch (state)
        {
            case 0:
                {
                    if(moveToObj)
                    {
                        if(agent)
                        agent.destination = moveToObj.transform.position;

                        animCont.Walk();
                        if((moveToObj.transform.position - transform.position).magnitude <= attackRange)
                        {
                            state = 1;
                            agent.destination = transform.position;
                        }
                    }
                    else
                    {
                        state = -1;
                    }
                    break;
                }
            case 1:
                {
                    if(!moveToObj)
                    {
                        state = -1;
                    }
                    attacktimer += Time.deltaTime;
                    if(attacktimer >= attackspeed)
                    {
                        bool animated = false;
                        if(animCont)
                        {
                            if(type == 0)
                            {
                                animated = animCont.Stab();
                            }
                            else if(type == 1)
                            {
                                animated = animCont.TwoHanded();
                            }
                            else
                            {
                                animated = animCont.Shoot();
                            }
                        }
                        //attack
                        if (animated)
                        {
                            attacktimer = 0;

                            if (enemyhealth)
                            {
                                //damage enemy health;
                               if( !enemyhealth.TakeDamage(1))
                                {
                                    resetEnemy();
                                }
                            }
                        }
                    }
                    break;
                }
            case 2:
                {
                    //let animation go through, check when it is done.  for now just turn off.
                    gameObject.SetActive(false);
                    break;
                }
            default:
                {
                    //find target
                    MoveToNearestEnemy();
                    break;
                }
        }
    }

    private void MoveToNearestEnemy()
    {
        GameObject closestObj = null;
        float closestZ = 99999f;
        for (int i = 0; i < enemyunits.units.Count; i++)
        {
            //float tempZ = Mathf.Abs(enemyunits.units[i].transform.position.z) - Mathf.Abs(transform.position.z);
            //if(Mathf.Abs(tempZ) < Mathf.Abs(closestZ))
            //{
            //    closestZ = tempZ;
            //    closestObj = enemyunits.units[i];
            //}
           Vector3 tempZ = (enemyunits.units[i].transform.position) - (transform.position);
           if(tempZ.magnitude < (closestZ))
           {
               closestZ = tempZ.magnitude;
               closestObj = enemyunits.units[i];
           }
        }
        moveToObj = closestObj;
        if(moveToObj)  //NOTE NOTE NOTE NOTE NOTE NOTE NOTE       IF THIS IS NULL, JUST WALK TOWARDS THE ENEMY BASE.  NO ENEMIES WERE FOUND ON THE FIELD.
        enemyhealth = moveToObj.GetComponent<UnitHealth>();
        //+= our resetEnemy() to their death event

        state = 0;
    }



    void resetEnemy()
    {

        moveToObj = null;
        state = -1;
        enemyunits.units.Remove(enemyhealth.gameObject);
        Destroy(enemyhealth.gameObject);
        enemyhealth = null;
    }
}
