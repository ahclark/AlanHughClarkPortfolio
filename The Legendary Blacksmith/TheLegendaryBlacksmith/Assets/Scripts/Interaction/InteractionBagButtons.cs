using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class InteractionBagButtons : MonoBehaviour
{
    private TextMesh textMesh;
    private Vector3 oldPosition;
    private Quaternion oldRotation;

    objectPlacement buttonControl;

    bool textActive = false;

    //private float attachTime;

    //private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers);

    //-------------------------------------------------
    void Awake()
    {
        buttonControl = GetComponent<objectPlacement>();
        //textMesh = GetComponentInChildren<TextMesh>();
        //textMesh.text = "No Hand Hovering";
    }

    
    //-------------------------------------------------
    // Called when a Hand starts hovering over this object
    //-------------------------------------------------
    private void OnHandHoverBegin(Hand hand)
    {
        buttonControl.ActivateText(true);
        //textMesh.text = "Hovering hand: " + hand.name;
    }


    //-------------------------------------------------
    // Called when a Hand stops hovering over this object
    //-------------------------------------------------
    private void OnHandHoverEnd(Hand hand)
    {
        buttonControl.ActivateText(false);
        //textMesh.text = "No Hand Hovering";
    }


    //-------------------------------------------------
    // Called every Update() while a Hand is hovering over this object
    //-------------------------------------------------
    private void HandHoverUpdate(Hand hand)
    {
        if(buttonControl)
        buttonControl.ActivateText(true);
        if (hand.GetStandardInteractionButtonDown()/* || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))*/)
        {
            buttonControl.ActivateText(false);
            buttonControl.SetObject(hand);
            //if (hand.currentAttachedObject != gameObject)
            //{
            //    // Save our position/rotation so that we can restore it when we detach
            //    oldPosition = transform.position;
            //    oldRotation = transform.rotation;
            //
            //    // Call this to continue receiving HandHoverUpdate messages,
            //    // and prevent the hand from hovering over anything else
            //    hand.HoverLock(GetComponent<Interactable>());
            //
            //    // Attach this object to the hand
            //    hand.AttachObject(gameObject, attachmentFlags);
            //}
            //else
            //{
            //    // Detach this object from the hand
            //    hand.DetachObject(gameObject);
            //
            //    // Call this to undo HoverLock
            //    hand.HoverUnlock(GetComponent<Interactable>());
            //
            //    // Restore position/rotation
            //    transform.position = oldPosition;
            //    transform.rotation = oldRotation;
            //}
        }
    }


    //-------------------------------------------------
    // Called when this GameObject becomes attached to the hand
    //-------------------------------------------------
    private void OnAttachedToHand(Hand hand)
    {
        //textMesh.text = "Attached to hand: " + hand.name;
        //attachTime = Time.time;
    }


    //-------------------------------------------------
    // Called when this GameObject is detached from the hand
    //-------------------------------------------------
    private void OnDetachedFromHand(Hand hand)
    {
        //textMesh.text = "Detached from hand: " + hand.name;
    }


    //-------------------------------------------------
    // Called every Update() while this GameObject is attached to the hand
    //-------------------------------------------------
    private void HandAttachedUpdate(Hand hand)
    {
        //textMesh.text = "Attached to hand: " + hand.name + "\nAttached time: " + (Time.time - attachTime).ToString("F2");
    }


    //-------------------------------------------------
    // Called when this attached GameObject becomes the primary attached object
    //-------------------------------------------------
    private void OnHandFocusAcquired(Hand hand)
    {
    }


    //-------------------------------------------------
    // Called when another attached GameObject becomes the primary attached object
    //-------------------------------------------------
    private void OnHandFocusLost(Hand hand)
    {
    }
}
