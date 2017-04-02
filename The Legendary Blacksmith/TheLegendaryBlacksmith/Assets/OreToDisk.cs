using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreToDisk : MonoBehaviour {

    [SerializeField]
    GameObject Disk;
    [SerializeField]
    string DiskCollider;

    bool once = false;



    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.name == DiskCollider && !once)
        {
            //plug our reaction function into ScrapPress.OnPressBottom();
            ScrapPress.OnPressBottom += DiskReaction;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == DiskCollider && !once)
        {
            //plug our reaction function into ScrapPress.OnPressBottom();
            ScrapPress.OnPressBottom -= DiskReaction;
        }
    }



    void DiskReaction()
    {
        if (!once)
        {
            Instantiate(Disk, transform.position, transform.rotation);
            ScrapPress.OnPressBottom -= DiskReaction;
            Destroy(gameObject);
            once = true;
        }
    }
}
