using UnityEngine;
using System.Collections;

public class VendingMechanism : MonoBehaviour {
    public GameObject spawnArrow;
    public GameObject spawnSword;
    public GameObject spawnShield;
    public GameObject spawnArea;
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(spawnSword, spawnArea.transform.position, spawnArea.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Instantiate(spawnShield, spawnArea.transform.position, spawnArea.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Instantiate(spawnArrow, spawnArea.transform.position, spawnArea.transform.rotation);
        }
    }
}
