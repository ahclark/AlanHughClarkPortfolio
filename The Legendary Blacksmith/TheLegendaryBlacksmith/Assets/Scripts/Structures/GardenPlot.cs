using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenPlot : MonoBehaviour
{
    [SerializeField]
    GameObject Filled;
    [SerializeField]
    GameObject Dig;
    [SerializeField]
    bool isOpen = false;
    [SerializeField]
    bool isFull = false;
    [SerializeField]
    string[] acceptables;
    string planted;
    int counter = 0;
    [SerializeField]
    GameObject[] fruitsOfLabor;
    [SerializeField]
    float growthSpeed = 1;
    


    // Use this for initialization
    void Start ()
    {
        CheckPlanter();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void CheckPlanter()
    {
        if (isOpen == true)
        {
            Dig.SetActive(false);
        }
        else
        {
            Dig.SetActive(true);
        }
        if (isFull == true)
        {
            Filled.SetActive(true);
            GrowingResource();
        }
        else
        {
            Filled.SetActive(false);
        }
    }

    void GrowingResource()
    {
        float growth = 0;
        Vector3 start = new Vector3(0, 0, 0);
        Vector3 finish = new Vector3(1, 1, 1);
        GameObject crop = fruitsOfLabor[counter];
        Instantiate(crop);
        while(growth <= 1)
        {
            crop.transform.localScale = Vector3.Lerp(start, finish, growth);
            growth += Time.deltaTime * growthSpeed;
        }
        crop.transform.localScale = finish;
    }

    private void OnTriggerEnter(Collider entity)
    {
        /*if (entity == controller / tool)
        {
            if (sideButtonsSqueezed && isOpen != true)
            {
                isOpen = true;
                isFull = false;
                planted = null;
                CheckPlanter();
            }
            else
            if (isOpen == true && control.triggerPressed)
            {
                foreach (string i in acceptables)
                {
                    int increment = 0;
                    if (entity.name == i)
                    {
                        planted = entity.name;
                        isOpen = false;
                        isFull = true;
                        counter = increment;
                        CheckPlanter();
                    }
                    increment++;
                }
            }
        }*/
        
    }
}
