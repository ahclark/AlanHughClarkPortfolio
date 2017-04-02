using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSpawn : MonoBehaviour {
    [SerializeField]
    GameObject EquipUnit;
    [SerializeField]
    GameObject StartPos;
    [SerializeField]
    MoveUp MU;

	// Use this for initialization
	void Start () {
        UnitSpawn();
	}
	
	// Update is called once per frame
    public void UnitSpawn()
    {
        EquipUnit.transform.position = StartPos.transform.position;
        EquipUnit.SetActive(true);
        MU.Moving = true;
    }
}
