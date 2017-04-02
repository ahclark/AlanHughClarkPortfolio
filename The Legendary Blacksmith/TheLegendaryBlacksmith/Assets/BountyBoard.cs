using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyBoard : MonoBehaviour {

    bool active = false;
    float timer = 60;
    public GameObject WorkOrderPrefab;
    public float refreshRate = 60;


    private void FixedUpdate()
    {
            timer += Time.deltaTime;
            if(timer >= refreshRate)
            {
                timer = 0;
                Activate();
            }
        
    }


    void Activate()
    {
        if(WorkOrderPrefab)
        {
            GameObject temp = Instantiate(WorkOrderPrefab, transform.position, Quaternion.identity);
            if(temp)
            {
                temp.name = "WorkOrder";
            }
        }
    }

    
}
