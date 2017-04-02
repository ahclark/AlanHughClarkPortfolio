using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class SteamIDEngraving : MonoBehaviour
{
    [SerializeField]
    private GameObject Text, RuneText;

    void Start()
    {
        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();

            Text.GetComponent<Text>().text = name;
            RuneText.GetComponent<Text>().text = name;
        }
    }
}
