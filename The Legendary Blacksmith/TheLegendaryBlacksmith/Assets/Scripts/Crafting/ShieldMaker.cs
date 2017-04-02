using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShieldMaker : MonoBehaviour
{
    [SerializeField]
    bool ready = false;
    [SerializeField]
    bool morph = false;
    Vector3 startScale;
    Vector3 endScale;
    float timer = 0;
    [SerializeField]
    float speed = 1;
    [SerializeField]
    GameObject shield;
    [SerializeField]
    GameObject position;

	// Use this for initialization
	void Start ()
    {
        startScale = gameObject.transform.lossyScale;
        endScale = new Vector3(gameObject.transform.localScale.x * 2, gameObject.transform.localScale.y / 2, gameObject.transform.localScale.z * 2);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (morph)
        {
            if (timer < 1)
            {
                timer += Time.deltaTime * speed;
                transform.localScale = Vector3.Lerp(startScale, endScale, timer);
            }
            else
            {
                morph = false;
                GameObject thing = Instantiate(shield, position.transform.position, position.transform.localRotation);
                thing.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                thing.transform.position = position.transform.position;
                thing.name = shield.name;
                //thing.transform.localRotation = position.transform.localRotation;
                if (transform.parent)
                {
                    transform.parent.GetComponent<Hand>().DetachObject(gameObject);
                }
                gameObject.transform.parent = null;
                Destroy(gameObject);
            }
        }
		
	}

    public void ItsMorphinTime()
    {
        morph = true;
        if (transform.parent)
        {
            transform.parent.GetComponent<Hand>().DetachObject(gameObject);
        }
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<MeshCollider>().enabled = false;
        morph = true;
    }

    public void Abort()
    {
        GetComponent<MeshCollider>().enabled = true;
        morph = false;
    }

    public void RimJob()
    {
        ready = true;
    }
}
