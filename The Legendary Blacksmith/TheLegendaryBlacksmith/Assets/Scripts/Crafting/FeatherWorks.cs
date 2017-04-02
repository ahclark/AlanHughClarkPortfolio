using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherWorks : MonoBehaviour
{
    public Transform m_target;

    private void Start()
    {
        m_target = GameObject.FindGameObjectWithTag("Head").transform;
    }

    void LateUpdate ()
    {
        transform.LookAt(m_target, new Vector3(0, 1, 0));
    }
}
