using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
public class PortalSpawner : MonoBehaviour {

    float onethird = .33f;
    float twothird = .66f;
    float half = .5f;

    [SerializeField]
    FlashPortal portal, lowerPortal;
    
    public ObjectPooler[] OP = new ObjectPooler[3];
    public Transform[] SpawnPositions;
    public OutpostController[] myOutposts;
    public int currSpawnPosIndex = 0;
    public int numOutposts = 1;
    bool spawning = false;
    public LinearMapping linearmap = null;
    public int lane = 0;
    public BlacksmithBaseController enemyBase;
    public LaneController myLane;
    public GameStateController stateController;
    public int side = 1;
	// Use this for initialization
	void Start () {
	}

    private void FixedUpdate()
    {
        UpdateLowerPortals();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "SpawnObject" && !spawning)
        {
            if (!other.transform.parent)
            {
                if (portal != null)
                    portal.Flash();
                if (lowerPortal != null)
                    lowerPortal.Flash();
                int tempType = other.gameObject.GetComponent<ToySpawner>().type;
                spawning = true;
                Spawn(tempType);
                spawning = false;

                Destroy(other.gameObject);
            }
            else if (other.transform.parent)
            {
                if (portal != null)
                    portal.Set(true);
                if (lowerPortal != null)
                    lowerPortal.Set(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "SpawnObject" && !spawning)
        {
            if (!other.transform.parent)
            {
                if (portal != null)
                    portal.Set(true);
                if (lowerPortal != null)
                    lowerPortal.Set(true);
                int tempType = other.gameObject.GetComponent<ToySpawner>().type;
                spawning = true;
                Spawn(tempType);
                spawning = false;
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SpawnObject")
        {
            if (portal != null)
                portal.Set(false);
            if (lowerPortal != null)
                lowerPortal.Set(false);
        }
    }

    private void Spawn(int type)
    {
        if (spawning)
        {
                if (!UpdateLowerPortals())
                {
                    currSpawnPosIndex = 0;
                }
                //new stuff
                GameObject temp = OP[type].Spawn(SpawnPositions[currSpawnPosIndex].transform);
            if (temp == null)
                return;
                temp.transform.position = new Vector3(temp.transform.position.x, 0, temp.transform.position.z);
                AgentController agentOBJ = temp.GetComponent<AgentController>();

                if (agentOBJ != null)
                {
                    agentOBJ.side = side;
                    agentOBJ.lane = lane;
                    agentOBJ.myLane = myLane;
                    agentOBJ.stateController = stateController;
                    agentOBJ.enemyBase = enemyBase;
                }
                if (!portal.flashing)
                {
                    if (portal != null)
                        portal.Set(false);
                    if (lowerPortal != null)
                        lowerPortal.Set(false);
                }
            }
        
    }

    private bool UpdateLowerPortals()
    {
        if (linearmap != null)
        {
            Vector3 verticalOffset = new Vector3(0, 2, 0);
            float tempval = linearmap.value;
            if (numOutposts == 3)
            {
                if (tempval < twothird && tempval > onethird)
                {
                    //two thirds
                    //middle distance
                    currSpawnPosIndex = 1;


                }
                else if (tempval < onethird)
                {
                    //one third
                    //closest portal
                    currSpawnPosIndex = 0;
                }
                else
                {
                    //all the way
                    //furthest portal
                    currSpawnPosIndex = 2;

                }
            }
            else if (numOutposts == 2)
            {
                if (tempval < half)
                {
                    currSpawnPosIndex = 0;
                }
                else
                {
                    currSpawnPosIndex = 1;
                }
            }
            if (myOutposts[currSpawnPosIndex])
            {
                if (myOutposts[currSpawnPosIndex].side == side)
                {
                    lowerPortal.gameObject.transform.position = SpawnPositions[currSpawnPosIndex].transform.position + verticalOffset;
                }
                else
                {
                    return false;
                }
            }

        }
        return true;
    }
    

}
