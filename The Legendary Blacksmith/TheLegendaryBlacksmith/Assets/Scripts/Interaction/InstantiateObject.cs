using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{

    [SerializeField]
    string spawnName;
    [SerializeField]
    GameObject thing;

    [SerializeField]
    bool normal = false;

    [SerializeField]
    GameObject spawnPosition;
    [SerializeField]
    bool positionLocal = false;
    [SerializeField]
    GameObject spawnScale;
    [SerializeField]
    bool scaleLocal = false;
    [SerializeField]
    GameObject spawnRotation;
    [SerializeField]
    bool rotationLocal = false;
    [SerializeField]
    bool childOfParent = false;

    private void Start()
    {
        if (!spawnPosition)
            spawnPosition = thing;
        if (!spawnScale)
            spawnScale = thing;
        if (!spawnRotation)
            spawnRotation = thing;
        if (spawnName == "")
            spawnName = thing.name;
    }

	public void MakeTheThing()
    {
        GameObject madeIt;
        if (normal)
        {
            madeIt = Instantiate(thing);
            madeIt.name = spawnName;
        }
        else if (positionLocal && rotationLocal)
        {
            madeIt = Instantiate(thing, spawnPosition.transform.localPosition, spawnRotation.transform.localRotation);
        }
        else if (positionLocal && !rotationLocal)
        {
            madeIt = Instantiate(thing, spawnPosition.transform.localPosition, spawnRotation.transform.rotation);
        }
        else if (!positionLocal && rotationLocal)
        {
            madeIt = Instantiate(thing, spawnPosition.transform.position, spawnRotation.transform.localRotation);
        }
        else
        {
            madeIt = Instantiate(thing, spawnPosition.transform.position, spawnRotation.transform.rotation);
        }

        if (!scaleLocal)
            madeIt.transform.localScale = spawnScale.transform.lossyScale;
        else
            madeIt.transform.localScale = spawnScale.transform.localScale;
        madeIt.name = spawnName;
        if (childOfParent)
        {
            madeIt.transform.parent = gameObject.transform.parent;
        }
    }
}
