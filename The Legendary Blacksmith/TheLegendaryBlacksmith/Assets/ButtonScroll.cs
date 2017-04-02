using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScroll : MonoBehaviour
{

    protected enum Direction
    {
        Left,
        Right
    }

    [SerializeField]
    protected Direction dir;
    protected float timer = 0.0f;

    protected ButtonPush BP;

    [SerializeField]
    protected float timerLength = 2.0f;


    // Use this for initialization
    protected virtual void Start()
    {
        BP = GetComponent<ButtonPush>();

    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
    }
    // Update is called once per frame
    protected virtual void Update()
    {

    }
}
