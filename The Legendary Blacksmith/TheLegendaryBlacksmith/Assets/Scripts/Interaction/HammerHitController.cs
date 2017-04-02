using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HammerHitController : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    [SerializeField]
    private ParticleSystem m_sparks;

    private void Start()
    {
        m_rigidbody = this.GetComponent<Rigidbody>();
    }

    // Check the veloctiy of the hammer against the value passed in
    // Return true if the velocity is greater or equal
    public bool CheckVelocity(float p_velocity)
    {
        if (m_rigidbody && m_rigidbody.velocity.magnitude >= p_velocity)
            return true;
        else
            return false;
    }

    // Play the sparks particle effect
    public void PlaySparks()
    {
        m_sparks.Play();
    }

    // Play the sparks particle effect at the position passed in
    public void PlaySparks(Vector3 p_position)
    {
        m_sparks.transform.position = p_position;
        m_sparks.Play();
    }

    // Vibrate the controller holding the hammer
    public void VibrateController(int p_value)
    {

    }
}
