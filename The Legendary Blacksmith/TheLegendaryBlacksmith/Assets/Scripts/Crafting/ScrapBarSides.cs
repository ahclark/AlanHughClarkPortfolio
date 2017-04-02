using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapBarSides : MonoBehaviour
{
    [SerializeField]
    GameObject main;
    [SerializeField]
    ScrapBar center;
    [SerializeField]
    GameObject angle;
    [SerializeField]
    GameObject hingeR;
    [SerializeField]
    GameObject hingeL;
    [SerializeField]
    GameObject other;

    [SerializeField]
    AudioSource audio;

    private void OnTriggerEnter(Collider entity)
    {
        if (entity.name == "Head" && center.CanBeHammered)
        {
            if(audio)
            AudioManager.instance.PlayCollisionSound(audio, AudioManager.AudioInteractionType.Bang, AudioManager.AudioObjectType.Metal, AudioManager.AudioObjectType.Metal);
        //    if (main.transform.rotation.x < 20 && main.transform.rotation.x > -20)
        //    {
        //        if (main.transform.rotation.z < 20 && main.transform.rotation.z > -20)
        //        {
        //            if (main.transform.rotation.y < 20 && main.transform.rotation.y > -20)
        //            {
                        if (gameObject.name == "HingeUR")
                        {
                            hingeR.transform.localRotation = angle.transform.localRotation;
                            center.downR = false;
                            center.upR = true;
                            gameObject.SetActive(false);
                            other.SetActive(false);
                        }
                        if (gameObject.name == "HingeUL")
                        {
                            hingeL.transform.localRotation = angle.transform.localRotation;
                            center.downL = false;
                            center.upL = true;
                            gameObject.SetActive(false);
                            other.SetActive(false);
                        }
        //            }
        //            if (main.transform.rotation.z < 200 && main.transform.rotation.z > 160)
        //            {
                        if (gameObject.name == "HingeDR")
                        {
                            hingeR.transform.localRotation = angle.transform.localRotation;
                            center.upR = false;
                            center.downR = true;
                            gameObject.SetActive(false);
                            other.SetActive(false);
                        }
                        if (gameObject.name == "HingeDL")
                        {
                            hingeL.transform.localRotation = angle.transform.localRotation;
                            center.upL = false;
                            center.downL = true;
                            gameObject.SetActive(false);
                            other.SetActive(false);
                        }
        //            }
        //        }
        //    }
        }
        center.HammerMe();
    }
} 

