using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeingHandled : MonoBehaviour
{
    public bool handled = false;
    [SerializeField]
    float maxTimer = 10f;
    float timer = 0;
    ForgeHeat FH;
    MeshRenderer MR;
    [SerializeField]
    Material StoneMat;
    OreToPommel thing;

    void Start()
    {
        thing = gameObject.GetComponent<OreToPommel>();
        FH = this.gameObject.GetComponent<ForgeHeat>();
        MR = this.gameObject.GetComponent<MeshRenderer>();
    }
	// Update is called once per frame
	void FixedUpdate ()
    {
		if(timer > 0 && handled)
        {
            timer -= Time.fixedDeltaTime;
            if(timer <= 0)
            {
                handled = false;
                FH.Heated = false;
                MR.material.color = StoneMat.color;
                thing.ResetShape();
            }
        }
	}

    public void HandleIt()
    {
        handled = true;
        timer = 0;
    }

    public void SetTime()
    {
        timer = maxTimer;
    }
}
