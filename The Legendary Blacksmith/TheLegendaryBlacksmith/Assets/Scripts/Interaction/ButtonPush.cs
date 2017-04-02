using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPush : MonoBehaviour
{
    public bool buttonOn;
    [SerializeField]
    public float buttonOffTimer = 0.5f;

    public UnityEvent OnButtonPush;

    public float offTimer = 0;

	// Use this for initialization
	void Start ()
    {
		
	}
    void Update()
    {
        if (offTimer > 0)
        {
            TurnButtonOff();
        }
    }
    private void OnTriggerEnter(Collider entity)
    {
        if (entity.tag == "Button")
        {
            buttonOn = false;
        }
    }
    private void OnTriggerStay(Collider entity)
    {
        if (entity.tag == "Button")
        {
            buttonOn = false;
        }
    }
    private void OnTriggerExit(Collider entity)
    {
        if (entity.tag == "Button")
        {
            buttonOn = true;
            if (offTimer <= buttonOffTimer * 0.8f)
            {
                GetComponent<AudioSource>().Play();
                OnButtonPush.Invoke();
            }
            offTimer = buttonOffTimer;
        }
    }
    private void TurnButtonOff()
    {
        offTimer -= Time.deltaTime * 1;
        if (offTimer <= 0)
        {
            buttonOn = false;
        }
    }
    public void DoEvents()
    {
        OnButtonPush.Invoke();
    }
}
