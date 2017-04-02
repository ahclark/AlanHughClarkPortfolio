using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandTutorials : MonoBehaviour {
    [SerializeField]
    Hand myHand;
    [SerializeField]
    TutorialMovement PuckToFurnace;
    [SerializeField]
    TutorialMovement ForgeToAnvil;
    [SerializeField]
    TutorialMovement HammerOnAnvil;
    [SerializeField]
    TutorialMovement AnvilToWater;
    [SerializeField]
    TutorialMovement WaterToGrinder;
    [SerializeField]
    TutorialMovement WoodToShaver;
    [SerializeField]
    TutorialMovement WoodToChop;
    [SerializeField]
    TutorialMovement ScrapBarToForge;
    [SerializeField]
    TutorialMovement HeatedBarToBell;
    [SerializeField]
    TutorialMovement HammerToBell;
    [SerializeField]
    TutorialMovement HiltToGrinder;




    

    public void JustPickedUp()
    {
        //called when we press the trigger to tell if we are grabbing something
        if(!myHand)
        {
            //hand is null
            return;
        }
        if (myHand.AttachedObjects.Count == 0)
        {
            //not holding anything
            return;
        }

        string heldObjName;
        for (int i = 0; i < myHand.AttachedObjects.Count; i++)
        {
            heldObjName = myHand.AttachedObjects[i].attachedObject.tag;
            switch(heldObjName)
            {
                case "Puck":
                    {
                        InitTutorial(PuckToFurnace, true);
                        InitTutorial(ForgeToAnvil, false);
                        InitTutorial(HammerOnAnvil, false);
                        InitTutorial(AnvilToWater, false);
                        InitTutorial(WaterToGrinder, false);

                        break;
                    }
                case "Wood":
                    {
                        InitTutorial(WoodToShaver, true);
                        InitTutorial(WoodToChop, true);
                        break;
                    }
                case "ScrapBar":
                    {
                        InitTutorial(ScrapBarToForge, true);
                        InitTutorial(HeatedBarToBell, false);
                        InitTutorial(HammerToBell, false);
                        break;
                    }
                case "WoodHilt":
                    {
                        InitTutorial(HiltToGrinder, true);
                        break;
                    }

            }
        }
 
    }




   public void JustLetGo()
    {
        ResetTutorial(PuckToFurnace);
        ResetTutorial(ForgeToAnvil);
        ResetTutorial(HammerOnAnvil);
        ResetTutorial(AnvilToWater);
        ResetTutorial(WaterToGrinder);
        ResetTutorial(WoodToChop);
        ResetTutorial(WoodToShaver);
        ResetTutorial(ScrapBarToForge);
        ResetTutorial(HeatedBarToBell);
        ResetTutorial(HammerToBell);
        ResetTutorial(HiltToGrinder);
    }

    void InitTutorial(TutorialMovement tut, bool setStartPos)
    {
        if (tut)
        {
            tut.gameObject.SetActive(true);
            if(setStartPos)
            tut.SetStartPosition(myHand.gameObject);
        }
    }

    void ResetTutorial(TutorialMovement tut)
    {
        if (tut)
        {
            tut.gameObject.SetActive(false);
        }
    }
}
