using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour {
    public delegate void EventUpdate();
    public event EventUpdate Death;

    [Header("Public Variables")]
    //public variables
    public int side = 0; //1 for red, -1 for blue
    public int lane = 0; // 0: left lane, 1: mid lane, 2: right lane
    public int unitType = -1; //0 heave, 1 light, 2 ranged
    public float health = 0; //dies when this reaches 0
    public float attackSpeed = 0; //how often do they attack?
    public float attackRange = 0; //what is their range to start attacking?
    public float maxSpeed = 0; //fastest a unit can travel
    public Vector3 targetPosition; //for storing the position to move to
    public float damage = 1;


    [Header("Serialized Variables")]
    //serialized variables
    [SerializeField]
    public GameStateController stateController;
    [SerializeField]
    public BlacksmithBaseController enemyBase;
    [SerializeField]
    UnitAnimationController myAnimator;
    public LaneController myLane;
    public GameObject DebugLine;
    public LineRenderer debugLineScript;
    public HealthBarTick[] healthbar = new HealthBarTick[10];
    public CombatArrowController arrow = null;
    
    //private variables
    private List<int> myActions = new List<int>();
    private float moveDistanceThreshold = 1.0f;
    private enum ActionDefines { MoveTo = 0, Capture, Attack, Sleep, ReEvaluate, Teleport, AttackBase};
    private AgentController enemyAgent = null;
    private OutpostController toCaptureOutpost = null;
    private float timer = 0;
    private bool teleported = false;
    private float reEvaluateTimer = 0;

    //Actions

    //0
    void MoveTo()
    {
        if (myAnimator)
        {
            myAnimator.Walk();
        }
        Vector3 distance = targetPosition - transform.position;
        debugLineScript.SetPosition(0, transform.position);
        debugLineScript.SetPosition(1, targetPosition);

        if (enemyAgent == null)
        {
            if (distance.magnitude > moveDistanceThreshold)
            {
                transform.position = transform.position + (distance.normalized * Time.deltaTime);
                myActions.Add((int)ActionDefines.MoveTo);
            }
            else
            {
                ResetActions();
            }
        }
        else
        {
            if (distance.magnitude > attackRange)
            {
                transform.position = transform.position + (distance.normalized * Time.deltaTime);
                myActions.Add((int)ActionDefines.MoveTo);
            }
            else
            {
                ResetActions();
            }
        }
    }
    //1
    void Capture()
    {
        if(myAnimator)
        {
            myAnimator.Idle();
        }
        if (toCaptureOutpost != null)
        {
            if(toCaptureOutpost.CaptureMe(.1f, side))
            {
                if(myAnimator)
                {
                    myAnimator.Walk();
                }
                ResetActions();
            }
            else
            {
                myActions.Add((int)ActionDefines.Capture);
            }
        }
        else
        {
            ResetActions();
        }
    }
    //2
    void Attack()
    {
        bool attack = false;
        if (myAnimator)
        {
            if (unitType == 0)
            {
                attack = myAnimator.Stab();
            }
            else if (unitType == 1)
            {
                attack = myAnimator.TwoHanded();
            }
            else
            {
                attack = myAnimator.Shoot();
            }
        }
        timer += Time.deltaTime;
        if (timer >= attackSpeed && attack)
        {
            timer = 0;
            if (enemyAgent != null)
            {
                if (arrow)
                {
                    arrow.enemy = enemyAgent;
                    arrow.enemyPos = enemyAgent.transform.position + Vector3.up * .25f;
                    arrow.gameObject.SetActive(true);
                    if(myAnimator)
                    {
                        myAnimator.Reload();
                    }
                }
                else
                {

                    enemyAgent.AttackMe(1.0f, unitType);
                }
            }
            else
            {
                ResetActions();
            }
        }
    }
    //3
    void Sleep()
    {
        if (myAnimator)
        {
            myAnimator.Idle();
        }
        //animate sleep stuff
        if (stateController.nightTime)
            myActions.Add((int)ActionDefines.Sleep);
        else
            ResetActions();
    }
    //4
    void ReEvaluate()
    {
        if(enemyAgent != null)
        {
            if(enemyAgent.health <= 0)
            {
                enemyAgent = null;
                ResetActions();
                return;
            }
            UpdateEnemyAction();
            return;
        }
        //evaluate where to go





        if(CheckGameState())
        {
            return;
        }





    }
    //5
    void Teleport()
    {
        //teleport to base
        teleported = true;
        gameObject.transform.position = enemyBase.transform.position;
        ResetActions();
    }
    //6
    void AttackBase()
    {
        //attack the base
        enemyBase.AttackMe(1.0f);
        ResetActions();
        gameObject.SetActive(false);
    }


    //pathing test functions
    bool CheckGameState()
    {
        //what should i be doing?

        //attacking is dealt with
        if (stateController.nightTime)
        {
            
            if (myLane)
            {
                Vector3 closest = new Vector3(0, 100, 0);
                Vector3 dist = new Vector3();
                int closestIndex = -1;
                for (int i = 0; i < myLane.outposts.Length; i++)
                {
                    dist = (myLane.outposts[i].transform.position - transform.position);
                    if(dist.magnitude < closest.magnitude && myLane.outposts[i].side == side)
                    {
                        closestIndex = i;
                        closest = dist;
                    }
                }
                if(closestIndex == -1)
                {
                    myActions.Add((int)ActionDefines.Sleep);
                }
                else
                {
                    targetPosition = myLane.outposts[closestIndex].transform.position;
                    if((targetPosition - transform.position).magnitude <= moveDistanceThreshold)
                    {
                        myActions.Add((int)ActionDefines.Sleep);
                    }
                    else
                    {
                        myActions.Add((int)ActionDefines.MoveTo);
                    }
                }
            }
            return true;
        }
        //1. Get to enemy base and capture outposts along the way
        if (side == 1)
        {
            if(teleported)
            {
                Vector3 dist = enemyBase.transform.position - transform.position;
                if ((dist.magnitude <= moveDistanceThreshold))
                {
                    myActions.Add((int)ActionDefines.AttackBase);
                    return true;
                }
            }
            if (myLane)
            {
                if (myLane.bluePortal)
                {
                    if (myLane.bluePortal.activeInHierarchy)
                    {
                        enemyBase.openToAttack = true;

                        Vector3 dist = myLane.bluePortal.transform.position - transform.position;
                        if ((dist.magnitude <= moveDistanceThreshold + 1.5f))
                        {
                            myActions.Add((int)ActionDefines.Teleport);
                            return true;
                        }

                    }
                }
            }
            if (toCaptureOutpost != null)
            {
                if(toCaptureOutpost.side == side)
                {
                    toCaptureOutpost = null;
                    ResetActions();
                    return false;
                }
                Vector3 dist = toCaptureOutpost.transform.position - transform.position;
                if(dist.magnitude <= 3)
                {
                    myActions.Add((int)ActionDefines.Capture);

                    return true;
                }
            }
                //find an outpost to try and capture
                GetPathToTarget();
                myActions.Add((int)ActionDefines.MoveTo);
            
        }
        if (side == -1)
        {
            if (teleported)
            {
                Vector3 dist = enemyBase.transform.position - transform.position;
                if ((dist.magnitude <= moveDistanceThreshold))
                {
                    myActions.Add((int)ActionDefines.AttackBase);
                    return true;
                }
            }
            if (myLane)
            {
                if (myLane.redPortal)
                {
                    if (myLane.redPortal.activeInHierarchy)
                    {
                        enemyBase.openToAttack = true;
                        Vector3 dist = myLane.redPortal.transform.position - transform.position;
                        if ((dist.magnitude <= moveDistanceThreshold + 1.5f))
                        {
                            myActions.Add((int)ActionDefines.Teleport);
                            return true;
                        }
                    }
                }
            }
            if (toCaptureOutpost != null)
            {
                if (toCaptureOutpost.side == side)
                {
                    toCaptureOutpost = null;
                    ResetActions();
                    return false;
                }
                Vector3 dist = toCaptureOutpost.transform.position - transform.position;
                if (dist.magnitude <= 3)
                {
                    myActions.Add((int)ActionDefines.Capture);
                    return true;
                }
            }
                GetPathToTarget();
                myActions.Add((int)ActionDefines.MoveTo);
        }
        return false;

    }

    void GetPathToTarget()
    {
        //how should i get where im going?
        //im not attacking, teleporting, capturing, or attacking the enemy base.  
        //I have checked all of those before this, and now i need to figure out where im going and if i need to cap an outpost.

        //find the next outpost in my lane
        if(!myLane)
        {
            return;
        }
        //where am i in relation to my stuff??
        float myZ = transform.position.z;
        float closest = float.MaxValue;
        if (side == -1)
            closest = -float.MaxValue;
        int closestIndex = -1;
        for (int i = 0; i < myLane.outposts.Length; i++)
        {
            if(side == 1)
            {
                if(myLane.outposts[i].transform.position.z > myZ + moveDistanceThreshold + .5f 
                    && myLane.outposts[i].transform.position.z < closest)
                {
                    closest = myLane.outposts[i].transform.position.z;
                    closestIndex = i;
                }
            }
            else
            {
                if (myLane.outposts[i].transform.position.z < myZ - moveDistanceThreshold - .5f
                     && myLane.outposts[i].transform.position.z > closest)
                {
                    closest = myLane.outposts[i].transform.position.z;
                    closestIndex = i;
                }
            }
        }
        if (closestIndex != -1)
        {
            targetPosition = myLane.outposts[closestIndex].transform.position;

            if (myLane.outposts[closestIndex].side != side)
            {
                toCaptureOutpost = myLane.outposts[closestIndex];
            }
        }


    }


    //Helper Functions

    void Act()
    {
        switch(myActions[0])
        {
            case 0:
                {
                    MoveTo();
                    break;
                }
            case 1:
                {
                    Capture();
                    break;
                }
            case 2:
                {
                    Attack();
                    break;
                }
            case 3:
                {
                    Sleep();
                    break;
                }
            case 4:
                {
                    ReEvaluate();
                    break;
                }
            case 5:
                {
                    Teleport();
                    break;
                }
            case 6:
                {
                    AttackBase();
                    break;
                }
        }
        myActions.RemoveAt(0);
        if(myActions.Count == 0)
        {
            ResetActions();
        }
        transform.LookAt(targetPosition);
    }

    void ResetActions()
    {
        myActions.Clear();
        myActions.Add((int)ActionDefines.ReEvaluate);
    }

    public void AttackMe(float damage, int type)
    {
        int weakness = -1;
        switch(unitType)
        {
            case 0:
                {
                    weakness = 1;
                    break;
                }
            case 1:
                {
                    weakness = 2;
                    break;
                }
            case 2:
                {
                    weakness = 0;
                    break;
                }
        }

        if(type == weakness)
        {
            damage += damage;
        }

       
            if (damage < 2)
            {
                if (healthbar[(int)health - 1])
                {
                    if ((int)health % 2 == 0)
                    {
                        healthbar[(int)health - 1].Tick(1);
                    }
                    else
                    {
                        healthbar[(int)health - 1].Tick(-1);

                    }

                }
            }
            else
            {
            int toggle = 1;
                int tempDamage = (int)damage;
                for (int i = 0; i < tempDamage; i++)
                {
                if(health < 0)
                {
                    continue;
                }

                if (healthbar[(int)health - 1 - i])
                {
                    if ((int)health % 2 == 0)
                    {
                        healthbar[(int)health - 1 - i].Tick(1 * toggle);
                    }
                    else
                    {
                        healthbar[(int)health - 1 - i].Tick(-1 * toggle);

                    }

                }

                toggle *= -1;
                }
            }
        
       
        health -= damage;
        
        if (health <= 0)
        {
            //im dead
            if (Death != null)
                Death();
            gameObject.SetActive(false);
        }

    }
    
    
    
    
    //TODO: tells it whether to walk to or attack enemy
    void UpdateEnemyAction()
    {
        if((enemyAgent.transform.position - transform.position).magnitude <= attackRange)
        {
            myActions.Add((int)ActionDefines.Attack);
        }
        else
        {
            targetPosition = enemyAgent.transform.position;
            myActions.Add((int)ActionDefines.MoveTo);
        }
    }

    void MyTargetDied()
    {
        if(!enemyAgent)
        {
            ResetActions();
            return;
        }
        if (enemyAgent.Death != null)
        {
            enemyAgent.Death -= MyTargetDied;
        }
        enemyAgent = null;
        ResetActions();
    }


    //Start, Update's, Exits, OnCollisions, or any unity/c# specific functions
    private void OnEnable()
    {
        //Get the game state if we do not already have it
        if(stateController == null)
        {
            stateController = GameObject.Find("GameState").GetComponent<GameStateController>();
        }
        myActions.Clear();
        if(DebugLine != null)
        {
            GameObject temp = Instantiate(DebugLine, transform.position, Quaternion.identity);
            debugLineScript = temp.GetComponent<LineRenderer>();
        }
        for (int i = 0; i < healthbar.Length; i++)
        {
            if(healthbar[i])
            {
                healthbar[i].gameObject.SetActive(true);
            }
        }
        toCaptureOutpost = null;
        enemyAgent = null;
        //evaluate for the first time our list of tasks
        myActions.Add((int)ActionDefines.ReEvaluate);
        Act();
    }

    private void OnDisable()
    {
        //lane = -1;
        //targetPosition = Vector3.zero; //for storing the position to move to
        //enemyBase = null;
        //myLane = null;
        //currAction = -1;
        // myActions.Clear();
        if(debugLineScript)
        Destroy(debugLineScript.gameObject);
        debugLineScript = null;
    }

    private void FixedUpdate()
    {
        reEvaluateTimer += Time.deltaTime;
        if(reEvaluateTimer >= 2 && !teleported)
        {
            reEvaluateTimer = 0;
            ResetActions();
        }
        Act();
    }

    private void OnTriggerStay(Collider other)
    {
        if(enemyAgent == null && other.gameObject != this)
        {
            if(other.tag == "Unit")
            {
                AgentController enemy = other.gameObject.GetComponent<AgentController>();
                if(enemy != null)
                {
                    if (enemy.side == side)
                        return;
                    enemyAgent = enemy;
                    enemyAgent.Death += MyTargetDied;
                    if (enemy.enemyAgent == null)
                    {
                        enemy.enemyAgent = this;
                        enemy.ResetActions();
                    }
                    ResetActions();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (enemyAgent == null && other.gameObject != this)
        {
            if (other.tag == "Unit")
            {
                AgentController enemy = other.gameObject.GetComponent<AgentController>();
                if (enemy != null)
                {
                    if (enemy.side == side)
                        return;
                    enemyAgent = enemy;
                    enemyAgent.Death += MyTargetDied;
                    if (enemy.enemyAgent == null)
                    {
                        enemy.enemyAgent = this;
                        enemy.ResetActions();
                    }
                    ResetActions();
                }
            }
        }
    }



}
