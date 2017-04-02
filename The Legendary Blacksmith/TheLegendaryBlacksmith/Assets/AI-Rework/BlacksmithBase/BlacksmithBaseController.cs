using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithBaseController : MonoBehaviour {

    public bool openToAttack = false;
    public float health = 10;
    public GameObject[] HealthBarPieces = new GameObject[10];
    public int side = 1;


    public void AttackMe(float damage)
    {
        health -= 1;
        if(health >= 0)
            Destroy(HealthBarPieces[(int)health]);

        if (health <= 0)
        {
            //im dead
            if(side == 1)
            {
                GameStateManager.managerinstance.ChangeState(GameStateManager.GameState.Lose);
            }
            else
            {
                GameStateManager.managerinstance.ChangeState(GameStateManager.GameState.Win);
            }

        }
    }
}
