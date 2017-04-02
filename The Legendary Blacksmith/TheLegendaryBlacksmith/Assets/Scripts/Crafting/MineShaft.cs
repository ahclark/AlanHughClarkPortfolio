using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineShaft : MonoBehaviour
{
    [SerializeField]
    GameObject Ore;
    [SerializeField]
    GameObject[] Hunks;
    Animator cart;
    public bool Full = false;
    public bool inPlace = false;
    int hunkNum = 0;
    BeingHandled handler;

	// Use this for initialization
	void Start ()
    {
        cart = GetComponent<Animator>();
	}

    void SpawnOre()
    {
        Vector3 orePosition = Ore.transform.position;
        handler = Hunks[hunkNum].GetComponent<BeingHandled>();
        if (!handler.handled)
        {
            Hunks[hunkNum].transform.position = orePosition;
            Hunks[hunkNum].SetActive(true);
            
            AddNum();
        }
        else
        {
            AddNum();
            SpawnOre();
        }
    }

    public void DoAnimation()
    {
        cart.SetBool("On", true);
    }
    public void StopAnimation()
    {
        cart.SetBool("On", false);
    }
    void AddNum()
    {
        hunkNum++;
        if (hunkNum >= Hunks.Length)
        {
            hunkNum = 0;
        }
    }
}
