//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LaneManager : MonoBehaviour {


//    public int index = 0;
//    List<MinionController> Units = new List<MinionController>();
//    public Transform[] waypoints;
//    public GameObject[] Bases;
//    public OutpostCapture[] outposts = new OutpostCapture[3];
//	// Use this for initialization
//	void Start () {
//	}
	
//	// Update is called once per frame
//	void Update () {
		
//	}

//    private void FixedUpdate()
//    {
        
//    }

//    public void AddUnit(GameObject unit, bool player)
//    {
//        MinionController tempCont = unit.GetComponent<MinionController>();
//        if (tempCont != null)
//        {
//            tempCont.Player = player;
//            tempCont.index = Units.Count;
//            tempCont.laneManager = gameObject.GetComponent<LaneManager>();
//            Units.Add(tempCont);
//        }
//    }

//    public void RemoveUnit(int index)
//    {
//        Units.RemoveAt(index);
//    }

//    public OutpostCapture GetClosestOutpost(bool player, int laneIndex, Transform myTrans)
//    {
//        Vector3 dist, longest = new Vector3(0, 50, 0);
//        OutpostCapture temp = null;
//        for (int i = 0; i < 3; i++)
//        {
//            //is this outpost taken by my team?
//            if((outposts[i].EnemyOwned && !player) || (outposts[i].PlayerOwned && player))
//            {
//                continue;
//            }

//            dist = outposts[i].transform.position - myTrans.position;
//            if(dist.magnitude < longest.magnitude)
//            {
//                longest = dist;
//                temp = outposts[i];
//            }
//        }
//        return temp;
//    }

//    public Transform GetWaypoint(int index)
//    {
//        if (index < waypoints.Length)
//            return waypoints[index];
//        else
//            return transform;
//    }

//    public GameObject GetBase(int index)
//    {
//        return Bases[index];
//    }

//    //public MinionController GetClosestEnemy(bool player, Transform myTrans)
//    //{
//    //    //search for unit that is close.
//    //    MinionController closest = null;
//    //    Vector3 dist, bestdist = new Vector3(0, 15, 0);
//    //    for (int i = 0; i < Units.Count; i++)
//    //    {
//    //        if(Units[i].Player != player && Units[i] != null)
//    //        {
//    //            dist = Units[i].transform.position - myTrans.position;
//    //            if(dist.magnitude < bestdist.magnitude)
//    //            {
//    //                closest = Units[i];
//    //                bestdist = dist;
//    //            }
//    //        }
//    //    }

//    //    return closest;
//    //}


//}
