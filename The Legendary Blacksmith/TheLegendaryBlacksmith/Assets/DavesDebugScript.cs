using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DavesDebugScript : MonoBehaviour {

    GameObject debugObject;
    public CircularDriveEvents cdrive;
    public Material[] materials;
    enum Colors { Green = 0, Yellow, Red}
    bool check = false;
	// Use this for initialization
	void Start () {
        debugObject = this.gameObject;







    }

    // Update is called once per frame
    void Update () {

	}

    void TestFunc(int i)
    {
        if (i > 0)
            TestGreen();
        else
            TestRed();
        print(i);
    }

    void TestGreen()
    {
        if(debugObject)
        {
            MeshRenderer tempRend = debugObject.GetComponent<MeshRenderer>();
            if(tempRend && materials != null)
            {
                tempRend.material = materials[(int)Colors.Green];
            }
        }
    }

    void TestRed()
    {
        if (debugObject)
        {
            MeshRenderer tempRend = debugObject.GetComponent<MeshRenderer>();
            if (tempRend && materials != null)
            {
                tempRend.material = materials[(int)Colors.Red];
            }
        }
    }

    void TestYellow()
    {
        if (debugObject)
        {
            MeshRenderer tempRend = debugObject.GetComponent<MeshRenderer>();
            if (tempRend && materials != null)
            {
                tempRend.material = materials[(int)Colors.Yellow];
            }
        }
    }

    void TestUpdate(float ratio)
    {
        if (debugObject)
        {
            MeshRenderer tempRend = debugObject.GetComponent<MeshRenderer>();
            if (tempRend && materials != null)
            {
                if(!tempRend.material)
                tempRend.material = new Material(materials[(int)Colors.Yellow]);
                tempRend.material.color = Color.Lerp(Color.black, Color.white, ratio);
            }
        }
    }
}
