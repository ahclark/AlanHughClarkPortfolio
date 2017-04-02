using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarTick : MonoBehaviour {
    bool go = false;
    int leftOrRight = 0; //-1 is left, 1 is right
    public Vector3 changeVector = new Vector3(0, .01f, 0);
    public Vector3 scaleVector = new Vector3(.01f, .01f, 0);
    Vector3 returnLocalPos, returnLocalScale;
    MinionDamageEffectController damageEffect = null;

    private void Start()
    {
        damageEffect = GetComponentInChildren<MinionDamageEffectController>();
    }
    public void Tick(int _leftOrRight)
    {
        returnLocalPos = transform.localPosition;
        returnLocalScale = transform.localScale;
        leftOrRight = _leftOrRight;
        go = true;
    }
    private void FixedUpdate()
    {
        if (go)
        {
            scaleVector.x = scaleVector.y = changeVector.y = Time.deltaTime;
            changeVector.x = Time.deltaTime * leftOrRight;
            if (transform.localScale.x < .95f)
            {
                if(damageEffect)
                {
                    damageEffect.Play();
                }
                transform.position = transform.position + changeVector;
                transform.localScale += Vector3.up * Time.deltaTime * 10;
                transform.localScale += Vector3.right * Time.deltaTime;

            }
            else
            {
                transform.localScale = returnLocalScale;
                transform.localPosition = returnLocalPos;
                gameObject.SetActive(false);
            }
        }
    }

}
