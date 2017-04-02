using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateToolsHold : MonoBehaviour {

    Image coRoutActivate;

    [SerializeField]
    float activateDuration = 4.0f;

    Coroutine coRout;
    bool coRoutRunning = false;

    public static ActivateToolsHold instance = null;

    public delegate void mDelegate();

    public mDelegate mDel;

    public enum MenuType
    {
        Tools,
        Structures,
        Map,
        Forfeit
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void StartActivation(bool activateBool, Image icon, mDelegate passedDelegate)
    {
        if (activateBool)
        {
            mDel = passedDelegate;
            if (coRoutRunning)
            {
                StopCoroutine(coRout);
                coRoutRunning = false;
            }
            if (coRoutActivate)
            {
                coRoutActivate.fillAmount = 0;
            }
            coRoutActivate = icon;
            coRout = StartCoroutine(Activate());
            coRoutRunning = true;
        }
        else
        {
            if (coRoutActivate == icon)
            {
                if (coRoutRunning)
                {
                    StopCoroutine(coRout);
                    coRoutRunning = false;
                }
                if (coRoutActivate)
                {
                    coRoutActivate.fillAmount = 0;
                    coRoutActivate.gameObject.SetActive(false);
                }
            }
        }
    }

    IEnumerator Activate()
    {
        coRoutActivate.gameObject.SetActive(true);
        coRoutActivate.fillAmount = 0;
        float t = 0.0f;
        float startTime = Time.time;

        while (coRoutActivate.fillAmount < 1.0f)
        {
            t = (Time.time - startTime) / activateDuration;
            coRoutActivate.fillAmount = Mathf.Lerp(0, 1.0f, t);
            yield return null;
        }
        mDel();
        coRoutActivate.fillAmount = 0;
    }
}
