using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class EquipWeapon : MonoBehaviour {

    [SerializeField]
    MeshRenderer[] MR;
    [SerializeField]
    Material RedMat;
    [SerializeField]
    Material YellowMat;
    [SerializeField]
    GameObject Heavy;
    [SerializeField]
    GameObject Light;
    [SerializeField]
    GameObject Ranged;
    [SerializeField]
    GameObject Spawner;
    [SerializeField]
    EquipSpawn ES;
    [SerializeField]
    GameObject TopParent;
    float timer = 0;

    public Transform[] spawnPositions;
    public GameObject[] unitPrefabs;
    GameObject redUnitContainer;
    UnitContainer redUnits;

	// Use this for initialization
	void Start () {
        foreach (MeshRenderer thing in MR)
        {
            thing.material = RedMat;
        }

        redUnitContainer = GameObject.Find("RedUnitContainer");
        if(redUnitContainer)
        {
            redUnits = redUnitContainer.GetComponent<UnitContainer>();
        }
	}

    void OnTriggerStay(Collider col)
    {
        //if (col.tag == "Equippable" && col.transform.parent != null)
        //{
        //    foreach (MeshRenderer thing in MR)
        //    {
        //        thing.material = YellowMat;
        //    }
        //}
        if (col.tag == "Equippable")
        {
            GameObject m_object = col.gameObject;
            if(!m_object)
            {
                return;
            }
            switch(col.name)
            {
                case "FullSword":

                    if (redUnits)
                        redUnits.AddUnit(Instantiate(unitPrefabs[0], spawnPositions[0].position, spawnPositions[0].rotation));

                    break;
                case "FullShield 1":

                    if (redUnits)
                        redUnits.AddUnit(Instantiate(unitPrefabs[2], spawnPositions[0].position, spawnPositions[0].rotation));

                    break;
                case "FullArrow":

                    if (redUnits)
                        redUnits.AddUnit(Instantiate(unitPrefabs[1], spawnPositions[0].position, spawnPositions[0].rotation));

                    break;
                case "WorkOrder":
                    //print("We saw your name");
                    //WorkOrderController temp = col.GetComponentInChildren<WorkOrderController>();
                    
                    //if (temp)
                    //{
                    //    if (temp.workOrderComplete)
                    //    {
                    //        int[] types = new int[3];
                    //        types[0] = temp.leftNum;
                    //        types[1] = temp.rightNum;
                    //        types[2] = temp.midNum;
                    //        for (int i = 0; i < 3; i++)
                    //        {
                    //           if(redUnits)
                    //           redUnits.AddUnit( Instantiate(unitPrefabs[types[i]], spawnPositions[i].position, spawnPositions[i].rotation));

                    //        }
                           
                    //    }
                    //    else
                    //    {
                    //        return;
                    //    }

                    //}
                    break;
                default:
                    break;
            }
            if (m_object.transform.parent)
            {
                Hand thehand = m_object.transform.parent.gameObject.GetComponent<Hand>();
                if (thehand)
                    thehand.DetachObject(m_object);
            }
            m_object.SetActive(false);
            Destroy(m_object);
            foreach (MeshRenderer thing in MR)
            {
                thing.material = RedMat;
            }
            TopParent.gameObject.SetActive(false);
            ES.UnitSpawn();
        }
    }

    void OnTriggerExit(Collider col)
    {
        foreach (MeshRenderer thing in MR)
        {
            thing.material = RedMat;
        }
    }
}
