using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatArrowController : MonoBehaviour {

    public AgentController enemy;
   public  Vector3 enemyPos;
    public Vector3 startPos;

    private void Start()
    {
        transform.position += Vector3.up * .25f;
        startPos = transform.localPosition;
        gameObject.SetActive(false);
    }

    // Use this for initialization


    private void OnDisable()
    {
        transform.localPosition = startPos;
        enemy = null;
    }
    private void FixedUpdate()
    {
        if (enemy)
        {
            Vector3 dist = (enemyPos - transform.position);
            transform.position = transform.position + dist.normalized * Time.deltaTime * 3;
            transform.LookAt(enemyPos);
            if(dist.magnitude <= .5)
            {
                enemy.AttackMe(1.0f, 2);
                gameObject.SetActive(false);
            }
        }

    }
}
