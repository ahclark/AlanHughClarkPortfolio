using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public EquipmentStorage[] Screens;

    [SerializeField]
    TextMesh number;
    public int DisplayNumber;
    public int screen = 0;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void RenderNumber()
    {
        DisplayNumber = screen + 1;
        number.text = "" + DisplayNumber;

    }

    ///Edited
    /// </Benjamin Ousley>
    /// Made the UpdateEquipment function return a boolean to determine whether a unit actually printed
    /// Did this to make the chest close and the particles activate exclusively when a unit is actually printed
    /// Portions of this edit marked with 
    /// //Ben (Bool)
    /// content
    /// //
    /// </1/27/2017>
    public bool UpdateEquipment()
    {
        return Screens[screen].PrintOne();
    }
}
