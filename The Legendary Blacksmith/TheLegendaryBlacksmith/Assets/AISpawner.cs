using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AISpawner : MonoBehaviour
{
    [SerializeField]
    FlashPortal portal = null;

    public int side = -1;
    public ObjectPooler[] OP = new ObjectPooler[3];
    public Transform[] SpawnPositions;
    int currSpawnPosIndex = 0;
    public int lane = 0;
    public BlacksmithBaseController enemyBase;
    public LaneController myLane;
    public GameStateController stateController;
    // Use this for initialization
    float timer = 0;
    [SerializeField]
    float timebetweenWaves = 30;


    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!DayNightCycle.dayInstance.starting)
            return;
        timer += Time.deltaTime;
        if (timer >= timebetweenWaves)
        {
            timer = 0;
            Spawn(Random.Range(0, 2));
        }
    }



    private void Spawn(int type)
    {

        GameObject temp = OP[type].Spawn(SpawnPositions[currSpawnPosIndex].transform);
        if(temp == null)
        {
            return;
        }
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

    }

}
