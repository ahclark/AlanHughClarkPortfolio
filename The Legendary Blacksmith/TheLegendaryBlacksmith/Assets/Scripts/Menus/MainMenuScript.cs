using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Valve.VR.InteractionSystem;
public class MainMenuScript : MonoBehaviour {

    enum Mode
    {
        Play,
        Options,
        Restart,
        MainMenu,
        Exit
    }
    [Tooltip("The first panel in the array must always be the main panel")] 
    [SerializeField]
    GameObject[] Panels;
    public string Scene;
    public GameObject m_object, other_object;
    float timer = 0;
    [SerializeField]
    Mode buttonMode;
	// Use this for initialization
    public void Play()
    {
        //SceneManager.LoadSceneAsync(Scene);
        SteamVR_LoadLevel.Begin(Scene);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void SwitchPanels()
    {
        if(m_object != null && other_object != null)
        {
            for(int i = 0; i < Player.instance.handCount; ++i)
            {
                if (Player.instance.hands[i].currentAttachedObject)
                    Player.instance.hands[i].DetachObject(Player.instance.hands[i].currentAttachedObject, true);
            }
            other_object.SetActive(true);
            m_object.SetActive(false);
        }
    }

    public void Activate()
    {
        switch (buttonMode)
        {
            case Mode.Play:
                {
                    Play();
                    break;
                }
            case Mode.Options:
                {
                    SwitchPanels();
                    break;
                }
            case Mode.Restart:
                {
                    Play();
                    break;
                }
            case Mode.Exit:
                {
                    Exit();
                    break;
                }
            case Mode.MainMenu:
                {
                    if (GameStateManager.managerinstance)
                        GameStateManager.managerinstance.ChangeState(GameStateManager.GameState.Playing);
                    SteamVR_LoadLevel.Begin("Main Menu");
                    break;
                }
            default:
                break;
        }
    }

    public void SetUpPanels()
    {
        foreach(GameObject panel in Panels)
        {
            panel.SetActive(false);
        }
        Panels[0].SetActive(true);
    }

}
