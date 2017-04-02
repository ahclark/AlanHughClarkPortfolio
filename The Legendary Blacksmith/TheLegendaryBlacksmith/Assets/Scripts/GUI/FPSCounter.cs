using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    float deltaTime = 0.0f;
    private Text txt;

    private void Start()
    {
        txt = GetComponent<Text>();
    } 

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        int fps = (int)(1.0f / deltaTime);
        //GetComponent<Text>().text = fps.ToString();
        txt.text = fps.ToString();
    }

    //void OnGUI()
    //{
    //    int w = Screen.width, h = Screen.height;
    //
    //    GUIStyle style = new GUIStyle();
    //
    //    Rect rect = new Rect(0, 0, w, h * 2 / 100);
    //    style.alignment = TextAnchor.UpperLeft;
    //    style.fontSize = h * 8 / 100;
    //    style.normal.textColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
    //    float msec = deltaTime * 1000.0f;
    //    float fps = 1.0f / deltaTime;
    //    string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    //    GUI.Label(rect, text, style);
    //}
}
