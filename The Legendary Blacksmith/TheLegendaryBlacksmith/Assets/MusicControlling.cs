using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControlling : MonoBehaviour
{

    [SerializeField]
    FadingAudioSource fadingAudio;
    [SerializeField]
    AudioClip introLoop;
    [SerializeField]
    AudioClip mainLoop;

    [SerializeField]
    AudioClip gamePlayIntro, gamePlayMain, winIntro, winMain, menuIntro, menuMain, multiplayerLobbyIntro, multiplayerLobbyMain, multiplayerGameIntro, multiplayerGameMain, lossIntro, lossMain, BossIntro, BossMain;

    AudioSource mAudio;

    [SerializeField]
    float whenFade = 2.0f;

    [SerializeField]
    float audioSourceVol = 0.3f;

    public enum MusicType
    {
        Start,
        MultiplayerLobby,
        MultiplayerGame,
        GamePlay,
        Boss,
        Win,
        Loss
    }

    bool firstClip = true;
    bool switching = false;

    public static MusicControlling instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    // Use this for initialization
    void Start()
    {
        if (fadingAudio == null)
            fadingAudio = GetComponent<FadingAudioSource>();
        mAudio = GetComponent<AudioSource>();
        mAudio.volume = audioSourceVol;
    }

    private void Update()
    {
        if (firstClip && !switching)
        {
            if (mAudio.clip.length - mAudio.time <= whenFade)
            {
                fadingAudio.Fade(mainLoop, audioSourceVol, true);
                firstClip = false;
            }
        }
        if (switching)
        {
            if (fadingAudio.GetFadeState() == FadingAudioSource.FadeState.None)
                switching = false;
        }
    }

    public void ChangeMusic(MusicType type)
    {
        switch (type)
        {
            case MusicType.Start:
                {
                    SwitchMusic(menuIntro, menuMain);
                    break;
                }

            case MusicType.MultiplayerLobby:
                {
                    SwitchMusic(multiplayerLobbyIntro, multiplayerLobbyMain);
                    break;
                }

            case MusicType.MultiplayerGame:
                {
                    SwitchMusic(multiplayerGameIntro, multiplayerGameMain);
                    break;
                }

            case MusicType.GamePlay:
                {
                    SwitchMusic(gamePlayIntro, gamePlayMain);
                    break;
                }

            case MusicType.Boss:
                {
                    SwitchMusic(BossIntro, BossMain);
                    break;
                }

            case MusicType.Win:
                {
                    SwitchMusic(winIntro, winMain);
                    break;
                }

            case MusicType.Loss:
                {
                    SwitchMusic(lossIntro, lossMain);
                    break;
                }

            default:
                break;
        }
    }

    public void SwitchMusic(AudioClip intro, AudioClip main)
    {
        introLoop = intro;
        mainLoop = main;
        fadingAudio.Fade(introLoop, audioSourceVol, true);
        firstClip = true;
        switching = true;
    }

}
