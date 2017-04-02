using UnityEngine;
using System.Collections;
public class BaseObjCombination : MonoBehaviour

{

    // Use this for initialization
    
    public GameObject spawn;
    public GameObject add;
    public GameObject character;

    
    // Update is called once per frame
    void Update()
    {
    }

    //void OnAwake()
    //{
    //    full.SetActive(false);
    //}

    void OnTriggerEnter(Collider col)
    {
        print("collison happened");

        //  && gameObject.GetComponent<BaseObjCombination>().gameObject.activeInHierarchy
        if (col.name == add.name)
        {
            print("please?");
            Instantiate(spawn);
            Vector3 trans = character.transform.position;
            spawn.transform.position = trans;
            Destroy(add);
            Destroy(gameObject.GetComponent<BaseObjCombination>().gameObject);
            
        }
        //attachment1.transform.parent = transform1.transform;
        ////attachment1.GetComponent<Rigidbody>().isKinematic = true;

        //if (col == attachment1.GetComponent<Collider>())
        //{
        //    print("collison ONE");

        //    attachment1.transform.parent = transform1.transform;
        //}
        //else if (col == attachment2.GetComponent<Collider>())
        //{
        //    print("collison TWO");

        //    attachment2.transform.parent = transform2.transform;
        //}
    }
}
