using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour {
    //various game objects for quick access
    [Header("Important Objects And Structures")]
    public LaneController[] lanes;
    public BlacksmithBaseController redBase, blueBase;


    //keeping state info for all to know about
    [Header("Game State Info")]
    public bool nightTime = false;


    private void Start()
    {
        DayNightCycle.NightTimeSwitch += NightTime;
    }

    void NightTime(bool _night)
    {
        nightTime = _night;
    }


    //If enemy about to capture "base outpost"
    //if (DialogueManager.dialogueInstance)
    //DialogueManager.dialogueInstance.EnemyNearingBase();

    //If friendlies about to capture "enemy base outpost"
    //if (DialogueManager.dialogueInstance)
    //DialogueManager.dialogueInstance.FriendlyNearingEnemy();

    //If Enemy finishes capturing a point
    //if (DialogueManager.dialogueInstance)
    //DialogueManager.dialogueInstance.EnemyTakesPoint();

    //If Friendly finishes capturing a point
    //if (DialogueManager.dialogueInstance)
    //DialogueManager.dialogueInstance.FriendlyTakesPoint();
}
