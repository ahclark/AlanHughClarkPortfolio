using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBluePrintScroll : ButtonScroll 
{
    [SerializeField]
    UnitBlueprint bluePrint;


    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
    }

     protected override void Update()
    {
        if(BP.buttonOn && timer > timerLength)
        {
            if (dir == Direction.Left)
                bluePrint.ScrollLeft();

            if (dir == Direction.Right)
                bluePrint.ScrollRight();

            timer = 0;
        }
    }

}
