using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicatewithDialogue : MonoBehaviour {

	public void Monocle()
    {
        if (DialogueManager.dialogueInstance)
            DialogueManager.dialogueInstance.GrabMonocle();
    }

    public void Book()
    {
        if (DialogueManager.dialogueInstance)
            DialogueManager.dialogueInstance.GrabBook();
    }
}
