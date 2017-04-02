using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashPortal : MonoBehaviour
{
    Renderer[] FullObjectRenderers;
    Renderer mRend;
    [SerializeField]
    Color originalColor;
    [SerializeField]
    Color newColor;
    [SerializeField]
    float timeofFlash = 0.2f;

    public bool flashing = false;
    IEnumerator co;
    private void Start()
    {
        //co = FlashCoroutine();
        FullObjectRenderers = gameObject.GetComponentsInChildren<Renderer>();
        FullObjectRenderers[0].material.SetColor("_TintColor", originalColor);
    }


    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        flashing = true;
        foreach (Renderer rend in FullObjectRenderers)
        {
            rend.material.SetColor("_TintColor", newColor);
        }

        yield return new WaitForSeconds(timeofFlash);

        foreach (Renderer rend in FullObjectRenderers)
        {
            rend.material.SetColor("_TintColor", originalColor);
        }
        flashing = false;
    }

    public void Set(bool name)
    {
        if (name)
        {
            foreach (Renderer rend in FullObjectRenderers)
                rend.material.SetColor("_TintColor", newColor);
        }
        else
        {
            foreach (Renderer rend in FullObjectRenderers)
                rend.material.SetColor("_TintColor", originalColor);
        }
    }
}
