using UnityEngine;
using System.Collections;

public class UIInteractionController : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    private bool holding;
    private bool colliding;
    private Transform m_UIobject;
    private Transform UIparent;
    private Vector3 prevPosition;
    private Vector3 prevVelocity;

    public SteamVR_Controller.Device GetDevice()
    {
        return device;
    }

    private void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        holding = false;
        m_UIobject = null;
    }

    private void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        if (m_UIobject && device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (m_UIobject && m_UIobject.GetComponent<BagofHolding>() != null)
            {
                m_UIobject.GetComponent<BagofHolding>().Activate();
            }
            if (m_UIobject && m_UIobject.GetComponent<objectPlacement>() != null)
            {
                m_UIobject.GetComponent<objectPlacement>().ActivateText(false);
                //m_UIobject.GetComponent<objectPlacement>().SetObject(device);
                m_UIobject = null;
            }
            if (m_UIobject && m_UIobject.GetComponent<PhysicalSlider>() != null)
            {
                holding = true;
                m_UIobject.GetComponent<PhysicalSlider>().ReleaseTrackedObject();
                m_UIobject.GetComponent<PhysicalSlider>().SetTrackedObject(transform);
            }
        }

        if (holding && m_UIobject && device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (m_UIobject.GetComponent<PhysicalSlider>() != null)
                m_UIobject.GetComponent<PhysicalSlider>().ReleaseTrackedObject();
        }
        if (m_UIobject)
        {
            if (!m_UIobject.gameObject.activeInHierarchy)
            {
                if (m_UIobject.GetComponent<PhysicalSlider>())
                    m_UIobject.GetComponent<PhysicalSlider>().ReleaseTrackedObject();
                if(m_UIobject.GetComponent<objectPlacement>())
                {
                    m_UIobject.GetComponent<objectPlacement>().ActivateText(false);
                }
                ReleaseObject();
            }
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!m_UIobject && /*collision.gameObject.tag == "Selectable"*/ collision.gameObject.layer == 5)
        {
                device.TriggerHapticPulse(1000);
                string name = collision.gameObject.name;
                m_UIobject = collision.gameObject.transform;
            if (m_UIobject.GetComponent<objectPlacement>())
                m_UIobject.GetComponent<objectPlacement>().ActivateText(true, m_UIobject.GetComponent<objectPlacement>().prefab.GetComponent<StructurePlacing>().RealObject.name);
        }
    }
    //private void OnTriggerStay(Collider collision)
    //{
    //    if (collision.gameObject.layer == 5)
    //    {
    //        if (!m_object && collision.gameObject.tag == "Selectable" && !holding)
    //        {
    //            m_object = collision.gameObject.transform;
    //            holding = true;
    //            return;
    //        }
    //        if (!m_UIobject && !holding)
    //        {
    //            m_UIobject = collision.gameObject.transform;
    //            holding = true;
    //            return;
    //        }
    //    }
    //}
    private void OnTriggerExit(Collider collision)
    {
        if (!holding && collision.gameObject.layer == 5)
        {
            if (m_UIobject.GetComponent<objectPlacement>())
                m_UIobject.GetComponent<objectPlacement>().ActivateText(false);
            m_UIobject = null;
            //if (collision.gameObject == m_UIobject.gameObject && m_UIobject != null)
            //{
            //    if(m_UIobject.GetComponent<PhysicalSlider>())
            //    {
            //        if (m_UIobject.GetComponent<PhysicalSlider>().Sliding())
            //            return;
            //    }
            //    m_UIobject = null;
            //    holding = false;
            //}
        }
    }

    public void ReleaseObject()
    {
        holding = false;
        if (m_UIobject)
            m_UIobject = null;
    }

}
