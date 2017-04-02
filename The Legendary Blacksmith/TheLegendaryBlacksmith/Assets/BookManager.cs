using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BookManager : MonoBehaviour
{
    public enum PageNavigation
    {
        Forward,
        Backward
    }

    [SerializeField]
    List<GameObject> pages;

    int currPage;

    [SerializeField]
    Animator anim;

    [SerializeField]
    InvokingLinearDrive LDLeft, LDRight;

    [SerializeField]
    GameObject bookDisplay;

    Coroutine FlipCorout = null;

    [SerializeField]
    BoxCollider bookClosed;

    [SerializeField]
    BoxCollider[] bookOpen;

    Coroutine teleportCorout, closeBookCorout;

    [SerializeField]
    Transform bookPedestal;

    [SerializeField]
    ParticleSystem sparkleMcSparkle;

    [SerializeField]
    float TimeUntilClose = 5.0f, TimeUntilTeleport = 5.0f, SparkleTime = 3.0f;

    [SerializeField]
    GameObject BookParent;

    [SerializeField]
    AudioClip closeBook, turnPage, turnPages;

    AudioSource mAudio;


    // Use this for initialization
    void Start()
    {
        if (!anim)
            anim = GetComponent<Animator>();
        currPage = 0;
        pages[currPage].SetActive(true);

        mAudio = GetComponent<AudioSource>();
    }

    public void FlipPage(bool forward)
    {
        if (forward)
        {
            if (currPage + 1 < pages.Capacity)
            {
                pages[currPage].SetActive(false);
                currPage++;
                anim.SetTrigger("FlipRight");
            }
        }
        else
        {

            if (currPage - 1 >= 0)
            {
                pages[currPage].SetActive(false);
                currPage--;
                anim.SetTrigger("FlipLeft");
            }
        }
    }

    public void SetCurrentPage()
    {
        pages[currPage].SetActive(true);
    }

    public void FlipToPage(GameObject page)
    {
        if (page)
        {
            if (pages.Contains(page))
            {
                pages[currPage].SetActive(false);
                currPage = pages.IndexOf(page);
                anim.SetTrigger("FlipMany");
            }
        }
    }

    public void StartCheckingFlipping(bool left)
    {
        if (FlipCorout != null)
            StopCoroutine(FlipCorout);
        FlipCorout = StartCoroutine(CheckFlipping(left));
    }

    public void StopCheckingFlipping(bool left)
    {
        if (left)
        {
            if (LDLeft)
            {
                if (FlipCorout != null)
                    StopCoroutine(FlipCorout);
                LDLeft.linearMapping.value = 0.0f;
            }
        }
        else
        {
            if (LDRight)
            {
                if (FlipCorout != null)
                    StopCoroutine(FlipCorout);
                LDRight.linearMapping.value = 0.0f;
            }
        }
    }

    IEnumerator CheckFlipping(bool left)
    {
        if (left)
        {
            while (true)
            {
                if (LDLeft.linearMapping.value > 0.1f)
                {
                    FlipPage(true);
                    break;
                }
                yield return null;
            }
        }

        else
        {
            while (true)
            {
                if (LDRight.linearMapping.value > 0.1f)
                {
                    FlipPage(false);
                    break;
                }
                yield return null;
            }
        }
    }

    public void ActivateBookDisplay()
    {
        if (!bookDisplay.activeInHierarchy)
            bookDisplay.SetActive(true);

    }

    public void ActivateOpenColliders()
    {
        if (bookOpen.Length > 0)
        {
            foreach (BoxCollider coll in bookOpen)
                coll.enabled = true;
        }
        if (bookClosed)
            bookClosed.enabled = false;

        PlayFlipMultipleSound();

        sparkleMcSparkle.gameObject.SetActive(false);

    }


    public void DeActivateBookDisplay()
    {
        if (bookDisplay.activeInHierarchy)
            bookDisplay.SetActive(false);

    }

    public void DeActivateOpenColliders()
    {
        if (bookOpen.Length > 0)
        {
            foreach (BoxCollider coll in bookOpen)
                coll.enabled = false;
        }
        if (bookClosed)
            bookClosed.enabled = true;
        if (closeBook)
            mAudio.PlayOneShot(closeBook);
        sparkleMcSparkle.gameObject.SetActive(true);
    }
    public void CloseBook()
    {
        if (closeBookCorout != null)
            StopCoroutine(closeBookCorout);
        closeBookCorout = StartCoroutine(CloseBookCoRout());
    }

    public void OpenBook()
    {
        if (closeBookCorout != null)
            StopCoroutine(closeBookCorout);
        if (!anim.GetBool("Open"))
            anim.SetBool("Open", true);
    }

    public void StartTeleport()
    {
        if (teleportCorout != null)
            StopCoroutine(teleportCorout);
        teleportCorout = StartCoroutine(TeleportCauseREASONS());

    }

    public void StopTeleport()
    {
        if (teleportCorout != null)
            StopCoroutine(teleportCorout);
        if (sparkleMcSparkle.isPlaying)
            sparkleMcSparkle.Stop(true);
    }

    IEnumerator TeleportCauseREASONS()
    {
        yield return new WaitForSeconds(TimeUntilTeleport);
        sparkleMcSparkle.Play(true);
        yield return new WaitForSeconds(SparkleTime);
        BookParent.transform.position = bookPedestal.position;
        BookParent.transform.rotation = bookPedestal.rotation;
        sparkleMcSparkle.Stop(true);
    }

    IEnumerator CloseBookCoRout()
    {
        yield return new WaitForSeconds(TimeUntilClose);
        anim.SetBool("Open", false);
    }

    public void PlayFlipOneSound()
    {

        if (turnPage)
            mAudio.PlayOneShot(turnPage);
    }

    public void PlayFlipMultipleSound()
    {
        if (turnPages)
            mAudio.PlayOneShot(turnPages);
    }

}
