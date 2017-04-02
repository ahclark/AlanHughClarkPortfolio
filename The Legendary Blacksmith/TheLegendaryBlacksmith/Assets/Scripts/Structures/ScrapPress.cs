using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class ScrapPress : MonoBehaviour
{
    /// <summary>
    /// Events to call for convenience of interactable objects.
    /// 
    /// OnPressTop = when the press hits its highest point.
    /// OnPressBottom = when the press hits its lowerst point.
    /// OnPressUpActive = when the press is moving up, called in a FixedUpdate() style
    /// OnPressDownActive = when the press is moving down, called in a FixedUpdate() style
    /// 
    /// float ratio = a 0-1 floating point scale of how far up/down the press is.
    /// </summary>
    public delegate void OnPressUpDelegate();
    public static event OnPressUpDelegate OnPressTop;

    public delegate void OnPressDownDelegate();
    public static event OnPressDownDelegate OnPressBottom;

    public delegate void OnPressUpActiveDelegate(float ratio);
    public static event OnPressUpActiveDelegate OnPressUpActive;

    public delegate void OnPressDownActiveDelegate(float ratio);
    public static event OnPressDownActiveDelegate OnPressDownActive;

    public UnityEvent AtTop;
    public UnityEvent AtBottom;

    [SerializeField]
    GameObject start;
    [SerializeField]
    GameObject end;
    [SerializeField]
    bool up = false;
    [SerializeField]
    bool down = false;
    [SerializeField]
    float speed = 1;
    [SerializeField]
    float speedDown = 1;
    [SerializeField]
    MoldMaker mold;
    bool doOnce = true;
    float timer = 0;
    public bool shieldIn = false;
    [SerializeField]
    LinearMapping value;

    private bool upOnce = false;
    private bool downOnce = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (up)
        {
            //if (timer < 1)
            if(value.value < 1)
            {
                //timer += Time.deltaTime * speed;

                if(OnPressUpActive != null)
                    //OnPressUpActive(timer);
                    OnPressUpActive(value.value);
            }
            else
            {
                up = false;
                if (OnPressTop != null)
                     OnPressTop();
                
            }
            
        }
        if (down)
        {
            //if (timer > 0)
            if (value.value > 0)
            {
                //timer -= Time.deltaTime * speed;
                SlamItDown();

                if(shieldIn)
                {
                    //if (timer < 0.155f)
                    if (value.value < 0.155f)
                    {
                        mold.SquishShield();
                        //if (timer <= 0.08f)
                        if (value.value <= 0.08f)
                        {
                            down = false;
                        }
                    }
                    
                }
                if (OnPressDownActive != null)
                    OnPressDownActive(timer);

            }
            else
            {

                down = false;
                value.value = 0;
                mold.RemoveScraps();
                if (OnPressBottom != null)
                    OnPressBottom();
            }
        }
        //transform.position = Vector3.Lerp(end.transform.position, start.transform.position, timer);
        LerpItUp();
        if (value.value >= 1 && !upOnce)
        {
            AtTop.Invoke();
            upOnce = true;
            downOnce = false;
        }
        if (value.value <= 0 && !downOnce)
        {
            AtBottom.Invoke();
            upOnce = false;
            downOnce = true;
        }

    }

    public void GoUp()
    {
        ResetPress();
        up = true;
        doOnce = true;
    }
    public void GoDown()
    {
        ResetPress();
        down = true;
            
    }
    public void ResetPress()
    {
        up = false;
        down = false;
    }

    public void LerpItUp()
    {
        transform.position = Vector3.Lerp(end.transform.position, start.transform.position, value.value);
    }

    public void CrankItUp()
    {
        value.value += speed;
    }

    public void SlamItDown()
    {
        value.value -= speedDown;
    }
}
