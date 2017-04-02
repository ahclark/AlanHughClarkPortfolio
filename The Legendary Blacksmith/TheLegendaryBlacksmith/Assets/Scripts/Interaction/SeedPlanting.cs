using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SeedPlanting : MonoBehaviour
{
    [SerializeField]
    GameObject seed;
    [SerializeField]
    GardenController plot;
    [SerializeField]
    GameObject growWood;
    [SerializeField]
    GameObject bulge;
    public bool planted = false;
    Vector3 startPos;
    Vector3 startScale;
    Vector3 endScale;
    [SerializeField]
    float speed = 1;
    public float growing = 0;
    [SerializeField]
    float growtime = 10;
    [SerializeField]
    bool canGrow = false;

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
        startScale = new Vector3(0.001f, 0.001f, 0.001f);
        endScale = new Vector3(1, 1, 1);
        growWood.transform.localScale = startScale;
        LevelBlockingTools.LevelBlockingMesh bark = growWood.GetComponentInChildren<LevelBlockingTools.LevelBlockingMesh>();
        bulge.SetActive(false);
        growWood.SetActive(false);
        if (bark)
        {
            DestroyImmediate(bark.meshFilter.sharedMesh);
            bark.CreateMesh();
            bark.UpdateMesh();
        }
    }

    void Update()
    {

        if (canGrow == true)
        {
            if (growing < growtime)
            {
                growWood.transform.localScale = Vector3.Lerp(startScale, endScale, growing);
            }
            growing += Time.deltaTime * speed;
            if (growing >= growtime)
            {
                canGrow = false;
                SpawnWood();
                growWood.SetActive(false);
            }
        }
    }

    private void PlantSomething()
    {
        planted = true;
    }

    void OnTriggerEnter(Collider entity)
    {
        if (!planted && entity.name == "Seed")
        {
            //seed = entity.gameObject;
            //PlantSomething();
            if (entity.transform.parent)
            {
                entity.transform.parent.gameObject.GetComponent<Hand>().DetachObject(entity.gameObject);
            }
            entity.gameObject.SetActive(false);
            Destroy(entity.gameObject);
            Consume();
        }
    }

    void OnTriggerStay(Collider entity)
    {
        if (!planted && entity.name == "Seed")
        {
            //seed = entity.gameObject;
            //PlantSomething();
            if (entity.transform.parent)
            {
                entity.transform.parent.gameObject.GetComponent<Hand>().DetachObject(entity.gameObject);
            }
            entity.gameObject.SetActive(false);
            Destroy(entity.gameObject);
            Consume();
        }
    }

    public void Consume()
    {
        //if (seed.transform.parent)
        //{
        //    seed.transform.parent.gameObject.GetComponent<Hand>().DetachObject(seed);
        //}
        bulge.SetActive(true);
        //seed.SetActive(false);
        //Destroy(seed);
        planted = true;
        growWood.SetActive(true);
        canGrow = true;
        growing = 0;
        
        
        
    }
    void SpawnWood()
    {
        //GameObject newObject = Instantiate(wood);
        //newObject.transform.position = startPos;
        //newObject.name = "Wood";

        /*Benjamin Ousley
          Changed the wood to spawn at the trigger's current position instead of the trigger's position at start
          Made this change because when the garden plot was moved in-game, the wood was spawning at the original position instead of the new position
          1/27/2017*/
        plot.wood[plot.woodCount].transform.position = transform.position;

           
        /*Edited by Benjamin Ousley
          Made this change because when the garden plot was moved in-game, the wood was spawning at the original position instead of the new position
          plot.wood[plot.woodCount].transform.position = startPos;*/

        plot.wood[plot.woodCount].SetActive(true);
        plot.wood[plot.woodCount].name = "Wood";
        plot.IncreaseCount();
        GrabWood();
    }
    public void GrabWood()
    {
        bulge.SetActive(false);
        growing = 0;
        planted = false;
    }

}
