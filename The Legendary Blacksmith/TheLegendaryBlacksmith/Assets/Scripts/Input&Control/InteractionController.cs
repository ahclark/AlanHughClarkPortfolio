using UnityEngine;
using System.Collections;

public class InteractionController : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    private bool holding;
    private bool colliding;
    private Transform m_object;
    private Vector3 prevPosition;
    private Vector3 prevVelocity;

    public Material Default;
    public Material Prompt;
    GameObject forge;
    Animator anim;
    [SerializeField]
    GameObject hand;

    public SteamVR_Controller.Device GetDevice()
    {
        return device;
    }

    private void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);
        anim = hand.GetComponent<Animator>();
        holding = false;
        m_object = null;
        forge = GameObject.Find("Forge");
        SteamVR_LaserPointer laser = GetComponent<SteamVR_LaserPointer>();
        laser.active = false;

    }

    private void Update()
    {
        if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            anim.SetTrigger("TriggerDown");
            anim.SetBool("TriggerPressed", true);
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            anim.SetTrigger("TriggerUp");
            anim.SetBool("TriggerPressed", false);
        }
        if (!device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            anim.SetTrigger("TriggerUp");
            anim.SetBool("TriggerPressed", false);
        }

        if (!holding && m_object && device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && m_object.gameObject.layer == 8)
        {
            device.TriggerHapticPulse(1000);

            holding = true;
            while (m_object.parent != null)
            {
                if (m_object.parent.tag == "Controller")
                {
                    m_object.parent.GetComponent<InteractionController>().DropObject(m_object);
                    break;
                }
                m_object = m_object.parent;
            }
            m_object.parent = gameObject.transform;

            PickupObject(m_object);

            //if(m_object.tag == "ForgeItem")
            //{
            //    if(forge && forge.GetComponent<ForgeController>())
            //    {
            //        forge.GetComponent<ForgeController>().ActivateTutorial();
            //    }
            //}
        }
        else if (holding)
        {
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                holding = false;
                //m_object = null;

                DropObject(m_object);
            }
            if (m_object)
            {
                prevPosition = m_object.transform.position;
                Rigidbody rigid = m_object.GetComponent<Rigidbody>();
                if (rigid)
                    prevVelocity = rigid.velocity;
            }
        }

        if (!m_object)
            this.transform.GetChild(1).GetComponent<Renderer>().material = Default;
    }

    private void OnTriggerEnter(Collider collision)
    {
        //device.TriggerHapticPulse(3999);
        Vibrate(3999);

        if (!holding && /*collision.gameObject.tag == "Selectable"*/ collision.gameObject.layer == 8 && !m_object)
        {
            m_object = collision.gameObject.transform;
            //this.GetComponentInChildren<Renderer>().material = Prompt;
            this.transform.GetChild(1).GetComponent<Renderer>().material = Prompt;
            if (m_object.GetComponent<Renderer>())
                m_object.GetComponent<Renderer>().material.shader = Shader.Find("Toon/Lit Outline");
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (!holding && /*collision.gameObject.tag == "Selectable"*/ collision.gameObject.layer == 8 && !m_object)
        {
            m_object = collision.gameObject.transform;
            //this.GetComponentInChildren<Renderer>().material = Prompt;
            this.transform.GetChild(1).GetComponent<Renderer>().material = Prompt;
            if (m_object.GetComponent<Renderer>())
                m_object.GetComponent<Renderer>().material.shader = Shader.Find("Toon/Lit Outline");
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (m_object)
        {
            if (!holding && collision.gameObject == m_object.gameObject /*&& collision.gameObject.layer == 8*/)
            {
                if (m_object.GetComponent<Renderer>())
                    m_object.GetComponent<Renderer>().material.shader = Shader.Find("Toon/Lit");
                m_object = null;
                //this.GetComponentInChildren<Renderer>().material = Default;
                this.transform.GetChild(1).GetComponent<Renderer>().material = Default;
            }
        }
    }

    void PickupObject(Transform p_object)
    {
        p_object.GetComponent<Rigidbody>().useGravity = false;
        p_object.GetComponent<Rigidbody>().isKinematic = true;

        prevPosition = transform.position;
        prevVelocity = p_object.GetComponent<Rigidbody>().velocity;
    }

    void DropObject(Transform p_object)
    {
        if (p_object)
        {
            m_object = null;
            holding = false;

            p_object.parent = null;
            p_object.GetComponent<Rigidbody>().useGravity = true;
            p_object.GetComponent<Rigidbody>().isKinematic = false;

            Vector3 throwVector = p_object.transform.position - prevPosition;
            float speed = throwVector.magnitude / Time.deltaTime;
            Vector3 throwVelocity = speed * throwVector.normalized;
            p_object.GetComponent<Rigidbody>().velocity = throwVelocity;
        }
    }

    public void SetObject(Transform p_object)
    {
        m_object = p_object;
        holding = true;
        while (m_object.parent != null)
            m_object = m_object.parent;
        m_object.parent = gameObject.transform;
        m_object.localPosition = Vector3.zero;
        m_object.localRotation = Quaternion.identity;

        PickupObject(m_object);
    }

    public void Vibrate(ushort value)
    {
        device.TriggerHapticPulse(value);
    }
}
