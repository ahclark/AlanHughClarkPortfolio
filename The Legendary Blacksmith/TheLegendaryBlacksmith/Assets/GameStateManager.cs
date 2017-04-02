using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager managerinstance = null;

    public enum GameState
    {
        Playing,
        Paused,
        Resuming,
        Win,
        Lose
    }

    [SerializeField]
    GameObject Win, Lose;

    [SerializeField]
    GameState mGameState;

    Image coRoutForfeit;

    [SerializeField]
    float forfeitDuration = 4.0f;

    Coroutine coRout;
    bool coRoutRunning = false;

    public bool TeleportLeft = true;

    TouchpadMenuController[] controllers;

    [SerializeField]
    GameObject m_fireworks;

    private void Awake()
    {
        if (managerinstance == null)
            managerinstance = this;
        else if (managerinstance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
        mGameState = GameState.Playing;
        if (PlayerPrefs.HasKey("Teleport"))
            TeleportLeft = PlayerPrefs.GetInt("Teleport") == 1 ? true : false;
        else
        {
            PlayerPrefs.SetInt("Teleport", TeleportLeft ? 1 : 0);
        }
    }

    public void ChangeState(GameState newState)
    {
        if (newState == mGameState)
            return;
        mGameState = newState;
        CheckState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            CheckState();
    }

    void CheckState()
    {
        switch (mGameState)
        {
            case GameState.Playing:
                {
                    if (Win)
                        Win.SetActive(false);
                    if (VendorDanceAfterWin.instance)
                        VendorDanceAfterWin.instance.Fighting();
                    if (m_fireworks)
                        m_fireworks.SetActive(false);
                    break;
                }
            case GameState.Paused:
                {
                    break;
                }
            case GameState.Resuming:
                {
                    break;
                }
            case GameState.Win:
                {
                    WinGame();
                    break;
                }
            case GameState.Lose:
                {
                    Lose.SetActive(true);
                    break;
                }
            default:
                break;
        }
    }

    public void StartForfeit(bool forfeitingBool, Image forfeit)
    {
        if (forfeitingBool)
        {
            if (coRoutRunning)
            {
                StopCoroutine(coRout);
                coRoutRunning = false;
            }
            if (coRoutForfeit)
            {
                coRoutForfeit.fillAmount = 0;
            }
            coRoutForfeit = forfeit;
            coRout = StartCoroutine(Forfeit());
            coRoutRunning = true;
        }
        else
        {
            if (coRoutForfeit == forfeit)
            {
                if (coRoutRunning)
                {
                    StopCoroutine(coRout);
                    coRoutRunning = false;
                }
                if (coRoutForfeit)
                {
                    coRoutForfeit.fillAmount = 0;
                    coRoutForfeit.gameObject.SetActive(false);
                }
            }
        }
    }

    IEnumerator Forfeit()
    {
        coRoutForfeit.gameObject.SetActive(true);
        coRoutForfeit.fillAmount = 0;
        float t = 0.0f;
        float startTime = Time.time;

        while (coRoutForfeit.fillAmount < 1.0f)
        {
            t = (Time.time - startTime) / forfeitDuration;
            coRoutForfeit.fillAmount = Mathf.Lerp(0, 1.0f, t);
            yield return null;
        }
        SteamVR_LoadLevel.Begin("Lose");
    }

    public void SetTeleportingHand(bool newHand, bool setHand = true)
    {
        if (setHand)
            TeleportLeft = newHand;
        if (Valve.VR.InteractionSystem.Teleport.instance)
            Valve.VR.InteractionSystem.Teleport.instance.SetHand(TeleportLeft);
        controllers = FindObjectsOfType(typeof(TouchpadMenuController)) as TouchpadMenuController[];
        if (controllers.Length > 0)
        {
            foreach (TouchpadMenuController control in controllers)
            {
                if (control.gameObject.activeInHierarchy)
                    control.SetHand(TeleportLeft);
            }
        }
    }

    void WinGame()
    {
        if (MusicControlling.instance)
            MusicControlling.instance.ChangeMusic(MusicControlling.MusicType.Win);
        if (DialogueManager.dialogueInstance)
            DialogueManager.dialogueInstance.Winning();
        if (Win)
            Win.SetActive(true);
        if (VendorDanceAfterWin.instance)
            VendorDanceAfterWin.instance.Winning();
        if (TroubleMaker.instance)
            TroubleMaker.instance.ActivateTroubleMaker();

        if (m_fireworks)
            m_fireworks.SetActive(true);
    }

    void LoseGame()
    {
        if (MusicControlling.instance)
            MusicControlling.instance.ChangeMusic(MusicControlling.MusicType.Loss);
        if (DialogueManager.dialogueInstance)
            DialogueManager.dialogueInstance.Losing();
        if (Lose)
            Lose.SetActive(true);
        if (TroubleMaker.instance)
            TroubleMaker.instance.ActivateTroubleMaker();
    }

    public GameState GetState()
    {
        return mGameState;
    }

    public void PlayGame()
    {
        ChangeState(GameState.Playing);
        if (MusicControlling.instance)
            MusicControlling.instance.ChangeMusic(MusicControlling.MusicType.GamePlay);
        if (DialogueManager.dialogueInstance)
            DialogueManager.dialogueInstance.BeginBattle();
        if (Win)
            Win.SetActive(false);
        if (VendorDanceAfterWin.instance)
            VendorDanceAfterWin.instance.Fighting();
        if (m_fireworks)
            m_fireworks.SetActive(false);
    }

}
