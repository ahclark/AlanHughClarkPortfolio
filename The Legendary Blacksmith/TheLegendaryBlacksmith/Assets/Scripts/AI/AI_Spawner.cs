//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AI_Spawner : MonoBehaviour {
//    public ObjectPooler[] OP;
//    float timer = 60;
//    public float WaveTimer, SpawnTimer;
//    bool spawning = false;
//    int spawnCount = 0;
//    public GameObject[] UnitPrefabs;
//    public Transform[] LaneTransforms;
//    public int[] WavePattern;
//    public GameObject[] LM;
//    LaneManager[] laneManager = new LaneManager[3];
//	// Use this for initialization
//	void Start () {
//        for (int i = 0; i < LM.Length; i++)
//        {
//            laneManager[i] = LM[i].GetComponent<LaneManager>();
//            print(laneManager[i]);
//        }
       
//	}
	
//	// Update is called once per frame
//	void Update () {
		
//	}

//    private void FixedUpdate()
//    {
//        timer += Time.deltaTime;
//        if(!spawning)
//        {
//            if(timer >= WaveTimer)
//            {
//                timer = 0;
//                spawning = true;
//            }
//        }
//        else
//        {
//            if(timer >= SpawnTimer)
//            {
//                timer = 0;
//                Spawn();
//            }
//            if(spawnCount == WavePattern.Length)
//            {
//                spawnCount = 0;
//                spawning = false;
//            }
//        }
//    }

//    private void Spawn()
//    {
//        for (int i = 0; i < LaneTransforms.Length; i++)
//        {
//            Transform tempTrans = LaneTransforms[i];
//            GameObject temp = OP[WavePattern[spawnCount]].Spawn(tempTrans);
//            laneManager[i].AddUnit(temp, false);
//        }
       
//        spawnCount++;
//    }
//}
