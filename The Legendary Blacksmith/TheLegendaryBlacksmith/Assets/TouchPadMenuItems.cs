///Benjamin Ousley
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class TouchPadMenuItems : MonoBehaviour
{
    [SerializeField]
    GameObject tools;
    [SerializeField]
    GameObject structures;
    [SerializeField]
    GameObject Map;

    [SerializeField]
    Image activationIcon = null;

    public void ActivateTools()
    {
        if (structures)
        {
            objectPlacement[] chil = structures.GetComponentsInChildren<objectPlacement>();
            foreach (objectPlacement obj in chil)
                obj.ActivateText(false);
            structures.SetActive(false);
        }
        if (tools)
        {
            if (tools)
            {
                if (tools.activeInHierarchy)
                {
                    objectPlacement[] chil = tools.GetComponentsInChildren<objectPlacement>();
                    foreach (objectPlacement obj in chil)
                        obj.ActivateText(!tools.activeInHierarchy);
                }
                tools.SetActive(!tools.activeInHierarchy);
            }
        }
    }

    public void ActivateStructures()
    {
        if (tools)
        {
            objectPlacement[] chil = tools.GetComponentsInChildren<objectPlacement>();
            foreach (objectPlacement obj in chil)
                obj.ActivateText(false);
            tools.SetActive(false);
        }
        if (structures)
        {
            if (structures)
            {
                if (structures.activeInHierarchy)
                {
                    objectPlacement[] chil = structures.GetComponentsInChildren<objectPlacement>();
                    foreach (objectPlacement obj in chil)
                        obj.ActivateText(!structures.activeInHierarchy);
                }
                structures.SetActive(!structures.activeInHierarchy);
            }
        }
    }

    public void SetForfeit(bool forfeitingBool)
    {
        GameStateManager.managerinstance.StartForfeit(forfeitingBool, activationIcon);
    }

    public void ActivateHoldButton(int type)
    {
        switch (type)
        {
            case (int)ActivateToolsHold.MenuType.Tools:
                {
                    if (tools.activeInHierarchy)
                        ActivateTools();
                    else
                        ActivateToolsHold.instance.StartActivation(true, activationIcon, ActivateTools);
                    break;
                }
            case (int)ActivateToolsHold.MenuType.Structures:
                {
                    if (structures.activeInHierarchy)
                        ActivateStructures();
                    else
                        ActivateToolsHold.instance.StartActivation(true, activationIcon, ActivateStructures);
                    break;
                }
            case (int)ActivateToolsHold.MenuType.Map:
                {
                    break;
                }
            case (int)ActivateToolsHold.MenuType.Forfeit:
                {
                    break;
                }
            default:
                break;

        }
    }

    public void DeActivateHoldButton(int type)
    {
        switch (type)
        {
            case (int)ActivateToolsHold.MenuType.Tools:
                {
                    ActivateToolsHold.instance.StartActivation(false, activationIcon, ActivateTools);
                    break;
                }
            case (int)ActivateToolsHold.MenuType.Structures:
                {
                    ActivateToolsHold.instance.StartActivation(false, activationIcon, ActivateStructures);
                    break;
                }
            case (int)ActivateToolsHold.MenuType.Map:
                {
                    break;
                }
            case (int)ActivateToolsHold.MenuType.Forfeit:
                {
                    break;
                }
            default:
                break;

        }
    }
}
