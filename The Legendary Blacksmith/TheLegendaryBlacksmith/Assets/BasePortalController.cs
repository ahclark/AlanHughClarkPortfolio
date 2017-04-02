using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePortalController : MonoBehaviour {
    public GameObject BasePortal;

    public void TurnMeOn()
    {
        gameObject.SetActive(true);
        BasePortal.SetActive(true);

    }


}
