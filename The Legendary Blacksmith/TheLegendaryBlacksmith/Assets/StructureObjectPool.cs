using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureObjectPool : ObjectPooler
{

    // Use this for initialization
    protected override void Start()
    {
        pooledObjects = new GameObject[numObjects];
        for (int i = 0; i < pooledObjects.Length; i++)
        {
            GameObject existingObject = GameObject.Find(prefabObject.name);
            if (existingObject)
            {
                pooledObjects[i] = existingObject;
                pooledObjects[i].SetActive(true);
            }
            else
            {
                pooledObjects[i] = Instantiate(prefabObject, transform.position, transform.rotation);
                pooledObjects[i].SetActive(false);
            }
            pooledObjects[i].name = prefabObject.name;
        }
    }

    public void DeSpawnCurrent()
    {
        GameObject returnObj = pooledObjects[currObject];
        returnObj.SetActive(false);
    }
}
