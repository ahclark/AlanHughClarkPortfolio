///
///Based off SteamVR's InteractionButtonEvents
///Added functionality:
///ApplicationMenu Button Events
///Interactive component not required
///



using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;
using System;

public struct ControllerInteractionEventArgs
{
    public uint controllerIndex;
    public float buttonPressure;
    public Vector2 touchpadAxis;
    public float touchpadAngle;
}

public delegate void ControllerInteractionEventHandler(object sender, ControllerInteractionEventArgs e);

//-------------------------------------------------------------------------
public class HandButtonEvents : MonoBehaviour
{
    public UnityEvent onTriggerDown;
    public UnityEvent onTriggerUp;
    public UnityEvent onGripDown;
    public UnityEvent onGripUp;
    public UnityEvent onTouchpadDown;
    public UnityEvent onTouchpadUp;
    public UnityEvent onTouchpadTouch;
    public UnityEvent onTouchpadRelease;
    public UnityEvent onApplicationMenuDown;
    public UnityEvent onApplicationMenuUp;


    public event ControllerInteractionEventHandler TriggerPressed;
    public event ControllerInteractionEventHandler TriggerReleased;
    public event ControllerInteractionEventHandler GripPressed;
    public event ControllerInteractionEventHandler GripReleased;
    public event ControllerInteractionEventHandler TouchpadPressed;
    public event ControllerInteractionEventHandler TouchpadReleased;
    public event ControllerInteractionEventHandler TouchpadTouchStart;
    public event ControllerInteractionEventHandler TouchpadTouchEnd;
    public event ControllerInteractionEventHandler TouchpadAxisChanged;



    [SerializeField]
    public Hand hand;

    //-------------------------------------------------
    void Update()
    {
        #region OneHand
        if (hand)
        {
            if (hand.controller != null)
            {
                //Trigger Pressed
                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
                {
                    OnTriggerPressed(SetButtonEvent(ref triggerPressed, true, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x));
                    EmitAlias(ButtonAlias.Trigger_Press, true, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x, ref triggerPressed);
                }

                // Trigger Pressed end
                if (hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
                {
                    OnTriggerReleased(SetButtonEvent(ref triggerPressed, false, 0f));
                    EmitAlias(ButtonAlias.Trigger_Press, false, 0f, ref triggerPressed);
                }

                //Trigger Axis
                if (Vector2ShallowEquals(triggerAxis, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)))
                {
                    triggerAxisChanged = false;
                }
                else
                {
                    OnTriggerAxisChanged(SetButtonEvent(ref triggerAxisChanged, true, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x));
                }


                //Grip Pressed
                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
                {
                    OnGripPressed(SetButtonEvent(ref gripPressed, true, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Grip).x));
                    EmitAlias(ButtonAlias.Grip_Press, true, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Grip).x, ref gripPressed);
                }

                // Grip Pressed End
                if (hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_Grip))
                {
                    OnGripReleased(SetButtonEvent(ref gripPressed, false, 0f));
                    EmitAlias(ButtonAlias.Grip_Press, false, 0f, ref gripPressed);
                }

