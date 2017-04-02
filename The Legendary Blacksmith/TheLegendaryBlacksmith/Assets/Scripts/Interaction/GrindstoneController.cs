using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GrindstoneController : MonoBehaviour
{
    [SerializeField]
    private float m_spinSpeed;

    public float m_spinSpeedModifier = 0.1f;
    public float m_spinSpeedDecay = 5.0f;
    public float m_maxSpinSpeed = 10000.0f;
    public float m_grindSpinSpeed = 4000.0f;
    public GameObject m_grinderCollider;

    [SerializeField]
    private GameObject m_winch;
    private CircularDrive m_circularDrive;
    private bool m_checkAngle;
    private float m_prevOutAngle;
    private float m_angleDifference;

    private void Start()
    {
        m_spinSpeed = 0.0f;
        m_circularDrive = m_winch.GetComponent<CircularDrive>();
        m_checkAngle = false;
    }

    private void Update()
    {
        if (m_circularDrive)
        {
            if (m_circularDrive.GetDriving())
            {
                if (m_checkAngle == false)
                {
                    m_prevOutAngle = m_circularDrive.outAngle;
                    m_checkAngle = true;
                }
                else
                {
                    m_angleDifference = m_prevOutAngle - m_circularDrive.outAngle;
                    m_spinSpeed += m_angleDifference;
                    m_prevOutAngle = m_circularDrive.outAngle;
                }
            }
            else
            {
                m_checkAngle = false;
            }

            if (m_spinSpeed > m_maxSpinSpeed)
                m_spinSpeed = m_maxSpinSpeed;
            else if (m_spinSpeed < m_maxSpinSpeed * -1.0f)
                m_spinSpeed = m_maxSpinSpeed * -1.0f;

            this.transform.Rotate(Vector3.forward, m_spinSpeed * m_spinSpeedModifier * Time.deltaTime);

            if (m_spinSpeed >= m_grindSpinSpeed || m_spinSpeed <= m_grindSpinSpeed * -1.0f)
                m_grinderCollider.SetActive(true);
            else
                m_grinderCollider.SetActive(false);

            m_spinSpeed += Mathf.Sign(m_spinSpeed) * -1 * m_spinSpeedDecay;
            if (m_spinSpeed < 0.01f && m_spinSpeed > -0.01f)
                m_spinSpeed = 0.0f;
        }
    }
}
