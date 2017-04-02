using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkFieldController : MonoBehaviour
{
    [SerializeField]
    GameObject m_firework;

    [SerializeField]
    float m_timer;
    public float m_intervalTime;

    private void Start()
    {
        m_timer = 0.0f;
    }

    private void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= m_intervalTime)
        {
            m_timer = 0.0f;
            //shoot firework
            GameObject firework = Instantiate(m_firework, this.transform.position, this.transform.rotation);
            firework.transform.Translate(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0.0f);
            FireworkController temp = firework.GetComponent<FireworkController>();
            temp.m_speed = Random.Range(10.0f, 30.0f);
        }
    }
}
