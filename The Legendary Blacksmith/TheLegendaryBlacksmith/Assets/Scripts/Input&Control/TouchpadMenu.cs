using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchpadMenu : MonoBehaviour {

    public TextMesh text;

    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

    GameObject[] menuItems;
    GameObject currItem;
    int counter = 0;

    Valve.VR.EVRButtonId TouchPad_Axis = Valve.VR.EVRButtonId.k_EButton_Axis0;

    bool trackingSwipe = false;
    bool checkingSwipe = false;

    // The angle range for detecting swipe
    private const float angleRange = 30;

    // Required Swipe dist
    private const float minSwipeDist = 0.2f;

    // Required velocity of swipe
    private const float minVelocity = 4.0f;

    //Start and end positioning for swipe
    Vector2 startpos, endpos;

    //Starting time for swipe
    float swipeTimeStart = 0.0f;

    //X and Y Axes
    private readonly Vector2 mXAxis = new Vector2(1, 0);
    private readonly Vector2 mYAxis = new Vector2(0, 1);


    // Use this for initialization
    void Start ()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);
        menuItems = Resources.LoadAll<GameObject>("TouchPadMenu");
        text.text = menuItems[0].name;
        currItem = menuItems[0];
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Touch down on Pad
		if(device.GetTouchDown(TouchPad_Axis))
        {
            trackingSwipe = true;
            startpos = new Vector2(device.GetAxis(TouchPad_Axis).x, device.GetAxis(TouchPad_Axis).y);
            swipeTimeStart = Time.time;

        }

        //Touch up from pad
        if(device.GetTouchUp(TouchPad_Axis))
        {
            trackingSwipe = false;
            trackingSwipe = true;
            checkingSwipe = true;
        }

        //If still Touching
        else if(trackingSwipe)
        {
            endpos = new Vector2(device.GetAxis(TouchPad_Axis).x, device.GetAxis(TouchPad_Axis).y);
        }

        //Check to see if valid swipe
        if(checkingSwipe)
        {
            checkingSwipe = false;
            float dt = Time.time - swipeTimeStart;
            Vector2 swipeVector = endpos - startpos;
            float velocity = swipeVector.magnitude / dt;

            if(velocity > minVelocity && swipeVector.magnitude > minSwipeDist)
            {
                //Detect Left and Right Swipe
                swipeVector.Normalize();
                float swipeAngle = Vector2.Dot(swipeVector, mXAxis);
                swipeAngle = Mathf.Acos(swipeAngle) * Mathf.Rad2Deg;

                if(swipeAngle < angleRange)
                    SwipeRight();
                else if(180.0f - swipeAngle < angleRange)
                    SwipeLeft();

                //Detect Up and Down Swipe
                else
                {
                    swipeAngle = Vector2.Dot(swipeVector, mYAxis);
                    swipeAngle = Mathf.Acos(swipeAngle) * Mathf.Rad2Deg;
                    if (swipeAngle < angleRange)
                        SwipeUp();
                    else if (180.0f - swipeAngle < angleRange)
                        SwipeDown();
                }
            }
        }
	}

    void SwipeLeft()
    {
        counter--;
        if (counter < 0)
            counter = menuItems.Length - 1;
        currItem = menuItems[counter];
        text.text = currItem.name;
    }

    void SwipeRight()
    {
        counter++;
        if (counter >= menuItems.Length)
            counter = 0;
        currItem = menuItems[counter];
        text.text = currItem.name;
    }

    void SwipeUp()
    {

    }

    void SwipeDown()
    {

    }
}