                //Grip Axis
                if (Vector2ShallowEquals(gripAxis, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Grip)))
                {
                    gripAxisChanged = false;
                }
                else
                {
                    OnGripAxisChanged(SetButtonEvent(ref gripAxisChanged, true, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Grip).x));
                }

                //Touchpad Touched
                if (hand.controller.GetTouch(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    OnTouchpadTouchStart(SetButtonEvent(ref touchpadTouched, true, 1f));
                    EmitAlias(ButtonAlias.Touchpad_Touch, true, 1f, ref touchpadTouched);
                }

                //Touchpad Pressed
                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    OnTouchpadPressed(SetButtonEvent(ref touchpadPressed, true, 1f));
                    EmitAlias(ButtonAlias.Touchpad_Press, true, 1f, ref touchpadPressed);
                }
                else if (hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    OnTouchpadReleased(SetButtonEvent(ref touchpadPressed, false, 0f));
                    EmitAlias(ButtonAlias.Touchpad_Press, false, 0f, ref touchpadPressed);
                }

                //Touchpad Untouched
                if (hand.controller.GetTouchUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    OnTouchpadTouchEnd(SetButtonEvent(ref touchpadTouched, false, 0f));
                    EmitAlias(ButtonAlias.Touchpad_Touch, false, 0f, ref touchpadTouched);
                }

                if (Vector2ShallowEquals(touchpadAxis, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)))
                {
                    touchpadAxisChanged = false;
                }
                else
                {
                    OnTouchpadAxisChanged(SetButtonEvent(ref touchpadAxisChanged, true, 1f));
                }

                //StartMenu Pressed
                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu))
                {
                    OnStartMenuPressed(SetButtonEvent(ref startMenuPressed, true, 1f));
                    EmitAlias(ButtonAlias.Start_Menu_Press, true, 1f, ref startMenuPressed);
                }
                else if (hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu))
                {
                    OnStartMenuReleased(SetButtonEvent(ref startMenuPressed, false, 0f));
                    EmitAlias(ButtonAlias.Start_Menu_Press, false, 0f, ref startMenuPressed);
                }

                // Save current touch and trigger settings to detect next change.
                touchpadAxis = new Vector2(hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y);
                triggerAxis = new Vector2(hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).y);
                gripAxis = new Vector2(hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Grip).x, hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Grip).y);
            }
        }
        #endregion

        #region BothHands
        else
        {
            for (int i = 0; i < Player.instance.handCount; i++)
            {
                Hand hand = Player.instance.GetHand(i);

                if (hand.controller != null)
                {
                    if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
                    {
                        onTriggerDown.Invoke();
                    }

                    if (hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
                    {
                        onTriggerUp.Invoke();
                    }

                    if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
                    {
                        onGripDown.Invoke();
                    }

                    if (hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_Grip))
                    {
                        onGripUp.Invoke();
                    }

                    if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                    {
                        onTouchpadDown.Invoke();
                    }

                    if (hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                    {
                        onTouchpadUp.Invoke();
                    }

                    if (hand.controller.GetTouchDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                    {
                        onTouchpadTouch.Invoke();
                    }

                    if (hand.controller.GetTouchUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                    {
                        onTouchpadRelease.Invoke();
                    }

                    if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu))
                    {
                        onApplicationMenuDown.Invoke();
                    }

                    if (hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu))
                    {
                        onApplicationMenuUp.Invoke();
                    }
                }
            }
        }
        #endregion

    }

    public virtual void OnTriggerPressed(ControllerInteractionEventArgs e)
    {
        if (TriggerPressed != null)
        {
            TriggerPressed(this, e);
        }
    }

    public virtual void OnTriggerReleased(ControllerInteractionEventArgs e)
    {
        if (TriggerReleased != null)
        {
            TriggerReleased(this, e);
        }
    }

    public virtual void OnGripPressed(ControllerInteractionEventArgs e)
    {
        if (GripPressed != null)
        {
            GripPressed(this, e);
        }
    }

    public virtual void OnGripReleased(ControllerInteractionEventArgs e)
    {
        if (GripReleased != null)
        {
            GripReleased(this, e);
        }
    }


    public virtual void OnTouchpadPressed(ControllerInteractionEventArgs e)
    {
        if (TouchpadPressed != null)
        {
            TouchpadPressed(this, e);
        }
    }

    public virtual void OnTouchpadReleased(ControllerInteractionEventArgs e)
    {
        if (TouchpadReleased != null)
        {
            TouchpadReleased(this, e);
        }
    }

    public virtual void OnTouchpadTouchStart(ControllerInteractionEventArgs e)
    {
        if (TouchpadTouchStart != null)
        {
            TouchpadTouchStart(this, e);
        }
    }

    public virtual void OnTouchpadTouchEnd(ControllerInteractionEventArgs e)
    {
        if (TouchpadTouchEnd != null)
        {
            TouchpadTouchEnd(this, e);
        }
    }

    public virtual void OnTouchpadAxisChanged(ControllerInteractionEventArgs e)
    {
        if (TouchpadAxisChanged != null)
        {
            TouchpadAxisChanged(this, e);
        }
    }

    private ControllerInteractionEventArgs SetButtonEvent(ref bool buttonBool, bool value, float buttonPressure)
    {
        var controllerIndex = hand.controller.index;
        buttonBool = value;
        ControllerInteractionEventArgs e;
        e.controllerIndex = controllerIndex;
        e.buttonPressure = buttonPressure;
        e.touchpadAxis = hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
        e.touchpadAngle = CalculateTouchpadAxisAngle(e.touchpadAxis);
        return e;
    }

    private float CalculateTouchpadAxisAngle(Vector2 axis)
    {
        float angle = Mathf.Atan2(axis.y, axis.x) * Mathf.Rad2Deg;
        angle = 90.0f - angle;
        if (angle < 0)
        {
            angle += 360.0f;
        }
        return angle;
    }

    public bool IsButtonPressed(ButtonAlias button)
    {
        switch (button)
        {
            case ButtonAlias.Trigger_Hairline:
                return triggerHairlinePressed;
            case ButtonAlias.Trigger_Touch:
                return triggerTouched;
            case ButtonAlias.Trigger_Press:
                return triggerPressed;
            case ButtonAlias.Trigger_Click:
                return triggerClicked;
            case ButtonAlias.Grip_Hairline:
                return gripHairlinePressed;
            case ButtonAlias.Grip_Touch:
                return gripTouched;
            case ButtonAlias.Grip_Press:
                return gripPressed;
            case ButtonAlias.Grip_Click:
                return gripClicked;
            case ButtonAlias.Touchpad_Touch:
                return touchpadTouched;
            case ButtonAlias.Touchpad_Press:
                return touchpadPressed;
            case ButtonAlias.Button_One_Press:
                return buttonOnePressed;
            case ButtonAlias.Button_One_Touch:
                return buttonOneTouched;
            case ButtonAlias.Button_Two_Press:
                return buttonTwoPressed;
            case ButtonAlias.Button_Two_Touch:
                return buttonTwoTouched;
            case ButtonAlias.Start_Menu_Press:
                return startMenuPressed;
        }
        return false;
    }

    public enum ButtonAlias
    {
        Undefined,
        Trigger_Hairline,
        Trigger_Touch,
        Trigger_Press,
        Trigger_Click,
        Grip_Hairline,
        Grip_Touch,
        Grip_Press,
        Grip_Click,
        Touchpad_Touch,
        Touchpad_Press,
        Button_One_Touch,
        Button_One_Press,
        Button_Two_Touch,
        Button_Two_Press,
        Start_Menu_Press
    }

    /// <summary>
    /// This will be true if the trigger is squeezed about half way in.
    /// </summary>
    [HideInInspector]
    public bool triggerPressed = false;
    /// <summary>
    /// This will be true if the trigger is squeezed a small amount.
    /// </summary>
    [HideInInspector]
    public bool triggerTouched = false;
    /// <summary>
    /// This will be true if the trigger is squeezed a small amount more from any previous squeeze on the trigger.
    /// </summary>
    [HideInInspector]
    public bool triggerHairlinePressed = false;
    /// <summary>
    /// This will be true if the trigger is squeezed all the way down.
    /// </summary>
    [HideInInspector]
    public bool triggerClicked = false;
    /// <summary>
    /// This will be true if the trigger has been squeezed more or less.
    /// </summary>
    [HideInInspector]
    public bool triggerAxisChanged = false;

    /// <summary>
    /// This will be true if the grip is squeezed about half way in.
    /// </summary>
    [HideInInspector]
    public bool gripPressed = false;
    /// <summary>
    /// This will be true if the grip is touched.
    /// </summary>
    [HideInInspector]
    public bool gripTouched = false;
    /// <summary>
    /// This will be true if the grip is squeezed a small amount more from any previous squeeze on the grip.
    /// </summary>
    [HideInInspector]
    public bool gripHairlinePressed = false;
    /// <summary>
    /// This will be true if the grip is squeezed all the way down.
    /// </summary>
    [HideInInspector]
    public bool gripClicked = false;
    /// <summary>
    /// This will be true if the grip has been squeezed more or less.
    /// </summary>
    [HideInInspector]
    public bool gripAxisChanged = false;

    /// <summary>
    /// This will be true if the touchpad is held down.
    /// </summary>
    [HideInInspector]
    public bool touchpadPressed = false;
    /// <summary>
    /// This will be true if the touchpad is being touched.
    /// </summary>
    [HideInInspector]
    public bool touchpadTouched = false;
    /// <summary>
    /// This will be true if the touchpad touch position has changed.
    /// </summary>
    [HideInInspector]
    public bool touchpadAxisChanged = false;

    /// <summary>
    /// This will be true if button one is held down.
    /// </summary>
    [HideInInspector]
    public bool buttonOnePressed = false;
    /// <summary>
    /// This will be true if button one is being touched.
    /// </summary>
    [HideInInspector]
    public bool buttonOneTouched = false;

    /// <summary>
    /// This will be true if button two is held down.
    /// </summary>
    [HideInInspector]
    public bool buttonTwoPressed = false;
    /// <summary>
    /// This will be true if button two is being touched.
    /// </summary>
    [HideInInspector]
    public bool buttonTwoTouched = false;

    /// <summary>
    /// This will be true if start menu is held down.
    /// </summary>
    [HideInInspector]
    public bool startMenuPressed = false;

    /// <summary>
    /// This will be true if the button aliased to the pointer is held down.
    /// </summary>
    [HideInInspector]
    public bool pointerPressed = false;
    /// <summary>
    /// This will be true if the button aliased to the grab is held down.
    /// </summary>
    [HideInInspector]
    public bool grabPressed = false;
    /// <summary>
    /// This will be true if the button aliased to the use is held down.
    /// </summary>
    [HideInInspector]
    public bool usePressed = false;
    /// <summary>
    /// This will be true if the button aliased to the UI click is held down.
    /// </summary>
    [HideInInspector]
    public bool uiClickPressed = false;
    /// <summary>
    /// This will be true if the button aliased to the menu is held down.
    /// </summary>
    [HideInInspector]
    public bool menuPressed = false;

    /// <summary>
    /// Emitted when the trigger is squeezed a small amount.
    /// </summary>
    public event ControllerInteractionEventHandler TriggerTouchStart;
    /// <summary>
    /// Emitted when the trigger is no longer being squeezed at all.
    /// </summary>
    public event ControllerInteractionEventHandler TriggerTouchEnd;

    /// <summary>
    /// Emitted when the trigger is squeezed past the current hairline threshold.
    /// </summary>
    public event ControllerInteractionEventHandler TriggerHairlineStart;
    /// <summary>
    /// Emitted when the trigger is released past the current hairline threshold.
    /// </summary>
    public event ControllerInteractionEventHandler TriggerHairlineEnd;

    /// <summary>
    /// Emitted when the trigger is squeezed all the way down.
    /// </summary>
    public event ControllerInteractionEventHandler TriggerClicked;
    /// <summary>
    /// Emitted when the trigger is no longer being held all the way down.
    /// </summary>
    public event ControllerInteractionEventHandler TriggerUnclicked;

    /// <summary>
    /// Emitted when the amount of squeeze on the trigger changes.
    /// </summary>
    public event ControllerInteractionEventHandler TriggerAxisChanged;

    /// <summary>
    /// Emitted when the grip is squeezed a small amount.
    /// </summary>
    public event ControllerInteractionEventHandler GripTouchStart;
    /// <summary>
    /// Emitted when the grip is no longer being squeezed at all.
    /// </summary>
    public event ControllerInteractionEventHandler GripTouchEnd;

    /// <summary>
    /// Emitted when the grip is squeezed past the current hairline threshold.
    /// </summary>
    public event ControllerInteractionEventHandler GripHairlineStart;
    /// <summary>
    /// Emitted when the grip is released past the current hairline threshold.
    /// </summary>
    public event ControllerInteractionEventHandler GripHairlineEnd;

    /// <summary>
    /// Emitted when the grip is squeezed all the way down.
    /// </summary>
    public event ControllerInteractionEventHandler GripClicked;
    /// <summary>
    /// Emitted when the grip is no longer being held all the way down.
    /// </summary>
    public event ControllerInteractionEventHandler GripUnclicked;

    /// <summary>
    /// Emitted when the amount of squeeze on the grip changes.
    /// </summary>
    public event ControllerInteractionEventHandler GripAxisChanged;

    /// <summary>
    /// Emitted when button one is touched.
    /// </summary>
    public event ControllerInteractionEventHandler ButtonOneTouchStart;
    /// <summary>
    /// Emitted when button one is no longer being touched.
    /// </summary>
    public event ControllerInteractionEventHandler ButtonOneTouchEnd;
    /// <summary>
    /// Emitted when button one is pressed.
    /// </summary>
    public event ControllerInteractionEventHandler ButtonOnePressed;
    /// <summary>
    /// Emitted when button one is released.
    /// </summary>
    public event ControllerInteractionEventHandler ButtonOneReleased;

    /// <summary>
    /// Emitted when button two is touched.
    /// </summary>
    public event ControllerInteractionEventHandler ButtonTwoTouchStart;
    /// <summary>
    /// Emitted when button two is no longer being touched.
    /// </summary>
    public event ControllerInteractionEventHandler ButtonTwoTouchEnd;
    /// <summary>
    /// Emitted when button two is pressed.
    /// </summary>
    public event ControllerInteractionEventHandler ButtonTwoPressed;
    /// <summary>
    /// Emitted when button two is released.
    /// </summary>
    public event ControllerInteractionEventHandler ButtonTwoReleased;

    /// <summary>
    /// Emitted when start menu is pressed.
    /// </summary>
    public event ControllerInteractionEventHandler StartMenuPressed;
    /// <summary>
    /// Emitted when start menu is released.
    /// </summary>
    public event ControllerInteractionEventHandler StartMenuReleased;

    /// <summary>
    /// Emitted when the pointer toggle alias button is pressed.
    /// </summary>
    public event ControllerInteractionEventHandler AliasPointerOn;
    /// <summary>
    /// Emitted when the pointer toggle alias button is released.
    /// </summary>
    public event ControllerInteractionEventHandler AliasPointerOff;
    /// <summary>
    /// Emitted when the pointer set alias button is released.
    /// </summary>
    public event ControllerInteractionEventHandler AliasPointerSet;

    /// <summary>
    /// Emitted when the grab toggle alias button is pressed.
    /// </summary>
    public event ControllerInteractionEventHandler AliasGrabOn;
    /// <summary>
    /// Emitted when the grab toggle alias button is released.
    /// </summary>
    public event ControllerInteractionEventHandler AliasGrabOff;

    /// <summary>
    /// Emitted when the use toggle alias button is pressed.
    /// </summary>
    public event ControllerInteractionEventHandler AliasUseOn;
    /// <summary>
    /// Emitted when the use toggle alias button is released.
    /// </summary>
    public event ControllerInteractionEventHandler AliasUseOff;

    /// <summary>
    /// Emitted when the menu toggle alias button is pressed.
    /// </summary>
    public event ControllerInteractionEventHandler AliasMenuOn;
    /// <summary>
    /// Emitted when the menu toggle alias button is released.
    /// </summary>
    public event ControllerInteractionEventHandler AliasMenuOff;

    /// <summary>
    /// Emitted when the UI click alias button is pressed.
    /// </summary>
    public event ControllerInteractionEventHandler AliasUIClickOn;
    /// <summary>
    /// Emitted when the UI click alias button is released.
    /// </summary>
    public event ControllerInteractionEventHandler AliasUIClickOff;

    /// <summary>
    /// Emitted when the controller is enabled.
    /// </summary>
    public event ControllerInteractionEventHandler ControllerEnabled;
    /// <summary>
    /// Emitted when the controller is disabled.
    /// </summary>
    public event ControllerInteractionEventHandler ControllerDisabled;
    /// <summary>
    /// Emitted when the controller index changed.
    /// </summary>
    public event ControllerInteractionEventHandler ControllerIndexChanged;

    private void EmitAlias(ButtonAlias type, bool touchDown, float buttonPressure, ref bool buttonBool)
    {
        if (pointerToggleButton == type)
        {
            if (touchDown)
            {
                pointerPressed = true;
                OnAliasPointerOn(SetButtonEvent(ref buttonBool, true, buttonPressure));
            }
            else
            {
                pointerPressed = false;
                OnAliasPointerOff(SetButtonEvent(ref buttonBool, false, buttonPressure));
            }
        }

        if (pointerSetButton == type)
        {
            if (!touchDown)
            {
                OnAliasPointerSet(SetButtonEvent(ref buttonBool, false, buttonPressure));
            }
        }

        if (grabToggleButton == type)
        {
            if (touchDown)
            {
                grabPressed = true;
                OnAliasGrabOn(SetButtonEvent(ref buttonBool, true, buttonPressure));
            }
            else
            {
                grabPressed = false;
                OnAliasGrabOff(SetButtonEvent(ref buttonBool, false, buttonPressure));
            }
        }

        if (useToggleButton == type)
        {
            if (touchDown)
            {
                usePressed = true;
                OnAliasUseOn(SetButtonEvent(ref buttonBool, true, buttonPressure));
            }
            else
            {
                usePressed = false;
                OnAliasUseOff(SetButtonEvent(ref buttonBool, false, buttonPressure));
            }
        }

        if (uiClickButton == type)
        {
            if (touchDown)
            {
                uiClickPressed = true;
                OnAliasUIClickOn(SetButtonEvent(ref buttonBool, true, buttonPressure));
            }
            else
            {
                uiClickPressed = false;
                OnAliasUIClickOff(SetButtonEvent(ref buttonBool, false, buttonPressure));
            }
        }

        if (menuToggleButton == type)
        {
            if (touchDown)
            {
                menuPressed = true;
                OnAliasMenuOn(SetButtonEvent(ref buttonBool, true, buttonPressure));
            }
            else
            {
                menuPressed = false;
                OnAliasMenuOff(SetButtonEvent(ref buttonBool, false, buttonPressure));
            }
        }
    }

    [Header("Action Alias Buttons")]

    [Tooltip("The button to use for the action of turning a laser pointer on / off.")]
    public ButtonAlias pointerToggleButton = ButtonAlias.Touchpad_Press;
    [Tooltip("The button to use for the action of setting a destination marker from the cursor position of the pointer.")]
    public ButtonAlias pointerSetButton = ButtonAlias.Touchpad_Press;
    [Tooltip("The button to use for the action of grabbing game objects.")]
    public ButtonAlias grabToggleButton = ButtonAlias.Grip_Press;
    [Tooltip("The button to use for the action of using game objects.")]
    public ButtonAlias useToggleButton = ButtonAlias.Trigger_Press;
    [Tooltip("The button to use for the action of clicking a UI element.")]
    public ButtonAlias uiClickButton = ButtonAlias.Trigger_Press;
    [Tooltip("The button to use for the action of bringing up an in-game menu.")]
    public ButtonAlias menuToggleButton = ButtonAlias.Button_Two_Press;

    public virtual void OnAliasPointerOn(ControllerInteractionEventArgs e)
    {
        if (AliasPointerOn != null)
        {
            AliasPointerOn(this, e);
        }
    }

    public virtual void OnAliasPointerOff(ControllerInteractionEventArgs e)
    {
        if (AliasPointerOff != null)
        {
            AliasPointerOff(this, e);
        }
    }

    public virtual void OnAliasPointerSet(ControllerInteractionEventArgs e)
    {
        if (AliasPointerSet != null)
        {
            AliasPointerSet(this, e);
        }
    }

    public virtual void OnAliasGrabOn(ControllerInteractionEventArgs e)
    {
        if (AliasGrabOn != null)
        {
            AliasGrabOn(this, e);
        }
    }

    public virtual void OnAliasGrabOff(ControllerInteractionEventArgs e)
    {
        if (AliasGrabOff != null)
        {
            AliasGrabOff(this, e);
        }
    }

    public virtual void OnAliasUseOn(ControllerInteractionEventArgs e)
    {
        if (AliasUseOn != null)
        {
            AliasUseOn(this, e);
        }
    }

    public virtual void OnAliasUseOff(ControllerInteractionEventArgs e)
    {
        if (AliasUseOff != null)
        {
            AliasUseOff(this, e);
        }
    }

    public virtual void OnAliasUIClickOn(ControllerInteractionEventArgs e)
    {
        if (AliasUIClickOn != null)
        {
            AliasUIClickOn(this, e);
        }
    }

    public virtual void OnAliasUIClickOff(ControllerInteractionEventArgs e)
    {
        if (AliasUIClickOff != null)
        {
            AliasUIClickOff(this, e);
        }
    }

    public virtual void OnAliasMenuOn(ControllerInteractionEventArgs e)
    {
        if (AliasMenuOn != null)
        {
            AliasMenuOn(this, e);
        }
    }

    public virtual void OnAliasMenuOff(ControllerInteractionEventArgs e)
    {
        if (AliasMenuOff != null)
        {
            AliasMenuOff(this, e);
        }
    }

    private bool Vector2ShallowEquals(Vector2 vectorA, Vector2 vectorB)
    {
        var distanceVector = vectorA - vectorB;
        return Math.Round(Mathf.Abs(distanceVector.x), axisFidelity, MidpointRounding.AwayFromZero) < float.Epsilon
               && Math.Round(Mathf.Abs(distanceVector.y), axisFidelity, MidpointRounding.AwayFromZero) < float.Epsilon;
    }

    [Header("Axis Refinement")]

    [Tooltip("The amount of fidelity in the changes on the axis, which is defaulted to 1. Any number higher than 2 will probably give too sensitive results.")]
    public int axisFidelity = 1;
    [Tooltip("The level on the trigger axis to reach before a click is registered.")]
    public float triggerClickThreshold = 1f;
    [Tooltip("The level on the grip axis to reach before a click is registered.")]
    public float gripClickThreshold = 1f;

    private Vector2 touchpadAxis = Vector2.zero;
    private Vector2 triggerAxis = Vector2.zero;
    private Vector2 gripAxis = Vector2.zero;

    public virtual void OnTriggerAxisChanged(ControllerInteractionEventArgs e)
    {
        if (TriggerAxisChanged != null)
        {
            TriggerAxisChanged(this, e);
        }
    }

    public virtual void OnGripAxisChanged(ControllerInteractionEventArgs e)
    {
        if (GripAxisChanged != null)
        {
            GripAxisChanged(this, e);
        }
    }

    public virtual void OnStartMenuPressed(ControllerInteractionEventArgs e)
    {
        if (StartMenuPressed != null)
        {
            StartMenuPressed(this, e);
        }
    }

    public virtual void OnStartMenuReleased(ControllerInteractionEventArgs e)
    {
        if (StartMenuReleased != null)
        {
            StartMenuReleased(this, e);
        }
    }
}
