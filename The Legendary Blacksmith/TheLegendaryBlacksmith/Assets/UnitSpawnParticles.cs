using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class UnitSpawnParticles : MonoBehaviour 
{
    [SerializeField]
    ParticleSystem particles = null;

    [SerializeField]
    CircularDrive CD;
	// Use this for initialization
	void Start () 
	{
		if(particles == null)
        {
            particles = GetComponent<ParticleSystem>();
        }
	}
	
    public void StartParticles()
    {
        gameObject.SetActive(true);
        particles.Play();
        CheckAngle();
    }
	public void CheckAngle()
    {
        StartCoroutine(CheckLid());
    }

    IEnumerator CheckLid()
    {
        while (CD.outAngle < 10.0f)
        {
            Debug.Log("CheckingLid: " + CD.outAngle);
            yield return null;
        }
        Debug.Log("FinishedCheckLid");
        particles.Stop();
        gameObject.SetActive(false);
    }
}
