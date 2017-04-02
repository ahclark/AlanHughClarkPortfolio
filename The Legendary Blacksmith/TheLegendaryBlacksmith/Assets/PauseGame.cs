using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [SerializeField]
    GameObject pauseHolder;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    float pauseMenuOffset = 1.0f;

    [SerializeField]
    Outline mOutline;

    [SerializeField]
    float flashDuration = 0.2f;

    bool paused = false;

    private void Start()
    {
        StartCoroutine(ShowHintatStart());
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Activate();
    }
#endif

    public void Activate()
    {
        if (!paused)
        {
            GameStateManager.managerinstance.ChangeState(GameStateManager.GameState.Paused);
            pauseHolder.SetActive(true);

            pauseMenu.GetComponent<MainMenuScript>().SetUpPanels();

            //Ray camRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //Vector3 temp = camRay.GetPoint(pauseMenuOffset);
            //temp.y = pauseHolder.transform.position.y;
            //pauseHolder.transform.position = temp;
            //
            //
            Vector3 tempLooking = 2 * pauseHolder.transform.position - Camera.main.transform.position;
            tempLooking.y = this.transform.position.y;
            pauseHolder.transform.LookAt(tempLooking);

            StartCoroutine(FlashOutline());

        }
        else
        {
            GameStateManager.managerinstance.ChangeState(GameStateManager.GameState.Resuming);
            pauseHolder.SetActive(false);
        }
        paused = !paused;
    }

    IEnumerator ShowHintatStart()
    {
        yield return new WaitForSeconds(5.0f);
        foreach (Hand hand in Player.instance.hands)
        {
            ControllerButtonHints.ShowTextHint(hand, Valve.VR.EVRButtonId.k_EButton_ApplicationMenu, "Menu");
        }
        yield return new WaitForSeconds(5.0f);
        foreach(Hand hand in Player.instance.hands)
        {
            ControllerButtonHints.HideTextHint(hand, Valve.VR.EVRButtonId.k_EButton_ApplicationMenu);
            if (ControllerButtonHints.IsButtonHintActive(hand, Valve.VR.EVRButtonId.k_EButton_ApplicationMenu))
                ControllerButtonHints.HideButtonHint(hand, Valve.VR.EVRButtonId.k_EButton_ApplicationMenu);
        }
        yield return null;
    }

    IEnumerator FlashOutline()
    {
        if(mOutline)
        {
            yield return new WaitForSeconds(flashDuration);
            mOutline.enabled = true;
            yield return new WaitForSeconds(flashDuration);
            mOutline.enabled = false;
        }
        yield return null;
    }
}
