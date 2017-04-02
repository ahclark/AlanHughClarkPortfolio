using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GuillotineLog : MonoBehaviour
{
    [SerializeField]
    GameObject[] wedges;
    [SerializeField]
    int wedgeNum = 0;
    Vector3 currentPos;
    Vector3 targetPos;
    float travelLength;
    float timer = 0;
    float curDist = 0;
    public bool showtime = false;
    bool ready = false;
    [SerializeField]
    BoxCollider block;
    [SerializeField]
    float speed = 1;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (showtime)
        {
            timer += Time.deltaTime * speed;
            curDist = timer / travelLength;

            if (curDist < travelLength)
            {
                transform.localPosition = Vector3.Lerp(currentPos, targetPos, curDist);
            }
            if(wedgeNum == wedges.Length)
            {
                ResetPositions();
            }
        }
	}

    public void CutTheLog()
    {
        if (ready)
        {
            //if (wedgeNum > 0)
            //{
                
                GameObject newObject = Instantiate(wedges[wedgeNum], wedges[wedgeNum].transform.position, wedges[wedgeNum].transform.rotation);
                newObject.transform.localScale = wedges[wedgeNum].transform.lossyScale;
                newObject.name = "LogSlice";
            newObject.GetComponent<MeshCollider>().enabled = true;
            newObject.GetComponent<Rigidbody>().isKinematic = false;
            //}
            wedges[wedgeNum].SetActive(false);
            if (wedgeNum == wedges.Length - 1)
            {
                ResetPositions();
            }
            wedgeNum++;
            ready = false;
        }

    }

    public void ChangePositions()
    {
        if (showtime)
        {
            currentPos = gameObject.transform.localPosition;
            targetPos = new Vector3(0, wedgeNum + 1, 0);
            travelLength = Vector3.Distance(currentPos, targetPos);
            timer = 0;
            ready = true;
        }
    }

    public void ResetPositions()
    {
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        showtime = false;
        //ChangePositions();
        wedgeNum = 0;
        timer = 0;
        currentPos = transform.localPosition;
        targetPos = transform.localPosition;
        block.isTrigger = true;
    }

    public void NowYouSeeMe()
    {
        foreach (GameObject thing in wedges)
        {
            thing.SetActive(true);
            block.isTrigger = false;
        }
        showtime = true;
    }

    void OnTriggerEnter(Collider entity)
    {
        if (!showtime)
        {
            if (entity.tag == "Wood")
            {
                if (entity.transform.parent)
                {
                    entity.transform.parent.gameObject.GetComponent<Hand>().DetachObject(entity.gameObject);
                }
                entity.gameObject.SetActive(false);
                NowYouSeeMe();
            }
        }
    }
}
