using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;
public class Quest : MonoBehaviour
{
    [SerializeField]
    Transform WorkOrderTrans;
    [SerializeField]
    Transform CoinTrans;
    [SerializeField]
    GameObject WorkOrderPrefab;
    [SerializeField]
    GameObject CoinPrefab;
    [SerializeField]
    GameObject Icon;
    [SerializeField]
    string QuestItemName;
    [SerializeField]
    float moraleIncrease = .1f;
    [SerializeField]
    GameObject TroopPrefab;
    [SerializeField]
    Transform TroopSpawnPosition;

    public UnityEvent onTurnIn;

    moraleBar mBar;
    private void Start()
    {
        // SpawnWorkOrder();
        GameObject moraleObj = GameObject.Find("MoraleBar");
        if (moraleObj)
        {
            mBar = moraleObj.GetComponent<moraleBar>();
        }
    }


    void SpawnWorkOrder()
    {
        GameObject toChange = Instantiate(WorkOrderPrefab, WorkOrderTrans.position, WorkOrderTrans.rotation);
        if (toChange)
        {
            toChange.name = "WorkOrder";
            WorkOrderController toChangeWorkOrder = toChange.GetComponent<WorkOrderController>();
            if (toChangeWorkOrder)
            {
                toChangeWorkOrder.NewWorkOrder(Icon, QuestItemName);
            }
        }
    }



    public void TurnIn(GameObject other)
    {
        if (other)
        {
            //if it is complete, spawn gold or react however

            //boost morale
            mBar.AddMorale(moraleIncrease);
            //give gold
            GameObject coin = Instantiate(CoinPrefab, CoinTrans.position, CoinTrans.rotation);
            coin.name = CoinPrefab.name;
            if (TroopPrefab && TroopSpawnPosition)
            {
                Instantiate(TroopPrefab, TroopSpawnPosition.position, TroopSpawnPosition.rotation);
            }
            // SpawnWorkOrder();
            if (onTurnIn != null)
                onTurnIn.Invoke();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == QuestItemName)
        {

            if (other.gameObject.transform.parent == null)
            {
                TurnIn(other.gameObject);
                other.gameObject.SetActive(false);
            }
            else
            {
                Hand theHand = other.gameObject.transform.parent.GetComponent<Hand>();
                if (!theHand)
                {
                    TurnIn(other.gameObject);
                    other.gameObject.SetActive(false);
                }
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name == QuestItemName)
        {

            if (other.gameObject.transform.parent == null)
            {
                TurnIn(other.gameObject);
                other.gameObject.SetActive(false);
            }
            else
            {
                Hand theHand = other.gameObject.transform.parent.GetComponent<Hand>();
                if (!theHand)
                {
                    TurnIn(other.gameObject);
                    other.gameObject.SetActive(false);
                }
            }

        }
    }
}
