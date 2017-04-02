using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FlockManager : MonoBehaviour {


    public delegate void EventUpdate(float dt);
    public static event EventUpdate UpdateEvent;
    
    // Use this for initialization
    public OutpostCapture[] outposts = new OutpostCapture[4];
    public List<Flock> RedFlocks = new List<Flock>();
    public List<Flock> BlueFlocks = new List<Flock>();

    float timer = 0;
    public float TargetUpdateTime = 2;
    int currCheck = 0;
    public Transform blueBasePortal, redBasePortal;
    public BaseHealth blueBaseHealth, redBaseHealth;
    bool nighttime = false;

    private void OnEnable()
    {
        DayNightCycle.NightTimeSwitch += NightTimeSwitch;
    }

    private void OnDisable()
    {
        DayNightCycle.NightTimeSwitch -= NightTimeSwitch;
    }

    void NightTimeSwitch(bool nightTime)
    {
        nighttime = nightTime;
        for (int i = 0; i < RedFlocks.Count; i++)
        {
            RedFlocks[i].sleeping = nighttime;
            RedFlocks[i].targetPos = Vector3.zero;

        }
        for (int i = 0; i < BlueFlocks.Count; i++)
        {
            BlueFlocks[i].sleeping = nighttime;
            BlueFlocks[i].targetPos = Vector3.zero;
        }
    }

    public void FindSleepingOutpost(Flock _flock)
    {
    
        Vector3 closest = new Vector3(0, 1000, 0);
        int any = 0;
        for (int i = 0; i < outposts.Length; i++)
        {
            Vector3 dist = outposts[i].transform.position - _flock.transform.position;

            if (dist.magnitude <= (closest - _flock.transform.position).magnitude && outposts[i].currType == _flock.side)
            {
                closest = outposts[i].transform.position;
                any++;
            }
        }
        if(any == 0)
        {
            _flock.targetPos = outposts[0].transform.position;
        }
        else
        {
            _flock.targetPos = closest;
        }

        
    }
    private void FixedUpdate()
    {
        float dt = Time.deltaTime;
        timer += dt;
        if(timer >= 2)
        {
            timer = 0;
            switch (currCheck)
            {
                case 0:
                    {

                        MergeRedFlocks();
                        currCheck++;
                        break;
                    }
                case 1:
                    {
                        MergeBlueFlocks();
                        currCheck = 0;
                        break;
                    }
            }
        }
        if(UpdateEvent != null)
            UpdateEvent(dt);
    }

    public void AddFlock(Flock _flock)
    {
        _flock.FM = this;
        if (_flock.side == 1)
            RedFlocks.Add(_flock);
        else
            BlueFlocks.Add(_flock);
        _flock.sleeping = nighttime;
    }

    public void RemoveFlock(Flock _flock)
    {
        if (_flock.side == 1)
            RedFlocks.Remove(_flock);
        else
            BlueFlocks.Remove(_flock);
        _flock.sleeping = nighttime;

    }

    public void DamageBlueBase()
    {
        
        blueBaseHealth.Damage();
        if(blueBaseHealth.health <= 0)
        {
            GameStateManager.managerinstance.ChangeState(GameStateManager.GameState.Win);
        }
    }

    public void DamageRedBase()
    {
        redBaseHealth.Damage();
        if (redBaseHealth.health <= 0)
        {
            GameStateManager.managerinstance.ChangeState(GameStateManager.GameState.Lose);
        }
    }


    private void MergeRedFlocks()
    {
        Vector3 dist;
        for (int f = 0; f < RedFlocks.Count; f++)
        {

            for (int i = 0; i < RedFlocks.Count; i++)
            {
                if (i != f)
                {
                    dist = RedFlocks[f].transform.position - RedFlocks[i].transform.position;
                    if (dist.magnitude <= 3)
                    {
                        if (RedFlocks[f].currOutpost == null && RedFlocks[i].currOutpost != null)
                        {
                            for (int j = 0; j < RedFlocks[f].boids.Count; j++)
                            {
                                RedFlocks[i].boids.Add(RedFlocks[f].boids[j]);
                            }
                            Flock destroyMe = RedFlocks[f];
                            RedFlocks.Remove(RedFlocks[f]);
                            Destroy(destroyMe.gameObject);
                        }
                        else
                        {
                            for (int j = 0; j < RedFlocks[i].boids.Count; j++)
                            {
                                RedFlocks[f].boids.Add(RedFlocks[i].boids[j]);
                            }
                            Flock destroyMe = RedFlocks[i];
                            RedFlocks.Remove(RedFlocks[i]);
                            Destroy(destroyMe.gameObject);
                        }
                    }
                }
            }
        }
    }

    private void MergeBlueFlocks()
    {
        Vector3 dist;
        for (int f = 0; f < BlueFlocks.Count; f++)
        {

            for (int i = 0; i < BlueFlocks.Count; i++)
            {
                if (i != f)
                {
                    dist = BlueFlocks[f].transform.position - BlueFlocks[i].transform.position;
                    if (dist.magnitude <= 3)
                    {
                        if (BlueFlocks[f].currOutpost == null && BlueFlocks[i].currOutpost != null)
                        {
                            for (int j = 0; j < BlueFlocks[f].boids.Count; j++)
                            {
                                BlueFlocks[i].boids.Add(BlueFlocks[f].boids[j]);
                            }
                            Flock destroyMe = BlueFlocks[f];
                            BlueFlocks.Remove(BlueFlocks[f]);
                            Destroy(destroyMe.gameObject);
                        }
                        else
                        {
                            for (int j = 0; j < BlueFlocks[i].boids.Count; j++)
                            {
                                BlueFlocks[f].boids.Add(BlueFlocks[i].boids[j]);
                            }
                            Flock destroyMe = BlueFlocks[i];
                            BlueFlocks.Remove(BlueFlocks[i]);
                            Destroy(destroyMe.gameObject);
                        }
                    }
                }
            }
        }
    }

    public bool CheckForCombat(Flock _flock)
    {
        if (_flock == null)
            return false;
        //_flock.SetEnemyFlock(enemyFlock);
        if(_flock.side == 1)
        {
            //search Blue Troops
            for (int i = 0; i < BlueFlocks.Count; i++)
            {
                if(( BlueFlocks[i].transform.position - _flock.transform.position).magnitude <= 7)
                {
                    BlueFlocks[i].SetEnemyFlock(_flock);
                    _flock.SetEnemyFlock(BlueFlocks[i]);
                    BlueFlocks[i].targetPos = _flock.transform.position;
                    _flock.targetPos = BlueFlocks[i].transform.position;
                    return true;
                }
            }
        }
        else
        {
            //search Red Troops
            for (int i = 0; i < RedFlocks.Count; i++)
            {
                if ((RedFlocks[i].transform.position - _flock.transform.position).magnitude <= 7)
                {
                    RedFlocks[i].SetEnemyFlock(_flock);
                    _flock.SetEnemyFlock(RedFlocks[i]);
                    RedFlocks[i].targetPos = _flock.transform.position;
                    _flock.targetPos = RedFlocks[i].transform.position;
                    return true;
                }
            }
        }

        return false;
    }

    private void UpdateTargets()
    {

        for (int f = 0; f < RedFlocks.Count; f++)
        {
            FindNearestOutposts(RedFlocks[f]);
        }
    }

    public void FindNearestOutposts(Flock _flock)
    {
        Vector3 closest = new Vector3(0, 1000, 0);
        int same = 0;
        for (int i = 0; i < outposts.Length; i++)
        {
            if (outposts[i].currType == _flock.side)
            {
                same++;
                continue;

            }
            //find nearest outpost
            Vector3 dist = outposts[i].transform.position - _flock.transform.position;
            if (dist.magnitude <= (closest - _flock.transform.position).magnitude)
            {
                
                if (dist.magnitude <= 1 && _flock.currAI_State == (int)Flock.AI_States.Capturing)
                {
                    if (!outposts[i].StartCapture(_flock))
                    {
                        continue;
                    }
                    

                }
                closest = outposts[i].transform.position;
            }
        }
        if(_flock.currAI_State != (int)Flock.AI_States.Capturing)
            _flock.targetPos = closest;
        if (same == outposts.Length)
        {
            _flock.currAI_State = (int)Flock.AI_States.Moving;
            //blue is 1, red is 2
            if (_flock.side == 1)
            {
                if (outposts[1].Portal != null)
                {
                    _flock.targetPos = outposts[1].transform.position;
                    _flock.teleportTrans = blueBasePortal;
                    _flock.goingToTeleport = true;
                }
            }
            else
            {
                if (outposts[2].Portal != null)
                {
                    _flock.targetPos = outposts[2].transform.position;
                    _flock.teleportTrans = redBasePortal;
                    _flock.goingToTeleport = true;
                }
            }
        }


    }
}
