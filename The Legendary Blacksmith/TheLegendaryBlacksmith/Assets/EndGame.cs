using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public int side = 0;
    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit")
        {
            TargetFinder temp = other.GetComponent<TargetFinder>();
            if (temp)
            {
                if (side == 1 && temp.side == -1)
                {
                    GameStateManager.managerinstance.ChangeState(GameStateManager.GameState.Lose);
                }
                else if (side == -1 && temp.side == 1)
                {
                    GameStateManager.managerinstance.ChangeState(GameStateManager.GameState.Win);
                }
            }
        }
    }
}

