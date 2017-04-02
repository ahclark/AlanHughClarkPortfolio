using UnityEngine;
using System.Collections;

public class Combination : MonoBehaviour
{
    public GameObject Button;
    public GameObject Sword;
    public GameObject Shield;
    public GameObject Arrow;
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    private ButtonPush BP;
    public bool activates;

    public bool Sword_Blade;
    public bool Sword_Hilt;
    public bool Shield_Spike;
    public bool Shield_Handle;
    public bool Shield_Base;
    public bool Arrow_Head;
    public bool Arrow_Shaft;
    public bool Arrow_Feathers;
    private bool pressed = false;

    public Transform Spawner;

    void Start ()
    {
        BP = Button.GetComponent<ButtonPush>();

        Sword_Blade = false;
        Sword_Hilt = false;
        Shield_Spike = false;
        Shield_Handle = false;
        Shield_Base = false;
        Arrow_Head = false;
        Arrow_Shaft = false;
        Arrow_Feathers = false;
    }

    private void Update()
    {
        if (BP.buttonOn)
        {
            activates = true;
            Debug.Log("DID IT");
            if (Sword_Blade && Sword_Hilt)
            {
                Instantiate(Sword, Spawner.position, Spawner.rotation).name = Sword.name;
            }
            if (Shield_Spike && Shield_Handle && Shield_Base)
                Instantiate(Shield, Spawner.position, Spawner.rotation).name = Shield.name;
            if (Arrow_Head && Arrow_Shaft && Arrow_Feathers)
                Instantiate(Arrow, Spawner.position, Spawner.rotation).name = Arrow.name;

            slot1.GetComponent<SlotController>().Show();
            slot2.GetComponent<SlotController>().Show();
            slot3.GetComponent<SlotController>().Show();
            slot1.GetComponent<SlotController>().ButtonPressed();
            slot2.GetComponent<SlotController>().ButtonPressed();
            slot3.GetComponent<SlotController>().ButtonPressed();
            Sword_Blade = false;
            Sword_Hilt = false;
            Shield_Spike = false;
            Shield_Handle = false;
            Shield_Base = false;
            Arrow_Head = false;
            Arrow_Shaft = false;
            Arrow_Feathers = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Head")
        {
            pressed = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Head")
        {
            pressed = false;
        }
    }
}
