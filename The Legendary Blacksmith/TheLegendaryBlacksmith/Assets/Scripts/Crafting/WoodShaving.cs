using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class WoodShaving : MonoBehaviour
{

    //up is zero for this map



    [SerializeField]
    WoodGrabbyThing woodHolder;
    public bool CanDoTheThing = false;
    [SerializeField]
    GameObject shaving;
    [SerializeField]
    GameObject spawner;
    [SerializeField]
    GameObject prefab;
    float baseNum = 0;
    [SerializeField]
    float shaveSize = 0.1f;
    [SerializeField]
    CircularDrive crank;
    public bool doTheThing = false;
    [SerializeField]
    LinearMapping shaverPos;
    GameObject trash;

    [SerializeField]
    GameObject shaft;
    [SerializeField]
    GameObject handle;
    [SerializeField]
    GameObject lerpLog;
    Vector3 lerpStart;
    [SerializeField]
    GameObject model1;
    public bool phase1 = false;
    public bool phase2 = false;
    float shaveLerp = 0;
    [SerializeField]
    float shrinkScale = 0.001f;
    Vector3 temporaryScale;
    [SerializeField]
    GameObject arrowSpawnLoc;
    public bool first = false;

    [SerializeField]
    GameObject handleObj;
    [SerializeField]
    GameObject shaftObj;
    public GameObject thing;
    [SerializeField]
    GameObject holdTheHandle;
    public Vector3 handlePos;

    [SerializeField]
    GameObject Starter;
    [SerializeField]
    GameObject Ender;
    [SerializeField]
    LinearMapping LinMap;

    [SerializeField]
    bool animating = false;


    // Use this for initialization
    void Start()
    {
        MakeNewShaving();
        lerpStart = lerpLog.transform.localScale;
        
    }



    void Update()
    {
        if (CanDoTheThing && doTheThing && shaverPos.value == 1)
        {
            GrowShaving();

        }
        transform.position = Vector3.Lerp(Starter.transform.position, Ender.transform.position, LinMap.value);
    }

    public void GrowShaving()
    {
        if (thing != null && thing.gameObject.transform.position != handlePos)
        {
            ResetShaving();
        }
        if (crank.outAngle > baseNum)
        {
            shaving.transform.localScale = new Vector3(shaving.transform.localScale.x + ((crank.outAngle - baseNum) * shaveSize), shaving.transform.localScale.y, shaving.transform.localScale.z);
            shaving.transform.localPosition = new Vector3(shaving.transform.localPosition.x - ((crank.outAngle - baseNum) * shaveSize), shaving.transform.localPosition.y, shaving.transform.localPosition.z);
            shaveLerp += ((crank.outAngle - baseNum) * shrinkScale);
            ShaveLog();
        }
        baseNum = crank.outAngle;

    }

    public void ResetShaving()
    {
        if (thing != null && thing.gameObject.transform.position != handlePos)
        {
            Debug.Log("handlemoved");
            shaveLerp = 0;
            phase1 = false;
            phase2 = false;
            first = false;
            thing = null;
            CanDoTheThing = false;
            woodHolder.accepting = true;
        }
        if (shaving.transform.localScale != new Vector3(1, 1, 1))
        {

            trash = shaving;
            MakeNewShaving();
            trash.GetComponent<FixLevelBlockingMesh>().DoUpdate = false;
            trash.transform.parent = null;
            trash.GetComponent<Rigidbody>().isKinematic = false;
            trash.GetComponent<Rigidbody>().useGravity = true;
            trash.GetComponent<BoxCollider>().enabled = true;
            trash.GetComponent<ShavingSelfDestruct>().doIt = true; 
        }
        crank.outAngle = 0;
        baseNum = 0;
    }

    public void ActivateShaving()
    {
        doTheThing = true;
        crank.outAngle = 0;
        baseNum = 0;
    }

    public void DeactivateShaving()
    {
        doTheThing = false;
    }

    public void ShaveWood()
    {
        CanDoTheThing = true;
        crank.outAngle = 0;
        baseNum = 0;
    }
    public void StopShavingWood()
    {
        CanDoTheThing = false;
    }
    public void MakeNewShaving()
    {
        shaving = Instantiate(prefab, spawner.transform.position, spawner.transform.rotation);
        shaving.transform.parent = spawner.transform;
        shaving.name = "Shaving";
        shaving.transform.localScale = new Vector3(1, 1, 1);
    }
    public void PossibleResetShaving()
    {
        if (shaverPos.value != 1)
        {
            ResetShaving();
        }
    }
    public void ShaveLog()
    {
        if (!first)
        {
            model1.SetActive(false);
            lerpLog.SetActive(true);
            lerpLog.transform.localScale = lerpStart;
            phase1 = true;
            first = true;
        }
        if (phase1)
        {

            lerpLog.transform.localScale = Vector3.Lerp(lerpStart, handle.transform.localScale, shaveLerp);
            if (shaveLerp >= 0.99)
            {
                phase1 = false;
                phase2 = true;

                if (DialogueManager.dialogueInstance)
                    DialogueManager.dialogueInstance.GrabWoodShaver();
                
                if (!animating)
                {
                    lerpLog.transform.localScale = lerpStart;

                    thing = Instantiate(handleObj, handle.transform.position, handle.transform.rotation);
                    thing.name = handleObj.name;
                    thing.transform.parent = holdTheHandle.transform;
                    lerpLog.SetActive(false);
                    temporaryScale = thing.transform.localScale;
                    handlePos = thing.transform.position;
                }

                shaveLerp = 0;
                ResetShaving();
            }
        }
        if (phase2)
        {
            if (!animating)
            {
                thing.transform.localScale = Vector3.Lerp(temporaryScale, shaft.transform.localScale, shaveLerp);
                if (shaveLerp >= 0.99)
                {
                    phase2 = false;
                    CanDoTheThing = false;
                    shaveLerp = 0;

                    if (thing.transform.parent)
                    {
                        thing.transform.parent = null;
                    }
                    thing.gameObject.SetActive(false);
                    //Destroy(thing.gameObject);

                    thing = Instantiate(shaftObj, arrowSpawnLoc.transform.position, arrowSpawnLoc.transform.rotation);
                    thing.SetActive(true);

                    lerpLog.transform.localScale = lerpStart;
                    lerpLog.SetActive(false);
                    first = false;
                    thing = null;
                    woodHolder.accepting = true;

                    ResetShaving();
                }
            }
        }
    }
    public void CheckTheHandle()
    {
        if (thing != null)
        {
            if (thing.GetComponent<MeshCollider>())
            {
                if (shaverPos.value == 1)
                {
                    thing.GetComponent<MeshCollider>().enabled = false;
                    if (thing.GetComponentInChildren<BoxCollider>())
                        thing.GetComponentInChildren<BoxCollider>().enabled = false;
                }
                else
                {
                    thing.GetComponent<MeshCollider>().enabled = true;
                    if (thing.GetComponentInChildren<BoxCollider>())
                        thing.GetComponentInChildren<BoxCollider>().enabled = true;
                }
            }
        }
    }

    public void PutHandleBack(GameObject entity)
    {
        first = true;
        phase1 = false;
        phase2 = true;

        lerpLog.transform.localScale = lerpStart;
        thing = entity;
        thing.transform.position = handle.transform.position;
        thing.transform.rotation = handle.transform.rotation;
        thing.transform.parent = holdTheHandle.transform;
        thing.GetComponent<Rigidbody>().isKinematic = true;
        lerpLog.SetActive(false);
        temporaryScale = thing.transform.localScale;
        handlePos = thing.transform.position;

        shaveLerp = 0;
        ResetShaving();
    }

    public void CanDoThings()
    {
        CanDoTheThing = true;
    }
    public void DoIt()
    {
        baseNum = crank.outAngle;
        doTheThing = true;
    }
    public void DontDoIt()
    {
        doTheThing = false;
    }

    public void AnimPurge()
    {
        phase2 = false;
        shaveLerp = 0;
        
        //Destroy(thing.gameObject);

        
        

        lerpLog.transform.localScale = lerpStart;
        lerpLog.SetActive(false);
        first = false;
        thing = null;
        woodHolder.accepting = true;
    }




}