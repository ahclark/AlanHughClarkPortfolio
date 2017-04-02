using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [SerializeField]
    VolumeControl volControl;

    public float masterVol, sfxVol, ambVol, musicVol, voiceVol;

    public enum AudioObjectType
    {
        Wood,
        Metal,
        Rock,
        Terrain,
        Unit,
        Building
    }

    public enum AudioInteractionType
    {
        Thud,
        Bang
    }

    [SerializeField]
    AudioClip[] WoodWoodThud, WoodWoodBang, WoodMetalThud, WoodMetalBang, WoodRockThud, WoodRockBang, WoodTerrainThud, WoodTerrainBang,
                WoodUnitThud, WoodUnitBang, WoodBuildingThud, WoodBuildingBang;

    [SerializeField]
    AudioClip[] MetalMetalThud, MetalMetalBang, MetalRockThud, MetalRockBang, MetalTerrainThud, MetalTerrainBang,
                MetalUnitThud, MetalUnitBang, MetalBuildingThud, MetalBuildingBang;

    [SerializeField]
    AudioClip[] RockRockThud, RockRockBang, RockTerrainThud, RockTerrainBang, RockUnitThud, RockUnitBang, RockBuildingThud, RockBuildingBang;

    [SerializeField]
    AudioClip[] TerrainTerrainThud, TerrainTerrainBang, TerrainUnitThud, TerrainUnitBang, TerrainBuildingThud, TerrainBuildingBang;

    [SerializeField]
    AudioClip[] UnitUnitThud, UnitUnitBang, UnitBuildingThud, UnitBuildingBang;

    [SerializeField]
    AudioClip[] BuildingBuildingThud, BuildingBuildingBang;

    [SerializeField]
    AudioClip WoodGrinding, MetalGrinding, RockGrinding;

    [SerializeField]
    AudioClip WoodSlicing, MetalSlicing, RockSlicing;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);

        #region ErasePrefs
        //PlayerPrefs.DeleteAll();
        #endregion

        #region Master
        //Set up Master Volume
        if (PlayerPrefs.HasKey("masterVol"))
            masterVol = PlayerPrefs.GetFloat("masterVol");
        else
        {
            masterVol = .5f;
            PlayerPrefs.SetFloat("masterVol", masterVol);
        }
        #endregion

        #region Effects
        //Set up Effects Volume
        if (PlayerPrefs.HasKey("sfxVol"))
            sfxVol = PlayerPrefs.GetFloat("sfxVol");
        else
        {
            sfxVol = .5f;
            PlayerPrefs.SetFloat("sfxVol", sfxVol);
        }
        #endregion

        #region Ambience
        //Set up Ambience Volume
        if (PlayerPrefs.HasKey("ambVol"))
            ambVol = PlayerPrefs.GetFloat("ambVol");
        else
        {
            ambVol = .5f;
            PlayerPrefs.SetFloat("ambVol", ambVol);
        }
        #endregion

        #region Music
        //Set up Music Volume
        if (PlayerPrefs.HasKey("musicVol"))
            musicVol = PlayerPrefs.GetFloat("musicVol");
        else
        {
            musicVol = .5f;
            PlayerPrefs.SetFloat("musicVol", musicVol);
        }
        #endregion

        #region Voice
        //Set up Voice Volume
        if (PlayerPrefs.HasKey("voiceVol"))
            voiceVol = PlayerPrefs.GetFloat("voiceVol");
        else
        {
            voiceVol = .5f;
            PlayerPrefs.SetFloat("voiceVol", voiceVol);
        }
        #endregion
    }
    private void Start()
    {
        #region Master
        volControl.SetVolume(VolumeControl.VolumeControls.Master, masterVol);
        #endregion

        #region Effects
        //Set up Effects Volume
        volControl.SetVolume(VolumeControl.VolumeControls.SFX, sfxVol, false);
        #endregion

        #region Ambience
        //Set up Ambience Volume
        volControl.SetVolume(VolumeControl.VolumeControls.Ambience, ambVol, false);
        #endregion

        #region Music
        //Set up Music Volume
        volControl.SetVolume(VolumeControl.VolumeControls.Music, musicVol, false);
        #endregion

        #region Voice
        //Set up Voice Volume
        volControl.SetVolume(VolumeControl.VolumeControls.Voice, voiceVol, false);
        #endregion
    }

    public void SaveVolume(VolumeControl.VolumeControls type, float value)
    {
        switch (type)
        {
            case VolumeControl.VolumeControls.Master:
                {
                    masterVol = value;
                    PlayerPrefs.SetFloat("masterVol", masterVol);
                    break;
                }
            case VolumeControl.VolumeControls.SFX:
                {
                    sfxVol = value;
                    PlayerPrefs.SetFloat("sfxVol", sfxVol);
                    break;
                }
            case VolumeControl.VolumeControls.Music:
                {
                    musicVol = value;
                    PlayerPrefs.SetFloat("musicVol", musicVol);
                    break;
                }
            case VolumeControl.VolumeControls.Ambience:
                {
                    ambVol = value;
                    PlayerPrefs.SetFloat("ambVol", ambVol);
                    break;
                }
            case VolumeControl.VolumeControls.Voice:
                {
                    voiceVol = value;
                    PlayerPrefs.SetFloat("voiceVol", voiceVol);
                    break;
                }
            default:
                break;
        }
    }

    public float GetVolume(VolumeControl.VolumeControls type)
    {
        switch (type)
        {
            case VolumeControl.VolumeControls.Master:
                {
                    return masterVol;
                }
            case VolumeControl.VolumeControls.SFX:
                {
                    return sfxVol;
                }
            case VolumeControl.VolumeControls.Music:
                {
                    return musicVol;
                }
            case VolumeControl.VolumeControls.Ambience:
                {
                    return ambVol;
                }
            case VolumeControl.VolumeControls.Voice:
                {
                    return voiceVol;
                }
            default:
                {
                    return -1;
                }
        }
    }

    public VolumeControl GetVolController()
    {
        return volControl;
    }

    public void PlayCollisionSound(AudioSource objCalling, AudioInteractionType objCallingInteraction,
                                    AudioObjectType objCallingType = AudioObjectType.Building, AudioObjectType objCollidedType = AudioObjectType.Building)
    {
        switch (objCallingInteraction)
        {
            #region Thud
            case AudioInteractionType.Thud:
                {
                    switch (objCallingType)
                    {
                        #region WoodObj
                        case AudioObjectType.Wood:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodWoodThud.Length > 0)
                                                objCalling.PlayOneShot(WoodWoodThud[Random.Range(0, WoodWoodThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (WoodMetalThud.Length > 0)
                                                objCalling.PlayOneShot(WoodMetalThud[Random.Range(0, WoodMetalThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (WoodRockThud.Length > 0)
                                                objCalling.PlayOneShot(WoodRockThud[Random.Range(0, WoodRockThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (WoodTerrainThud.Length > 0)
                                                objCalling.PlayOneShot(WoodTerrainThud[Random.Range(0, WoodTerrainThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (WoodUnitThud.Length > 0)
                                                objCalling.PlayOneShot(WoodUnitThud[Random.Range(0, WoodUnitThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (WoodBuildingThud.Length > 0)
                                                objCalling.PlayOneShot(WoodBuildingThud[Random.Range(0, WoodBuildingThud.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion
                        #region MetalObj
                        case AudioObjectType.Metal:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodMetalThud.Length > 0)
                                                objCalling.PlayOneShot(WoodMetalThud[Random.Range(0, WoodMetalThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (MetalMetalThud.Length > 0)
                                                objCalling.PlayOneShot(MetalMetalThud[Random.Range(0, MetalMetalThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (MetalRockThud.Length > 0)
                                                objCalling.PlayOneShot(MetalRockThud[Random.Range(0, MetalRockThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (MetalTerrainThud.Length > 0)
                                                objCalling.PlayOneShot(MetalTerrainThud[Random.Range(0, MetalTerrainThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (MetalUnitThud.Length > 0)
                                                objCalling.PlayOneShot(MetalUnitThud[Random.Range(0, MetalUnitThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (MetalBuildingThud.Length > 0)
                                                objCalling.PlayOneShot(MetalBuildingThud[Random.Range(0, MetalBuildingThud.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion
                        #region RockObj
                        case AudioObjectType.Rock:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodRockThud.Length > 0)
                                                objCalling.PlayOneShot(WoodRockThud[Random.Range(0, WoodRockThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (MetalRockThud.Length > 0)
                                                objCalling.PlayOneShot(MetalRockThud[Random.Range(0, MetalRockThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (RockRockThud.Length > 0)
                                                objCalling.PlayOneShot(RockRockThud[Random.Range(0, RockRockThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (RockTerrainThud.Length > 0)
                                                objCalling.PlayOneShot(RockTerrainThud[Random.Range(0, RockTerrainThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (RockUnitThud.Length > 0)
                                                objCalling.PlayOneShot(RockUnitThud[Random.Range(0, RockUnitThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (RockBuildingThud.Length > 0)
                                                objCalling.PlayOneShot(RockBuildingThud[Random.Range(0, RockBuildingThud.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion
                        #region TerrainObj
                        case AudioObjectType.Terrain:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodTerrainThud.Length > 0)
                                                objCalling.PlayOneShot(WoodTerrainThud[Random.Range(0, WoodTerrainThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (MetalTerrainThud.Length > 0)
                                                objCalling.PlayOneShot(MetalTerrainThud[Random.Range(0, MetalTerrainThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (RockTerrainThud.Length > 0)
                                                objCalling.PlayOneShot(RockTerrainThud[Random.Range(0, RockTerrainThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (TerrainTerrainThud.Length > 0)
                                                objCalling.PlayOneShot(TerrainTerrainThud[Random.Range(0, TerrainTerrainThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (TerrainUnitThud.Length > 0)
                                                objCalling.PlayOneShot(TerrainUnitThud[Random.Range(0, TerrainUnitThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (TerrainBuildingThud.Length > 0)
                                                objCalling.PlayOneShot(TerrainBuildingThud[Random.Range(0, TerrainBuildingThud.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion
                        #region UnitObj
                        case AudioObjectType.Unit:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodUnitThud.Length > 0)
                                                objCalling.PlayOneShot(WoodUnitThud[Random.Range(0, WoodUnitThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (MetalUnitThud.Length > 0)
                                                objCalling.PlayOneShot(MetalUnitThud[Random.Range(0, MetalUnitThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (RockUnitThud.Length > 0)
                                                objCalling.PlayOneShot(RockUnitThud[Random.Range(0, RockUnitThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (TerrainUnitThud.Length > 0)
                                                objCalling.PlayOneShot(TerrainUnitThud[Random.Range(0, TerrainUnitThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (UnitUnitThud.Length > 0)
                                                objCalling.PlayOneShot(UnitUnitThud[Random.Range(0, UnitUnitThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (UnitBuildingThud.Length > 0)
                                                objCalling.PlayOneShot(UnitBuildingThud[Random.Range(0, UnitBuildingThud.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion
                        #region BuildingObj
                        case AudioObjectType.Building:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodBuildingThud.Length > 0)
                                                objCalling.PlayOneShot(WoodBuildingThud[Random.Range(0, WoodBuildingThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (MetalBuildingThud.Length > 0)
                                                objCalling.PlayOneShot(MetalBuildingThud[Random.Range(0, MetalBuildingThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (RockBuildingThud.Length > 0)
                                                objCalling.PlayOneShot(RockBuildingThud[Random.Range(0, RockBuildingThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (TerrainBuildingThud.Length > 0)
                                                objCalling.PlayOneShot(TerrainBuildingThud[Random.Range(0, TerrainBuildingThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (UnitBuildingThud.Length > 0)
                                                objCalling.PlayOneShot(UnitBuildingThud[Random.Range(0, UnitBuildingThud.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (BuildingBuildingThud.Length > 0)
                                                objCalling.PlayOneShot(BuildingBuildingThud[Random.Range(0, BuildingBuildingThud.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                            #endregion
                    }
                    break;
                }
            #endregion
            #region Bang
            case AudioInteractionType.Bang:
                {
                    switch (objCallingType)
                    {
                        #region WoodObj
                        case AudioObjectType.Wood:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodWoodBang.Length > 0)
                                                objCalling.PlayOneShot(WoodWoodBang[Random.Range(0, WoodWoodBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (WoodMetalBang.Length > 0)
                                                objCalling.PlayOneShot(WoodMetalBang[Random.Range(0, WoodMetalBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (WoodRockBang.Length > 0)
                                                objCalling.PlayOneShot(WoodRockBang[Random.Range(0, WoodRockBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (WoodTerrainBang.Length > 0)
                                                objCalling.PlayOneShot(WoodTerrainBang[Random.Range(0, WoodTerrainBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (WoodUnitBang.Length > 0)
                                                objCalling.PlayOneShot(WoodUnitBang[Random.Range(0, WoodUnitBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (WoodBuildingBang.Length > 0)
                                                objCalling.PlayOneShot(WoodBuildingBang[Random.Range(0, WoodBuildingBang.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion
                        #region MetalObj
                        case AudioObjectType.Metal:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodMetalBang.Length > 0)
                                                objCalling.PlayOneShot(WoodMetalBang[Random.Range(0, WoodMetalBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (MetalMetalBang.Length > 0)
                                                objCalling.PlayOneShot(MetalMetalBang[Random.Range(0, MetalMetalBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (MetalRockBang.Length > 0)
                                                objCalling.PlayOneShot(MetalRockBang[Random.Range(0, MetalRockBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (MetalTerrainBang.Length > 0)
                                                objCalling.PlayOneShot(MetalTerrainBang[Random.Range(0, MetalTerrainBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (MetalUnitBang.Length > 0)
                                                objCalling.PlayOneShot(MetalUnitBang[Random.Range(0, MetalUnitBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (MetalBuildingBang.Length > 0)
                                                objCalling.PlayOneShot(MetalBuildingBang[Random.Range(0, MetalBuildingBang.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion
                        #region RockObj
                        case AudioObjectType.Rock:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodRockBang.Length > 0)
                                                objCalling.PlayOneShot(WoodRockBang[Random.Range(0, WoodRockBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (MetalRockBang.Length > 0)
                                                objCalling.PlayOneShot(MetalRockBang[Random.Range(0, MetalRockBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (RockRockBang.Length > 0)
                                                objCalling.PlayOneShot(RockRockBang[Random.Range(0, RockRockBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (RockTerrainBang.Length > 0)
                                                objCalling.PlayOneShot(RockTerrainBang[Random.Range(0, RockTerrainBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (RockUnitBang.Length > 0)
                                                objCalling.PlayOneShot(RockUnitBang[Random.Range(0, RockUnitBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (RockBuildingBang.Length > 0)
                                                objCalling.PlayOneShot(RockBuildingBang[Random.Range(0, RockBuildingBang.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion
                        #region TerrainObj
                        case AudioObjectType.Terrain:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodTerrainBang.Length > 0)
                                                objCalling.PlayOneShot(WoodTerrainBang[Random.Range(0, WoodTerrainBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (MetalTerrainBang.Length > 0)
                                                objCalling.PlayOneShot(MetalTerrainBang[Random.Range(0, MetalTerrainBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (RockTerrainBang.Length > 0)
                                                objCalling.PlayOneShot(RockTerrainBang[Random.Range(0, RockTerrainBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (TerrainTerrainBang.Length > 0)
                                                objCalling.PlayOneShot(TerrainTerrainBang[Random.Range(0, TerrainTerrainBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (TerrainUnitBang.Length > 0)
                                                objCalling.PlayOneShot(TerrainUnitBang[Random.Range(0, TerrainUnitBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (TerrainBuildingBang.Length > 0)
                                                objCalling.PlayOneShot(TerrainBuildingBang[Random.Range(0, TerrainBuildingBang.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion
                        #region UnitObj
                        case AudioObjectType.Unit:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodUnitBang.Length > 0)
                                                objCalling.PlayOneShot(WoodUnitBang[Random.Range(0, WoodUnitBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (MetalUnitBang.Length > 0)
                                                objCalling.PlayOneShot(MetalUnitBang[Random.Range(0, MetalUnitBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (RockUnitBang.Length > 0)
                                                objCalling.PlayOneShot(RockUnitBang[Random.Range(0, RockUnitBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (TerrainUnitBang.Length > 0)
                                                objCalling.PlayOneShot(TerrainUnitBang[Random.Range(0, TerrainUnitBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (UnitUnitBang.Length > 0)
                                                objCalling.PlayOneShot(UnitUnitBang[Random.Range(0, UnitUnitBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (UnitBuildingBang.Length > 0)
                                                objCalling.PlayOneShot(UnitBuildingBang[Random.Range(0, UnitBuildingBang.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion
                        #region BuildingObj
                        case AudioObjectType.Building:
                            {
                                switch (objCollidedType)
                                {
                                    case AudioObjectType.Wood:
                                        {
                                            if (WoodBuildingBang.Length > 0)
                                                objCalling.PlayOneShot(WoodBuildingBang[Random.Range(0, WoodBuildingBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Metal:
                                        {
                                            if (MetalBuildingBang.Length > 0)
                                                objCalling.PlayOneShot(MetalBuildingBang[Random.Range(0, MetalBuildingBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Rock:
                                        {
                                            if (RockBuildingBang.Length > 0)
                                                objCalling.PlayOneShot(RockBuildingBang[Random.Range(0, RockBuildingBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Terrain:
                                        {
                                            if (TerrainBuildingBang.Length > 0)
                                                objCalling.PlayOneShot(TerrainBuildingBang[Random.Range(0, TerrainBuildingBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Unit:
                                        {
                                            if (UnitBuildingBang.Length > 0)
                                                objCalling.PlayOneShot(UnitBuildingBang[Random.Range(0, UnitBuildingBang.Length - 1)]);
                                            break;
                                        }
                                    case AudioObjectType.Building:
                                        {
                                            if (BuildingBuildingBang.Length > 0)
                                                objCalling.PlayOneShot(BuildingBuildingBang[Random.Range(0, BuildingBuildingBang.Length - 1)]);
                                            break;
                                        }
                                }
                                break;
                            }
                            #endregion
                    }
                    break;
                }
            #endregion

            default:
                break;
        }
    }

    public void PlayGrindingSound(AudioSource objCalling, AudioObjectType objCallingType, bool loop)
    {
        switch (objCallingType)
        {
            #region Wood
            case AudioObjectType.Wood:
                {
                    objCalling.clip = WoodGrinding;
                    objCalling.loop = loop;
                    objCalling.Play();
                    break;
                }
            #endregion
            #region Metal
            case AudioObjectType.Metal:
                {
                    objCalling.clip = MetalGrinding;
                    objCalling.loop = loop;
                    objCalling.Play();
                    break;
                }
            #endregion
            #region Rock
            case AudioObjectType.Rock:
                {
                    objCalling.clip = RockGrinding;
                    objCalling.loop = loop;
                    objCalling.Play();
                    break;
                }
            #endregion

            default:
                break;
        }
    }

    public void PlaySlicingSound(AudioSource objCalling, AudioObjectType objCallingType)
    {
        switch (objCallingType)
        {
            #region Wood
            case AudioObjectType.Wood:
                {
                    objCalling.clip = WoodSlicing;
                    objCalling.loop = true;
                    objCalling.Play();
                    break;
                }
            #endregion
            #region Metal
            case AudioObjectType.Metal:
                {
                    objCalling.clip = MetalSlicing;
                    objCalling.loop = true;
                    objCalling.Play();
                    break;
                }
            #endregion
            #region Rock
            case AudioObjectType.Rock:
                {
                    objCalling.clip = RockSlicing;
                    objCalling.loop = true;
                    objCalling.Play();
                    break;
                }
            #endregion

            default:
                break;
        }
    }

    public void StopGrinding_SlicingSound(AudioSource objCalling)
    {
        objCalling.clip = null;
        objCalling.loop = false;
        objCalling.Stop();
    }
}
