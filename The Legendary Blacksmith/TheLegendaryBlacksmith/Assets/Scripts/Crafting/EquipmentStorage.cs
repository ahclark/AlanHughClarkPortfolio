using UnityEngine;
using System.Collections;

public class EquipmentStorage : MonoBehaviour
{
    public GameObject Button;
    public GameObject Swordsman;
    public GameObject Tank;
    public GameObject Archer;
    public GameObject left;
    public GameObject right;

    public GameObject[] Prefabs;
    public bool Sword1;
    public bool Sword2;
    public bool Sword3;

    public bool Shield1;
    public bool Shield2;
    public bool Shield3;

    public bool Chestplate1;
    public bool Chestplate2;
    public bool Chestplate3;

    public bool Arrow1;
    public bool Arrow2;
    public bool Arrow3;

    public bool Bow1;
    public bool Bow2;
    public bool Bow3;

    public Transform Spawner;

    void Start()
    {
        Prefabs = new GameObject[2];
        Sword1 = false;
        Sword2 = false;
        Sword3 = false;

        Shield1 = false;
        Shield2 = false;
        Shield3 = false;

        Chestplate1 = false;
        Chestplate2 = false;
        Chestplate3 = false;

        Arrow1 = false;
        Arrow2 = false;
        Arrow3 = false;

        Bow1 = false;
        Bow2 = false;
        Bow3 = false;
    }

    ///Edited
    /// </Benjamin Ousley>
    /// Made the print function return a boolean to determine whether a unit actually printed
    /// Did this to make the chest close and the particles activate exclusively when a unit is actually printed
    /// Portions of this edit marked with 
    /// //Ben (Bool)
    /// content
    /// //
    /// </1/27/2017>
    public bool PrintOne()
    {
        //Ben(Bool)
        bool printed = false;
        if (Sword1 && Sword2)
        {
            Instantiate(Swordsman, Spawner.position, Spawner.rotation).name = Swordsman.name;

            //Ben(Bool)
            printed = true;
            //
        }
        if (Shield1 && Sword1)
        {
            Instantiate(Tank, Spawner.position, Spawner.rotation).name = Tank.name;

            //Ben(Bool)
            printed = true;
            //
        }
        if (Arrow1 && Bow1)
        {
            Instantiate(Archer, Spawner.position, Spawner.rotation).name = Archer.name;

            //Ben(Bool)
            printed = true;
            //
        }
        //

        left.GetComponent<WeaponStorageController>().Show();
        right.GetComponent<WeaponStorageController>().Show();
        left.GetComponent<WeaponStorageController>().ButtonPressed();
        right.GetComponent<WeaponStorageController>().ButtonPressed();

        Sword1 = false;
        Sword2 = false;
        Sword3 = false;

        Shield1 = false;
        Shield2 = false;
        Shield3 = false;

        Chestplate1 = false;
        Chestplate2 = false;
        Chestplate3 = false;

        Arrow1 = false;
        Arrow2 = false;
        Arrow3 = false;

        Bow1 = false;
        Bow2 = false;
        Bow3 = false;


        //Ben(Bool)
        return printed;
        //
    }

}

