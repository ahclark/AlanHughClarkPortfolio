using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TroubleMaker : MonoBehaviour
{
    public static TroubleMaker instance = null;

    [SerializeField]
    GameObject ShiftyDude;

    [SerializeField]
    ParticleSystem poof;

    [SerializeField]
    GameObject coin_prefab;

    [SerializeField]
    moraleBar morale;

    AudioSource aud;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    // Use this for initialization
    void Start ()
    {
        ShiftyDude.SetActive(false);
        aud = GetComponent<AudioSource>();
	}

    public void ActivateTroubleMaker()
    {
        ShiftyDude.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent)
        {
            if (other.transform.parent.GetComponent<Hand>())
                return;
        }
        if (other.name == coin_prefab.name && ShiftyDude.activeInHierarchy)
        {
            other.gameObject.SetActive(false);
            StartCoroutine(MakeTrouble());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.parent)
        {
            if (other.transform.parent.GetComponent<Hand>())
                return;
        }
        if (other.name == coin_prefab.name && ShiftyDude.activeInHierarchy)
        {
            other.gameObject.SetActive(false);
            StartCoroutine(MakeTrouble());
        }
    }

    IEnumerator MakeTrouble()
    {
        poof.Play(true);
        aud.Play();
        ShiftyDude.SetActive(false);
        while (poof.isPlaying)
            yield return null;
        if (GameStateManager.managerinstance)
            GameStateManager.managerinstance.PlayGame();
        morale.ResetandPlay();

        yield return null;
    }
}
