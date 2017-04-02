using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwordDownLerp : MonoBehaviour {
 
    [SerializeField]
    float lerpTimer;
    [SerializeField]
    Color malleable;
    [SerializeField]
    Color brittleColor;
    [SerializeField]
    Color barrelReady;
    [SerializeField]
    Color dullOrange;
    [SerializeField]
    Material malleableMat;
    [SerializeField]
    Material brittleMat;
    [SerializeField]
    ForgeHeat FH;
    Material lerpinMat;
    [SerializeField]
    Material BarrelReadyMat;
    [SerializeField]
    MeshRenderer MR;
    [SerializeField]
    Color check;
    int lerpTime;
    public bool startLerpin;
    float multVal;
    [SerializeField]
    float speed;
    public bool brittle;
    public bool hit;
    public bool cool;
    // Use this for initialization
    public void Start () {
        startLerpin = false;
        malleable = malleableMat.color;
        brittleColor = brittleMat.color;
        barrelReady = BarrelReadyMat.color;
        lerpTime = Random.Range(0, 10);
        MR.material = new Material(malleableMat);
        MR.material.color = FH.StartMat.color;
        brittle = false;
        hit = false;
        cool = false;
        lerpTimer = 0;

        switch (lerpTime)
        {
            case 0:
                multVal = 0.1f * speed;
                break;
            case 1:
                multVal = 0.15f * speed;
                break;
            case 2:
                multVal = 0.2f * speed;
                break;
            case 3:
                multVal = 0.25f * speed;
                break;
            case 4:
                multVal = 0.3f * speed;
                break;
            case 5:
                multVal = 0.35f * speed;
                break;
            case 6:
                multVal = 0.4f * speed;
                break;
            case 7:
                multVal = 0.45f * speed;
                break;
            case 8:
                multVal = 0.5f * speed;
                break;
            case 9:
                multVal = 0.55f * speed;
                break;
            default:
                break;
        }
    }
	
    void FixedUpdate()
    {
        if (startLerpin && hit == false)
        {
            lerpTimer += Time.fixedDeltaTime * multVal;
            MR.material.color = Color.Lerp(malleable, brittleColor, lerpTimer);
        }
        else if (hit == true && cool == false)
        {
            MR.material.color = barrelReady;
        }
        if (lerpTimer >= 1)
        {
            startLerpin = false;
            brittle = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.name == "Forge" && FH.Heated)
        {
            startLerpin = true;
            check = MR.material.color;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Forge" && brittle && !hit && !cool)
        {
            Start();
            MR.material.color = malleable;
        }
    }
}
