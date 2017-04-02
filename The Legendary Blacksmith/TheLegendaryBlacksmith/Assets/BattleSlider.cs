using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSlider : MonoBehaviour {

    public Transform start, end;


	// Use this for initialization



    public void SetSliderPos(float value)
    {
        //0-1 position from start to finish
        if(start && end)
        {
            gameObject.transform.position = Vector3.Lerp(start.position, end.position, value);
        }
        
    }
}
