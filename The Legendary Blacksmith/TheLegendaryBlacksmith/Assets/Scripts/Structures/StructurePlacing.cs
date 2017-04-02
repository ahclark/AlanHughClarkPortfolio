using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class StructurePlacing : MonoBehaviour
{
    public enum Placement
    {
        None = 0x00,
        Floor = 0x01,
        LowerShelf = 0x02,
        UpperShelf = 0x04,
        All = 0x08
    }

    public GameObject RealObject;

    [SerializeField]
    [BitMask(typeof(Placement))]
    Placement placementTag;

    Collider coll;
    Vector3 rayPoint = Vector3.zero;
    float[] bounds;
    GameObject floor;
    Renderer mColor;
    Color mGreen = Color.green;
    Color mRed = Color.red;
    public Hand device;
    Transform deviceTrans = null;
    Vector3 startPos;
    SteamVR_LaserPointer laser;
    [SerializeField]
    LayerMask IgnoredLayer;
    int layermask;
    StructureObjectPool RealObjectPool;
    // Use this for initialization
    void Start()
    {
        coll = gameObject.GetComponent<Collider>();
        floor = GameObject.FindGameObjectWithTag("Floor");
        Collider floorColl = floor.GetComponent<Collider>();
        bounds = new float[4] { floorColl.bounds.min.x, floorColl.bounds.max.x, floorColl.bounds.min.z, floorColl.bounds.max.z };
        mGreen.a = 0.2f;
        mRed.a = 0.2f;
        mColor = transform.gameObject.GetComponent<Renderer>();
        mColor.material.color = mGreen;
        if (transform.childCount > 0)
        {
            Renderer[] rendChildren;
            rendChildren = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rendChildren)
            {
                rend.material = mColor.material;
            }
        }
        deviceTrans = device.transform;
        startPos = deviceTrans.position;
        laser = deviceTrans.GetComponent<SteamVR_LaserPointer>();
        if (laser)
            laser.active = true;
    }
    private void OnEnable()
    {
        if (laser)
            laser.active = true;
    }
    private void OnDisable()
    {
        if (laser)
            laser.active = false;
    }
    // Update is called once per frame
    void Update()
    {
        startPos = deviceTrans.position;
        // Perform a raycast starting from the controller's position and going 1000 meters
        // out in the forward direction of the controller to see if we hit something
        RaycastHit controllerHit;
        if (Physics.Raycast(startPos, deviceTrans.forward, out controllerHit, 1000, IgnoredLayer.value) && controllerHit.transform.name != transform.name
            && controllerHit.transform.tag == placementTag.ToString())
        {
            rayPoint = controllerHit.point;
            rayPoint.x = Mathf.Clamp(rayPoint.x, bounds[0], bounds[1]);
            rayPoint.z = Mathf.Clamp(rayPoint.z, bounds[2], bounds[3]);
            transform.position = Vector3.Lerp(transform.position, rayPoint, Time.deltaTime * 10.0f);
        }
        if (controllerHit.transform && controllerHit.transform.tag == placementTag.ToString())
        {
            GameObject obj = controllerHit.transform.gameObject;
            Vector3 MyNormal = controllerHit.normal;
            //MyNormal = controllerHit.transform.TransformDirection(MyNormal);

            if (MyNormal == controllerHit.transform.up)
            {
                Vector3 temp = transform.position;
                float dist = temp.y - obj.transform.position.y;
                dist -= (transform.lossyScale.y / 4 + obj.transform.lossyScale.y / 4);
                temp.y -= dist;
                transform.position = temp;
            }
            else if (MyNormal == -controllerHit.transform.up)
            {
                Vector3 temp = transform.position;
                float dist = temp.y - obj.transform.position.y;
                dist += (transform.lossyScale.y / 4 + obj.transform.lossyScale.y / 4);
                temp.y -= dist;
                transform.position = temp;
            }
            else if (MyNormal == controllerHit.transform.forward)
            {
                Vector3 temp = transform.position;
                float dist = temp.z - obj.transform.position.z;
                dist -= (transform.lossyScale.z / 4 + obj.transform.lossyScale.z / 4);
                temp.z -= dist;
                transform.position = temp;
            }
            else if (MyNormal == -controllerHit.transform.forward)
            {
                Vector3 temp = transform.position;
                float dist = temp.z - obj.transform.position.z;
                dist += (transform.lossyScale.z / 4 + obj.transform.lossyScale.z / 4);
                temp.z -= dist;
                transform.position = temp;
            }
            else if (MyNormal == controllerHit.transform.right)
            {
                Vector3 temp = transform.position;
                float dist = temp.x - obj.transform.position.x;
                dist -= (transform.lossyScale.x / 4 + obj.transform.lossyScale.x / 4);
                temp.x -= dist;
                transform.position = temp;
            }
            else if (MyNormal == -controllerHit.transform.right)
            {
                Vector3 temp = transform.position;
                float dist = temp.x - obj.transform.position.x;
                dist += (transform.lossyScale.x / 4 + obj.transform.lossyScale.x / 4);
                temp.x -= dist;
                transform.position = temp;
            }
            //if (controllerHit.transform.tag == placementTag.ToString() && controllerHit.transform.tag == "Floor")
            //{
            //    Vector3 temp = transform.position;
            //    float dist = temp.y - floor.transform.position.y;
            //    dist -= (transform.lossyScale.y/* / 2 + floor.transform.lossyScale.y / 2);
            //    temp.y -= dist;
            //    transform.position = temp;
            //}
        }
        Vector3 tempRay = deviceTrans.position;
        tempRay.y = transform.position.y;
        transform.LookAt(tempRay);
        if (device.GetStandardInteractionButtonDown() && mColor.material.color != mRed)
        {
            GameObject realObject = RealObjectPool.Spawn(transform);
            realObject.name = RealObject.name;

            gameObject.SetActive(false);

        }
        if (device.controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != placementTag.ToString() && other.transform.tag != "Terrain"/* && collision.gameObject.layer != IgnoredLayer*/)
        {
            mColor.material.color = mRed;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag != placementTag.ToString() && other.transform.tag != "Terrain"/* && other.gameObject.layer != IgnoredLayer*/)
        {
            mColor.material.color = mRed;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Terrain" && other.transform.tag != placementTag.ToString())
            mColor.material.color = mGreen;
    }

    public void SetObject(StructureObjectPool pool)
    {
        RealObjectPool = pool;
        RealObjectPool.DeSpawnCurrent();
    }

    public class BitMaskAttribute : PropertyAttribute
    {
        public System.Type propType;
        public BitMaskAttribute(System.Type aType)
        {
            propType = aType;
        }
    }
}
