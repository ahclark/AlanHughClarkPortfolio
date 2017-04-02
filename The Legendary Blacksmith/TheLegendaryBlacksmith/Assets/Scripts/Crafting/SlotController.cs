using UnityEngine;
using System.Collections;

public class SlotController : MonoBehaviour
{
    public GameObject button;
    private GameObject Object = null;
    private Object Representative = null;

    void OnTriggerEnter (Collider col)
    {
        if (col.tag == "Combinable" && Object == null)
        {
            Object = col.gameObject;

            switch (Object.name)
            {
                case "Sword_Blade":
                    {
                        button.GetComponent<Combination>().Sword_Blade = true;
                        break;
                    }
                case "Sword_Hilt":
                    {
                        button.GetComponent<Combination>().Sword_Hilt = true;
                        break;
                    }
                case "Shield_Spike":
                    {
                        button.GetComponent<Combination>().Shield_Spike = true;
                        break;
                    }
                case "Shield_Handle":
                    {
                        button.GetComponent<Combination>().Shield_Handle = true;
                        break;
                    }
                case "Shield_Base":
                    {
                        button.GetComponent<Combination>().Shield_Base = true;
                        break;
                    }
                case "Arrow_Head":
                    {
                        button.GetComponent<Combination>().Arrow_Head = true;
                        break;
                    }
                case "Arrow_Shaft":
                    {
                        button.GetComponent<Combination>().Arrow_Shaft = true;
                        break;
                    }
                case "Arrow_Feathers":
                    {
                        button.GetComponent<Combination>().Arrow_Feathers = true;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            Representative = Instantiate(Object.GetComponent<CombineObject>().Representative, this.transform.position, this.transform.rotation, this.transform);
            Destroy(Object.gameObject);
            Hide();
        }
    }

    public void Show ()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }
    void Hide ()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
    }
    public void ButtonPressed ()
    {
        if (Representative)
        {
            Destroy(Representative);
            Object = null;
            Representative = null;
        }
    }
}
