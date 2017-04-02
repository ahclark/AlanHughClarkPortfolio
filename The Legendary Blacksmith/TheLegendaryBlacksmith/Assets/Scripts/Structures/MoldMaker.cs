using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class MoldMaker : MonoBehaviour
{
    public int Scraps = 0;
    List<Collider> scraplist;
    [SerializeField]
    GameObject scrapBar;
    [SerializeField]
    GameObject ring;
    [SerializeField]
    GameObject spawn;
    [SerializeField]
    GameObject rimSpawn;
    [SerializeField]
    LinearMapping LinMap;
    [SerializeField]
    ScrapPress press;
    [SerializeField]
    GameObject shield;
    public UnityEvent OnSmash;


    // Use this for initialization
    void Start ()
    {
        scraplist = new List<Collider>();
    }

    private void OnTriggerEnter(Collider entity)
    {
        if (!scraplist.Contains(entity) && entity.name == "Scrap")
        {
            scraplist.Add(entity);
            Scraps++;
        }
        if (entity.name == "Shield_Rimmed")
        {
            shield = entity.gameObject;
            press.shieldIn = true;
        }
    }
    private void OnTriggerExit(Collider entity)
    {
        if (scraplist.Contains(entity) && entity.name == "Scrap")
        {
            scraplist.Remove(entity);
            Scraps--;
        }
        if (entity.name == "Shield_Rimmed")
        {
            shield = null;
            press.shieldIn = false;
        }
    }
    public void RemoveScraps()
    {
        OnSmash.Invoke();
        List<Collider> Destruction = new List<Collider>();
        for (int i = Scraps; i >= 3; i-=3)
        {
            if (LinMap.value < 1)
            {
                Instantiate(scrapBar, spawn.transform.position, spawn.transform.rotation).name = "Scrap Bar";
                
                LinMap.value = 0;
            }
            else
            {
                Instantiate(ring, rimSpawn.transform.position, rimSpawn.transform.rotation).name = "ShieldRim";
            }
            //Scraps -= 3;
            Scraps = 0;
        }
        foreach(Collider thing in scraplist)
        {
            //scraplist.Remove(thing);
            if (thing.transform.parent)
            {
                Hand temp = thing.transform.parent.gameObject.GetComponent<Hand>();
                if(temp)
                    temp.DetachObject(thing.gameObject);
            }
            thing.gameObject.SetActive(false);
            //Destroy(thing.gameObject);
            

            Destruction.Add(thing);
            

        }
        scraplist.Clear();
        
        Scraps = 0;
    }
    public void SquishShield()
    {
        shield.GetComponent<ShieldMaker>().ItsMorphinTime();
    }

}
