using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LeverSnapper : MonoBehaviour
{
    public float m_LowerLock;
    public float m_UpperLock;
    public float m_SnapRate;

    [SerializeField]
    private bool m_held;
    private CircularDrive m_CircularDrive;
    private Vector3 m_VectorOfRotation;
    private LinearMapping m_LinearMapping;

    private void Start()
    {
        m_CircularDrive = this.GetComponent<CircularDrive>();
        if (!m_CircularDrive)
        {
            this.enabled = false;
            return;
        }

        if (m_CircularDrive.axisOfRotation == CircularDrive.Axis_t.XAxis)
            m_VectorOfRotation = Vector3.right;
        else if (m_CircularDrive.axisOfRotation == CircularDrive.Axis_t.YAxis)
            m_VectorOfRotation = Vector3.up;
        else if (m_CircularDrive.axisOfRotation == CircularDrive.Axis_t.ZAxis)
            m_VectorOfRotation = Vector3.forward;

        m_LinearMapping = this.GetComponent<LinearMapping>();

        if (m_LowerLock > m_UpperLock)
            m_LowerLock = m_UpperLock;
    }

    private void Update()
    {
        m_held = m_CircularDrive.GetDriving();

        if (!m_CircularDrive.GetDriving() &&
            m_CircularDrive.outAngle > m_CircularDrive.minAngle &&
            m_CircularDrive.outAngle < m_CircularDrive.maxAngle)
        {
            if (m_CircularDrive.outAngle < m_LowerLock)
            {
                m_CircularDrive.outAngle -= m_SnapRate * Time.deltaTime;
                if (m_CircularDrive.outAngle < m_CircularDrive.minAngle)
                {
                    m_CircularDrive.outAngle = m_CircularDrive.minAngle;
                    m_CircularDrive.onMinAngle.Invoke();
                }
                m_CircularDrive.UpdateValues();
            }
            else if (m_CircularDrive.outAngle >= m_UpperLock)
            {
                m_CircularDrive.outAngle += m_SnapRate * Time.deltaTime;
                if (m_CircularDrive.outAngle > m_CircularDrive.maxAngle)
                {
                    m_CircularDrive.outAngle = m_CircularDrive.maxAngle;
                    m_CircularDrive.onMaxAngle.Invoke();
                }
                m_CircularDrive.UpdateValues();
            }
        }
    }
}
