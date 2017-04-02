using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class DispenserController : MonoBehaviour
{
    [SerializeField]
    int resources = 0;
    [SerializeField]
    GameObject[] pucks;
    public int whichPuck = 0;
    [SerializeField]
    LinearMapping distance;
    [SerializeField]
    GameObject spawnPoint;
    int cost = 9;
    [SerializeField]
    TextMesh resourceCount;
    [SerializeField]
    BoxCollider barrier;
    [SerializeField]
    int maxResources = 8;
    //Serialized array of scrap objects added by Jish to implement scrap spawning 1/25/2017
    [SerializeField]
    GameObject[] scraps;
    //Transform for the spawn point of the scraps added by Jish to implement scrap spawning 1/25/2017
    [SerializeField]
    GameObject[] scrapSpawns;
    //for testing random spawn of 3 scraps 1/25/2017 JMR
    [SerializeField]
    int whichScrap;
    //for testing random number of spawned scraps 1/28/2017 JMR
    [SerializeField]
    int scrapQuantity;
    [SerializeField]
    float betweenTimer = 0.25f;
    [SerializeField]
    bool cooldown = false;
    float timer = 0;

	// Use this for initialization
	void Start ()
    {
        UpdateResources();
	}
    void Update()
    {
        if (cooldown == true)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime * 1;
            }
            else
            {
                cooldown = false;
            }
        }
    }

    private void OnTriggerEnter(Collider entity)
    {
        if (resources < maxResources)
        {
            if (entity.tag == "Ore")
            {
                resources++;
                
                UpdateResources();

                if (entity.transform.parent)
                {
                    entity.transform.parent.gameObject.GetComponent<Hand>().DetachObject(entity.gameObject);
                }
                entity.GetComponent<BeingHandled>().handled = false;
                entity.gameObject.SetActive(false);
            }
        }
    }

    public void SpawnPuck()
    {
        SetPuck();
        if (cost <= resources && !cooldown)
        {
            Instantiate(pucks[whichPuck], spawnPoint.transform.position, spawnPoint.transform.rotation);
            resources -= cost;
            UpdateResources();
            timer = betweenTimer;
            cooldown = true;
        }
    }

    public void SetPuck()
    {
        //changed by josh 2/19/2017 to update the dispenser to the new crafting system
        whichPuck = 0;
        cost = 2;
        //if (distance.value <= 0.32)
        //{
        //    whichPuck = 0;
        //    cost = 2;
        //}
        //else if (distance.value > 0.32 && distance.value < 0.66)
        //{
        //    whichPuck = 1;
        //    cost = 3;
        //}
        //else if (distance.value > 0.65)
        //{
        //    whichPuck = 2;
        //    cost = 1;
        //}
        //else
        //    Debug.Log("somehow you screwed up you moron");
    }

    void UpdateResources()
    {
        resourceCount.text = "" + resources + "";
        if (resources >= maxResources)
        {
            barrier.isTrigger = false;
        }
        else
        {
            barrier.isTrigger = true;
        }
    }
    
    public void SpawnScrap()
    {
        if (resources > 0)
        {
            resources--;
            UpdateResources();
            //scrapQuantity random set to spawn a random number of scrap pieces as an ore is processed 1/28/2017 JMR
            scrapQuantity = Random.Range(1, 4);
            //for loop to instantiate the correct number of scrap pieces predetermined by the scrapQuantity 1/28/2017 JMR
            for (int i = 0; i < scrapQuantity; i++)
            {
                whichScrap = Random.Range(0, 3);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Instantiate call for the spawning of scraps implemented by Jish to implement scrap spawning 1/25/2017
                Instantiate(scraps[whichScrap], scrapSpawns[i].transform.position, scrapSpawns[i].transform.rotation).name = "Scrap";
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
        }
    }

}
