using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMeter : MonoBehaviour {
    public Transform StartTrans, EndTrans;
    float start, end;
    public Transform slider;
    public float speed = 3;
    public moraleBar morale;

	// Use this for initialization
	void Start () {
		if(StartTrans && EndTrans)
        {
            start = StartTrans.position.x;
            end = EndTrans.position.x;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if(slider)
        {
            MoveToSlider(Time.deltaTime);
        }
        if(morale.GetPlaying())
            CheckWinLose();

    }

    void CheckWinLose()
    {
        Vector3 dist = StartTrans.position - gameObject.transform.position;
        if (dist.magnitude <= .1f)
        {
            //GameStateManager.managerinstance.ChangeState(GameStateManager.GameState.Lose);
            return;
        }
        dist = EndTrans.position - gameObject.transform.position;
        if(dist.magnitude <= .1f)
        {
            GameStateManager.managerinstance.ChangeState(GameStateManager.GameState.Win);
            morale.SetPlaying(false);
            return;
        }
    }

    void MoveToSlider(float dt)
    {
        Vector3 currPos = gameObject.transform.position;
        Vector3 dist = slider.position - currPos;
        float val = speed * dt * dist.x;

        if (dist.magnitude > 0)
            gameObject.transform.position = currPos + new Vector3(val, 0, 0);

    }
}
