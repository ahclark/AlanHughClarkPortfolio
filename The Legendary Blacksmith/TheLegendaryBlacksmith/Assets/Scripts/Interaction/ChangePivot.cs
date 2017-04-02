using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePivot : MonoBehaviour
{
    [SerializeField]
    Vector3 pivot;
    Rigidbody skeleton;

    // Use this for initialization
    void Start ()
    {
        skeleton = GetComponent<Rigidbody>();
        skeleton.centerOfMass = pivot;
    }
}
