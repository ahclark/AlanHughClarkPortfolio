using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintAllController : MonoBehaviour
{

    [SerializeField]
    GameObject[] SpawnLocs;

    [SerializeField]
    GameObject LeftEquip;

    [SerializeField]
    GameObject RightEquip;



    [SerializeField]
    WeaponStorageController[] WSCs;

    public EquipmentStorage Screen1;
    public EquipmentStorage Screen2;
    public EquipmentStorage Screen3;
    public EquipmentStorage Screen4;
    public EquipmentStorage Screen5;

    [SerializeField]
    GameObject Button;

    [SerializeField]
    GameObject Swordsman;

    [SerializeField]
    GameObject Tank;

    [SerializeField]
    GameObject Archer;

    ButtonPush BS;

    float timer;

    bool printed;

    // Use this for initialization
    void Start()
    {

        BS = Button.GetComponent<ButtonPush>();
        timer = 0;
        printed = false;
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
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
    public bool PrintAll()
    {
        //Ben (Bool)
        bool printingPass = false;
        //
        if (timer > 2)
        {
            timer = 0;
            //Ben (Bool)
            if (Print(Screen1, 0))
            {
                printingPass = true;
            }
            //

            //Ben (Bool)
            if (Print(Screen2, 1))
            {
                printingPass = true;
            }
            //

            //Ben (Bool)
            if (Print(Screen3, 2))
            {
                printingPass = true;
            }
            //

            //Ben (Bool)
            if (Print(Screen4, 3))
            {
                printingPass = true;
            }
            //

            //Ben (Bool)
            if (Print(Screen5, 4))
            {
                printingPass = true;
            }
            //
        }
        //Ben (Bool)
        return printingPass;
        //

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

    bool Print(EquipmentStorage info, int spawnIndex)
    {

        //Ben(Bool)
        bool returnBool = false;
        if (info.Arrow1 && info.Bow1)
        {
            Instantiate(Archer, SpawnLocs[spawnIndex].transform.position, SpawnLocs[spawnIndex].transform.rotation).name = Archer.name;
            printed = true;
        }
        else if (info.Shield1 && info.Sword1)
        {
            Instantiate(Tank, SpawnLocs[spawnIndex].transform.position, SpawnLocs[spawnIndex].transform.rotation).name = Tank.name;
            printed = true;
        }
        else if (info.Sword1 && info.Sword1)
        {
            Instantiate(Swordsman, SpawnLocs[spawnIndex].transform.position, SpawnLocs[spawnIndex].transform.rotation).name = Swordsman.name;
            printed = true;

        }
        returnBool = printed;
        if (printed)
        {
            Clear(info);
        }

        //Ben (Bool)
        return returnBool;
    }

    void Clear(EquipmentStorage info)
    {
        printed = false;
        for (int i = 0; i < 2; i++)
        {
            WSCs[i].ButtonPressed();
            WSCs[i].Render();
        }

        info.Sword1 = false;
        info.Sword2 = false;
        info.Sword3 = false;

        info.Shield1 = false;
        info.Shield2 = false;
        info.Shield3 = false;

        info.Chestplate1 = false;
        info.Chestplate2 = false;
        info.Chestplate3 = false;

        info.Arrow1 = false;
        info.Arrow2 = false;
        info.Arrow3 = false;

        info.Bow1 = false;
        info.Bow2 = false;
        info.Bow3 = false;

        info.Prefabs[0] = null;
        info.Prefabs[1] = null;


    }
}
