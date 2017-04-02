using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {
    float timer = 0;

    public float AlignmentStrength = 1;
    public float CohesionStrength = 1;
    public float SeparationStrength = 1;
    public Vector3 AveragePosition;
    protected Vector3 AverageForward;
    public float FlockRadius;
    public Vector3 targetPos = new Vector3(0, 0, 0);
    public bool goingToTeleport = false;
    public List<Boid> boids = new List<Boid>();
    public Vector3 boidVel;
    public Transform teleportTrans = null;
    public FlockManager FM;
    public int side = 0;
    public int currAI_State = 0;
    float attackTimer = 0;
    public float attackspeed;
    Flock enemyFlock = null;
    bool attackingBase = false;
    float selfDestructTimer = 0;
    public OutpostCapture currOutpost = null;
    public DeathEffectScript deathEffect = null;
    public bool sleeping = false;
    public enum AI_States
    {
        Moving = 0, Attacking, Capturing
    }
    private void Start()
    {
        FM = GameObject.Find("GameField").GetComponent<FlockManager>();
        AveragePosition = transform.position;
    }
    private void OnEnable()
    {
        FlockManager.UpdateEvent += FlockUpdate;
    }
    private void OnDisable()
    {
        FlockManager.UpdateEvent -= FlockUpdate;
    }
    
    void Sleep()
    {
        if (targetPos == Vector3.zero)
        {
            FM.FindSleepingOutpost(this);

        }
        else if((targetPos - transform.position ).magnitude <= 1)
        {
            return;
        }
        else
        {
            AlignmentStrength = 1;
            CohesionStrength = 0;
            SeparationStrength = 0;
            GetAveragePositions();
            transform.position = AveragePosition;
            UpdateBoidVelocity(Time.deltaTime);
            if((targetPos - transform.position).magnitude <= 1)
            {
                for (int i = 0; i < boids.Count; i++)
                {
                    boids[i].Velocity = Vector3.zero;
                }
            }
        }
    }

    void FlockUpdate(float dt)
    {
        timer += dt;
        if(sleeping)
        {
            Sleep();
            return;
        }
        if(timer >= 10)
        {
            timer = 0;
            targetPos = Vector3.zero;
        }
        GetAveragePositions();
        transform.position = AveragePosition;
        if(attackingBase)
        {
            selfDestructTimer += dt;
            if(selfDestructTimer >= 5)
            {
                selfDestructTimer = 0;

                SelfDestruct();
            }
        }
        if (goingToTeleport)
        {
           
                Teleport();


                return;
         
        }

        switch(currAI_State)
        {
            case (int)AI_States.Moving:
                {


                    if (FM.CheckForCombat(this))
                    {
                        currAI_State = (int)AI_States.Attacking;
                        break;
                    }
                    if ((targetPos != Vector3.zero))
                    {
                        if ((targetPos - transform.position).magnitude <= 2 && !attackingBase)
                        {
                            currAI_State = (int)AI_States.Capturing;
                            FM.FindNearestOutposts(this);

                        }
                        else
                        {
                            AlignmentStrength = 1;
                            CohesionStrength = 1;
                            SeparationStrength = 1;
                            UpdateBoidVelocity(dt);
                        }
                    }
                    else
                    {
                        //find Target
                        FM.FindNearestOutposts(this);
                    }
                    break;
                }
            case (int)AI_States.Attacking:
                {


                    if (targetPos == Vector3.zero || enemyFlock == null)
                    {
                        if(!FM.CheckForCombat(this))
                        {
                            currAI_State = (int)AI_States.Moving;
                        }
                    }
                    else if((enemyFlock.transform.position - transform.position).magnitude <= 5)
                    {
                        attackTimer += Time.deltaTime;

                        AlignmentStrength = 1;
                        CohesionStrength = 0;
                        SeparationStrength = 0;
                        UpdateBoidVelocity(dt);

                        if (attackTimer >= attackspeed)
                        {
                            attackTimer = 0;
                            if (!enemyFlock.AttackFlock())
                            {
                                currAI_State = (int)AI_States.Moving;
                                targetPos = Vector3.zero;
                                enemyFlock = null;
                            }
                        }
                    }
                    else
                    {
                        AlignmentStrength = 1;
                        CohesionStrength = 1;
                        SeparationStrength = 0;
                        UpdateBoidVelocity(dt);
                    }
                    break;
                }
            case (int)AI_States.Capturing:
                {
                    if (FM.CheckForCombat(this))
                    {
                        currAI_State = (int)AI_States.Attacking;
                        currOutpost = null;

                        break;
                    }

                    if (targetPos == Vector3.zero)
                    {
                        currAI_State = (int)AI_States.Moving;
                        currOutpost = null;
                    }
                    else
                    {
                        //move to base
                        AlignmentStrength = 1;
                        CohesionStrength = 1;
                        SeparationStrength = 0;
                        UpdateBoidVelocity(dt);
                        if (currOutpost != null)
                            currOutpost.Capture(side);
                        else
                            FM.FindNearestOutposts(this);
                    }
                    break;
                }
        }
    }

    private void SetMaxSpeed(float speed)
    {
        for (int i = 0; i < boids.Count; i++)
        {
            boids[i].MaxSpeed = speed;
        }
    }

    private void Teleport()
    {

       

        if (teleportTrans == null)
        {
            return;
        }
        attackingBase = true;

        goingToTeleport = false;
        for (int i = 0; i < boids.Count; i++)
        {
            boids[i].assignedY = teleportTrans.transform.position.y;
            boids[i].transform.position = teleportTrans.position;

        }
        targetPos = teleportTrans.position + new Vector3(0, 0, 14 * -side);

        currAI_State = (int)AI_States.Moving;
    }

    private void SelfDestruct()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            boids[i].gameObject.SetActive(false);
            boids.RemoveAt(i);
            i--;
            if (side == 1)
                FM.DamageBlueBase();
            else
                FM.DamageRedBase();
        }
        FM.RemoveFlock(this);
        attackingBase = false;
        Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    private void UpdateBoidVelocity(float dt)
    {
        for (int i = 0; i < boids.Count; ++i)
        {
            boidVel = boids[i].Velocity;
            Vector3 acc = new Vector3(0, 0, 0);
            acc += CalculateAlignmentAcceleration(boids[i]);
            acc += CalculateCohesionAcceleration(boids[i]);
            acc += CalculateSeparationAcceleration(boids[i]);
            acc = acc * (boids[i].MaxSpeed * dt);
            boidVel += acc;
            if (boidVel.magnitude > boids[i].MaxSpeed)
            {

                boidVel.Normalize();
                boidVel *= boids[i].MaxSpeed;
            }
            boids[i].Velocity = boidVel;
            boids[i].BoidUpdate(dt);
        }
    } 

    void GetAveragePositions()
    {
        AveragePosition = new Vector3(0, 0, 0);
        for (int i = 0; i < boids.Count; i++)
        {
            if(boids[i] != null)
                AveragePosition += boids[i].transform.position;
        }
        if(boids.Count != 0)
        AveragePosition = AveragePosition / boids.Count;
    }

    public void AddBoid(GameObject boid)
    {
        if (boid != null )
        {
            Boid temp = boid.GetComponent<Boid>();
            if(!boids.Contains(temp))
                boids.Add(temp);
        }
    }

    private Vector3 CalculateAlignmentAcceleration(Boid boid)
    {
        Vector3 temp = (targetPos - transform.position) * (1 / boid.MaxSpeed);
        if (temp.magnitude > 1)
        {
            temp.Normalize();
            temp *= AlignmentStrength;
            return temp;
        }
        else
        {
            temp *= AlignmentStrength;
            return temp;
        }

    }

    private Vector3 CalculateCohesionAcceleration(Boid boid)
    {
        Vector3 influence = AveragePosition - boid.transform.position;
        float length = influence.magnitude;
        influence.Normalize();

        if (length < FlockRadius)
        {
            influence.x = influence.x * (influence.x / FlockRadius);
            influence.y = influence.y * (influence.y / FlockRadius);
            influence.z = influence.z * (influence.z / FlockRadius);
        }

        influence *= CohesionStrength;
        return influence;
    }

    private Vector3 CalculateSeparationAcceleration(Boid boid)
    {
        Vector3 sum = new Vector3(0, 0, 0);

        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 vecdist = boid.transform.position - boids[i].transform.position;
            float dist = vecdist.magnitude;
            float safeDist = boid.SafeRadius + boids[i].SafeRadius;
            if (dist < safeDist)
            {
                vecdist.Normalize();
                vecdist *= ((safeDist - dist) / safeDist);
                sum += vecdist;
            }
        }
        if (sum.magnitude > 1)
            sum.Normalize();


        return sum * SeparationStrength;
    }

    public void SetEnemyFlock(Flock _flock)
    {
        enemyFlock = _flock;
    }

    public bool AttackFlock()
    {
        boids[0].gameObject.SetActive(false);
        boids.RemoveAt(0);
        if(boids.Count <= 0)
        {
            FM.RemoveFlock(this);
            if (currOutpost != null)
                currOutpost.captureFlocks.Clear();
            currOutpost = null;
            Destroy(this.gameObject);
            return false;
        }
        return true;
    }

}
