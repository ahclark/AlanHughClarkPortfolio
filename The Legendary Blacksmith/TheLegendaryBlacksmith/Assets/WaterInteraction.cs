using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInteraction : MonoBehaviour {
    SwordDownLerp SDL;
    MeshRenderer MR;
    [SerializeField]
    Material Iron;
	// Use this for initialization
	void Start () {
        SDL = this.gameObject.GetComponent<SwordDownLerp>();
        MR = this.gameObject.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider col)
    {
        if (col.name == "Water" && SDL.hit)
        {
            if (col.gameObject.GetComponent<WaterSteam>())
                col.gameObject.GetComponent<WaterSteam>().ActivateSteam();
            MR.material = new Material(Iron);
            SDL.cool = true;
        }
    }
}
