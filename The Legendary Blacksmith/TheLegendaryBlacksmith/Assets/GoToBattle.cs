using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToBattle : MonoBehaviour {
    NavMeshAgent myAgent;

    GameObject cloud;
    GameObject mBar;

    [SerializeField]
    float ArrivalMoraleBoost = .05f;

	// Use this for initialization
	void Start () {
        cloud = GameObject.Find("Battle 1");
        mBar = GameObject.Find("MoraleBar");
        myAgent = GetComponent<NavMeshAgent>();
        if (!myAgent)
            gameObject.SetActive(false);
        else
        {
            if(cloud)
            {
                myAgent.destination = cloud.transform.position;
            }
        }
	}
	



    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Cloud")
        {
            if(mBar)
            {
                moraleBar temp = mBar.GetComponent<moraleBar>();
                temp.AddMorale(ArrivalMoraleBoost);

            }
            Destroy(gameObject);
        }
    }
}
