using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    protected GameObject[] pooledObjects;
    public GameObject prefabObject;
    protected int currObject = 0;
    [SerializeField]
    protected int numObjects = 1;
	// Use this for initialization
	virtual protected void Start () {
        pooledObjects = new GameObject[numObjects];
        for (int i = 0; i < pooledObjects.Length; i++)
        {
            pooledObjects[i] = Instantiate(prefabObject, transform.position, transform.rotation);
            //Edited by Ben; needed for structure placement
            pooledObjects[i].name = prefabObject.name;
            pooledObjects[i].SetActive(false);
        }
	}
	
    public GameObject Spawn(Transform spawnPos)
    {
        if (pooledObjects[currObject].activeInHierarchy)
        {
            return null;
        }
        GameObject returnObj = pooledObjects[currObject];
        returnObj.SetActive(true);
        returnObj.transform.position = spawnPos.position;
        returnObj.transform.rotation = spawnPos.rotation;

        currObject++;
        if(currObject == pooledObjects.Length)
        {
            currObject = 0;

        }


        return returnObj;
    }

    

    




}
