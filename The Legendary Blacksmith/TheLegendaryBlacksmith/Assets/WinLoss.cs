using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoss : MonoBehaviour
{
    Camera camMain;
    [SerializeField]
    float waitAfterSeeing = 1.0f;
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    bool Win;
    private void Start()
    {
        camMain = Camera.main;
    }
    private void OnBecameVisible()
    {
        if (Win)
            StartCoroutine(WinGame());
        else
            StartCoroutine(LoseGame());
    }
    IEnumerator WinGame()
    {
        while (true)
        {
            Ray camRay = camMain.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit camHit;
            if (Physics.Raycast(camRay, out camHit, Mathf.Infinity, mask))
            {
                if (camHit.transform.gameObject == gameObject)
                {
                    break;
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(waitAfterSeeing);
        gameObject.SetActive(false);
        SteamVR_LoadLevel.Begin("Win");
        yield return null;
    }

    IEnumerator LoseGame()
    {
        while (true)
        {
            Ray camRay = camMain.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit camHit;
            if (Physics.Raycast(camRay, out camHit,Mathf.Infinity, mask))
            {
                Debug.Log(camHit.transform.gameObject);
                if (camHit.transform.gameObject == gameObject)
                {
                    break;
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(waitAfterSeeing);
        gameObject.SetActive(false);
        SteamVR_LoadLevel.Begin("Lose");
        yield return null;
    }
}
