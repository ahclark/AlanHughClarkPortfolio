using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReturn : MonoBehaviour {
    [SerializeField]
    GameObject boolSource;
    [SerializeField]
    GameObject Chest;
    [SerializeField]
    GameObject Shield;
    [SerializeField]
    GameObject Sword;
    [SerializeField]
    GameObject Arrow;
    [SerializeField]
    GameObject Bow;
    Vector3[] Locations;
    [SerializeField]
    Vector3 spawnLocation1 = new Vector3(1, 0, 0);
    [SerializeField]
    Vector3 spawnLocation2 = new Vector3(1, 0, 1);
    [SerializeField]
    GameObject LeftEquip;
    [SerializeField]
    GameObject RightEquip;
   
    private int SpawnCount;
    private WeaponStorageController WSC1;
    private WeaponStorageController WSC2;


    [SerializeField]
    InventoryManager InvMan;
    private EquipmentStorage ES;
    private ButtonPush BS;
    private float timer;
    // Use this for initialization
    void Start ()
    {
        ES = InvMan.Screens[InvMan.screen];
        BS = this.GetComponent<ButtonPush>();
        WSC1 = LeftEquip.GetComponent<WeaponStorageController>();
        WSC2 = RightEquip.GetComponent<WeaponStorageController>();
        timer = 0;
        SpawnCount = 0;
        Locations = new Vector3[3];
        Locations[0] = spawnLocation1;
        Locations[1] = spawnLocation2;
    }

    void Return()
    {
        ES = InvMan.Screens[InvMan.screen];
        Debug.Log("creating from screen " + InvMan.screen + 1);
        if (ES.Prefabs[0] != null)
        {
            Instantiate(ES.Prefabs[0], Locations[SpawnCount], this.transform.rotation).name = ES.Prefabs[0].name;
            SpawnCount++;
            ES.Prefabs[0] = null;
        }
        if (ES.Prefabs[1] != null)
        {
            Instantiate(ES.Prefabs[1], Locations[SpawnCount], this.transform.rotation).name = ES.Prefabs[1].name;
            SpawnCount++;
            ES.Prefabs[1] = null;
        }
       
        

        SpawnCount = 0;
        ES.Sword1 = false;
        ES.Sword2 = false;
        ES.Sword3 = false;
        
        ES.Shield1 = false;
        ES.Shield2 = false;
        ES.Shield3 = false;
        
        ES.Chestplate1 = false;
        ES.Chestplate2 = false;
        ES.Chestplate3 = false;
        
        ES.Arrow1 = false;
        ES.Arrow2 = false;
        ES.Arrow3 = false;
        
        ES.Bow1 = false;
        ES.Bow2 = false;
        ES.Bow3 = false;

        WSC1.ButtonPressed();
        WSC1.Show();
        
        WSC2.ButtonPressed();
        WSC2.Show();
       
       
        

    }
    private void OnTriggerExit(Collider entity)
    {
        if(BS.buttonOn == true)
        {
            Return();
        }
    }
}
