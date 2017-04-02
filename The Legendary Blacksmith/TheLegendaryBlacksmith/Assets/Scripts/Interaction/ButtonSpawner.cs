using UnityEngine;
using System.Collections;

public class ButtonSpawner : MonoBehaviour {

    public GameObject Button;
    public Vector3 spawnLocation = new Vector3(0, 1, 0);
    public GameObject ToCopy;
    public float timer = 0;
    private ButtonPush BP;

    void Start()
    {
        BP = Button.GetComponent<ButtonPush>();
    }
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }
 
    private void Update()
    {
        if (BP.buttonOn && timer > 2)
        {
            GameObject newGo = GameObject.Instantiate(ToCopy);
            newGo.name = ToCopy.name;
            newGo.transform.position = this.transform.position + spawnLocation;
            newGo.transform.localScale = ToCopy.transform.lossyScale;
            timer = 0;
        }
    }
}
