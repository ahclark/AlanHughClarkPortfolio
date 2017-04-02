using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBlueprint : ButtonScrollParent
{
    [SerializeField]
    GameObject[] Swordsman;

    [SerializeField]
    GameObject[] Archer;

    [SerializeField]
    GameObject[] Tank;

    protected override void Start()
    {
        base.Start();
        Activate();
    }

    protected override void Activate()
    {
        switch (counter)
        {
            case 0:
                {
                    foreach (GameObject ar in Archer)
                        ar.SetActive(false);
                    foreach (GameObject ta in Tank)
                        ta.SetActive(false);
                    foreach (GameObject sw in Swordsman)
                        sw.SetActive(true);
                    break;

                }

            case 1:
                {
                    foreach (GameObject sw in Swordsman)
                        sw.SetActive(false);
                    foreach (GameObject ta in Tank)
                        ta.SetActive(false);
                    foreach (GameObject ar in Archer)
                        ar.SetActive(true);
                    break;
                }

            case 2:
                {
                    foreach (GameObject ar in Archer)
                        ar.SetActive(false);
                    foreach (GameObject sw in Swordsman)
                        sw.SetActive(false);
                    foreach (GameObject ta in Tank)
                        ta.SetActive(true);
                    break;
                }
            default:
                break;
        }
    }

}
