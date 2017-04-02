using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class WeaponStorageController : MonoBehaviour
{
    public GameObject button;
    private GameObject m_object = null;
    public GameObject PrefabSet = null;
    [SerializeField]
    GameObject[] Prefabs;
    private Object Representative = null;
    [SerializeField]
    InventoryManager InvMan;
    private EquipmentStorage ES;
    [SerializeField]
    int index;

    enum gear {Sword, Shield, Chestplate, Arrow, Bow };
    void Start()
    {
        ES = InvMan.Screens[InvMan.screen];

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Equippable" && m_object == null)
        {
            ES = InvMan.Screens[InvMan.screen];
            m_object = col.gameObject;

            switch (m_object.name)
            {
                case "FullSword":
                    {
                        PrefabSet = Prefabs[(int)gear.Sword];
                        if (ES.Sword1 == false)
                            ES.Sword1 = true;
                        else if (ES.Sword2 == false)
                            ES.Sword2 = true;
                        else if (ES.Sword3 == false)
                            ES.Sword3 = true;
                        break;
                    }
                case "FullShield 1":
                    {
                        PrefabSet = Prefabs[(int)gear.Shield];

                        if (ES.Shield1 == false)
                            ES.Shield1 = true;
                        else if (ES.Shield2 == false)
                            ES.Shield2 = true;
                        else if (ES.Shield3 == false)
                            ES.Shield3 = true;
                        break;
                    }
                case "FullArrow":
                        PrefabSet = Prefabs[(int)gear.Arrow];

                        if (ES.Arrow1 == false)
                            ES.Arrow1 = true;
                        else if (ES.Arrow2 == false)
                            ES.Arrow2 = true;
                        else if (ES.Arrow3 == false)
                            ES.Arrow3 = true;
                        break;
                case "FullBow":
                    {
                        PrefabSet = Prefabs[(int)gear.Bow];

                        if (ES.Bow1 == false)
                            ES.Bow1 = true;
                        else if (ES.Bow2 == false)
                            ES.Bow2 = true;
                        else if (ES.Bow3 == false)
                            ES.Bow3 = true;
                        break;
                    }
                case "FullChest":
                    {
                        PrefabSet = Prefabs[(int)gear.Chestplate];

                        if (ES.Chestplate1 == false)
                            ES.Chestplate1 = true;
                        else if (ES.Chestplate2 == false)
                            ES.Chestplate2 = true;
                        else if (ES.Chestplate3 == false)
                            ES.Chestplate3 = true;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            ES.Prefabs[index] = PrefabSet;

            Representative = Instantiate(ES.Prefabs[index].GetComponent<CombineObject>().Representative, this.transform.position, this.transform.rotation, this.transform);
            Hide();

            if (m_object.transform.parent)
            {
                m_object.transform.parent.gameObject.GetComponent<Hand>().DetachObject(m_object);
            }
            m_object.SetActive(false);
            Destroy(m_object);

        }
    }

    public void Show()
    {
        GetComponent<MeshRenderer>().enabled = true; //mesh of the purp sphere
        GetComponent<SphereCollider>().enabled = true; //collider of the purp sphere
    }
    void Hide()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
    }
    public void ButtonPressed()
    {
        ES = InvMan.Screens[InvMan.screen];
        if (Representative)
        {
            Destroy(Representative);
            m_object = null;
            Representative = null;
            ES.Prefabs[index] = null;
            PrefabSet = null;
        }
    }

    public void Render()
    {
        ES = InvMan.Screens[InvMan.screen];
        if (Representative)
        {

            Destroy(Representative);
            m_object = null;
            Representative = null;
            PrefabSet = null;
        }
        if (ES.Prefabs[index])
        {
            Representative = Instantiate(ES.Prefabs[index].GetComponent<CombineObject>().Representative, this.transform.position, this.transform.rotation, this.transform);
            Hide();
        }
        else
        {
            Show();
        }
     }
}

