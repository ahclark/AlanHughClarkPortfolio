using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCraftables : MonoBehaviour {

    [SerializeField]
    GameObject HorseShoe;
    [SerializeField]
    string HorseShoeCollider;

    bool once = false;



    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.name == HorseShoeCollider && !once)
        {
            //plug our reaction function into ScrapPress.OnPressBottom();
            ScrapPress.OnPressBottom += HorseShoeReaction;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == HorseShoeCollider && !once)
        {
            //plug our reaction function into ScrapPress.OnPressBottom();
            ScrapPress.OnPressBottom -= HorseShoeReaction;
        }
    }



    void HorseShoeReaction()
    {
        if (!once)
        {
            GameObject toChange = Instantiate(HorseShoe, transform.position, transform.rotation);
            toChange.name = HorseShoe.name;
            if (DialogueManager.dialogueInstance)
                DialogueManager.dialogueInstance.ActivateNewWeapon(HorseShoe.name);
            ScrapPress.OnPressBottom -= HorseShoeReaction;
            once = true;
            gameObject.SetActive(false);
            Destroy(gameObject);

        }
    }

}
