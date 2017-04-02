using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TouchPadRadialMenu))]
public class RadialMenuInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TouchPadRadialMenu rMenu = (TouchPadRadialMenu)target;
        if (GUILayout.Button("Regenerate Buttons"))
        {
            rMenu.RegenerateButtons();
        }
    }
}
