using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectSelection : MonoBehaviour
{
    GameObject[] buttons;
    GameObject[] objects;
    public bool Struct;
    public GameObject[] walls;

    // Use this for initialization
    void Start()
    {
        if (Struct)
        {
            buttons = Resources.LoadAll<GameObject>("StructuresBag");
            objects = Resources.LoadAll<GameObject>("Structures");
        }
        else
        {
            buttons = Resources.LoadAll<GameObject>("ToolsBag");
            objects = Resources.LoadAll<GameObject>("Tools");
        }
        Vector3 tempPos = Vector3.zero, maxPos = Vector3.zero;
        for (int i = 0; i < walls.Length; ++i)
        {
            if (tempPos.x > walls[i].transform.position.x)
                tempPos.x = walls[i].transform.position.x + walls[i].transform.lossyScale.x * .5f;
            if (tempPos.y < walls[i].transform.position.y)
                tempPos.y = walls[i].transform.position.y - walls[i].transform.lossyScale.y * .5f;
            if (tempPos.z < walls[i].transform.position.z)
                tempPos.z = walls[i].transform.position.z + walls[i].transform.lossyScale.z * .5f;
            if (maxPos.x < walls[i].transform.position.x)
                maxPos.x = walls[i].transform.position.x - walls[i].transform.lossyScale.x;
            if (maxPos.y > walls[i].transform.position.y)
                maxPos.y = walls[i].transform.position.y + walls[i].transform.lossyScale.y;
        }
        //Vector3 tempPlacement = transform.InverseTransformPoint(tempPos);
        Vector3 tempPlacement = new Vector3(-0.05f, 0, 0);
        tempPos.x = (-0.05f);
        for (int i = 0; i < buttons.Length; ++i)
        {
            GameObject tempObject = GameObject.Instantiate<GameObject>(buttons[i]);
            tempObject.name = buttons[i].name;
            //tempObject.gameObject.AddComponent<objectPlacement>();
            tempObject.gameObject.GetComponent<objectPlacement>().prefab = objects[i];
            tempObject.gameObject.GetComponent<objectPlacement>().Struct = Struct;
            //if (Struct)
            //{
            //    tempObject.transform.localScale *= 1.0f;
            //}
            //else
            //    tempObject.transform.localScale *= .3f;
            tempObject.transform.SetParent(transform);
            tempObject.GetComponent<Rigidbody>().isKinematic = true;
            if (i == 0)
                tempPlacement.y -= tempObject.transform.localScale.y;
            float multiplier = 0.0f;
            if (Struct)
                multiplier = .25f;
            else
                multiplier = 1.0f;
            tempPlacement.x += (tempObject.transform.localScale.x * (2.0f * multiplier));
            if (transform.TransformPoint(tempPlacement).x + tempObject.transform.localScale.x > maxPos.x)
            {
                tempPlacement.x = tempPos.x + tempObject.transform.localScale.x * (2.0f * multiplier);
                tempPlacement.y -= tempObject.transform.localScale.y * (8.0f * multiplier);
            }
            tempObject.transform.localPosition = tempPlacement;
            tempPlacement.x += (tempObject.transform.localScale.x);
            if (transform.TransformPoint(tempPlacement).x > maxPos.x)
            {
                tempPlacement.x = tempPos.x;
                tempPlacement.y -= tempObject.transform.localScale.y * (8.0f * multiplier);
            }

        }
    }
}
