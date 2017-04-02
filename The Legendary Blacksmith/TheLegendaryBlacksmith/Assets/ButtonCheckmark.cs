using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;


public class ButtonCheckmark : MonoBehaviour
{
    public UnityEvent action;
    public bool box_checked = true;
    [SerializeField]
    Image checkMark;
    string fileName;
    [SerializeField]
    string FILE_NAME;
    StreamWriter fileWriter;
    StreamReader fileReader;
    [SerializeField]
    GameObject checked_text, unchecked_text;
    private void Awake()
    {
        //fileName = Application.persistentDataPath + "/" + FILE_NAME;
        //Debug.Log(fileName);
        //if (File.Exists(fileName))
        //{
        //    fileReader = File.OpenText(fileName);
        //    string temp = fileReader.ReadLine();
        //    Debug.Log(temp);
        //    if (temp == "False")
        //    {
        //        box_checked = false;
        //    }
        //    else
        //        box_checked = true;
        //    fileReader.Close();
        //}
        //else
        //{
        //    fileWriter = File.CreateText(fileName);
        //    fileWriter.WriteLine(box_checked.ToString());
        //    fileWriter.Close();
        //}

        if (PlayerPrefs.HasKey(FILE_NAME))
            box_checked = PlayerPrefs.GetInt(FILE_NAME) == 1 ? true : false;
        else
        {
            if (GameStateManager.managerinstance)
                box_checked = GameStateManager.managerinstance;
            PlayerPrefs.SetInt(FILE_NAME, box_checked ? 1 : 0);
        }
        BoxCheck(box_checked);
    }

    public void BoxCheck()
    {
        box_checked = !box_checked;
        checkMark.enabled = box_checked;
        if (checked_text)
            checked_text.SetActive(box_checked);
        if (unchecked_text)
            unchecked_text.SetActive(!box_checked);

        action.Invoke();
    }

    public void BoxCheck(bool newBox)
    {
        box_checked = newBox;
        checkMark.enabled = box_checked;
        if (checked_text)
            checked_text.SetActive(box_checked);
        if (unchecked_text)
            unchecked_text.SetActive(!box_checked);
        action.Invoke();
    }

    public void SelectHand()
    {
        //FileStream fcreate = File.Open(fileName, FileMode.Create);
        //fileWriter = new StreamWriter(fcreate);
        //fileWriter.WriteLine(box_checked.ToString());
        //fileWriter.Close();
        PlayerPrefs.SetInt(FILE_NAME, box_checked ? 1 : 0);
        if (GameStateManager.managerinstance)
            GameStateManager.managerinstance.SetTeleportingHand(box_checked);
        //Teleport.instance.SetHand(box_checked);
    }
}
