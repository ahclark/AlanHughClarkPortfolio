using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkingPortal : NetworkBehaviour {
    public GameObject spawnUnit;
	// Use this for initialization
	void Start () {
        NetworkServer.Spawn(gameObject);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer)
            return;
        if(other.tag == "SpawnObject")
        {
            CmdSpawn();
        }
    }
    [Command]
    void CmdSpawn()
    {

        NetworkServer.Spawn(Instantiate(spawnUnit, transform.position, transform.rotation));
    }
}
