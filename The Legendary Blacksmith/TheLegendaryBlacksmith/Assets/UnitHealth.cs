using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour {

    float health = 10;

    public HealthBarTick[] healthbar = new HealthBarTick[10];
    public bool TakeDamage(float damage)
    {
        if(health <= 0)
        {
            return false;
        }
        if (healthbar[(int)health - 1])
        {
            if ((int)health % 2 == 0)
            {
                healthbar[(int)health - 1].Tick(1);
            }
            else
            {
                healthbar[(int)health - 1].Tick(-1);

            }
            health -= damage;
        }
        if (health <= 0)
        {
            return false;
        }
        return true;
    }
}
