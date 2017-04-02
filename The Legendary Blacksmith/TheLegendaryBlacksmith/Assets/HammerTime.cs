using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class HammerTime : MonoBehaviour {

    public Material Default;
    public Material Malleable;
    public Material Brittle;
    public bool isMalleable;
    public bool isBrittle;
    public bool inForge;
    public float ReadyTime;
    private float ForgeTimer;
    Color mRed = Color.red;
    Renderer mat;

    public bool OnAnvil;

    public Mesh preforge;
    [SerializeField]
    Mesh[] meshes;
    //public Mesh mesh1;
    //public Mesh mesh2;
    //public Mesh mesh3;
    //public Mesh mesh4;
    //public Mesh mesh5;
    public GameObject change;
    MeshFilter change_mesh;
    public GameObject spawn;
    public GameObject failSpawn;
    public int count = 0;
    int count_limit = 0;
    public float breakItDown;
    public float bringItBack;
    public float numCube;
    private bool onlyOnce = true;
    private float timer = 0;

    public ParticleSystem particles;

    AudioSource audio;

    void Start()
    {
        // gameObject.GetComponent<BaseObjCombination>().gameObject.GetComponent<MeshFilter>().mesh = mesh1;
        mat = GetComponent<Renderer>();
        count_limit = meshes.Length;
        change_mesh = change.GetComponent<MeshFilter>();
        change_mesh.mesh = meshes[count];
        isMalleable = false;
        isBrittle = false;
        inForge = false;
        ForgeTimer = 0.0f;

        OnAnvil = false;

        mat.material = Default;

        if (particles)
            particles.enableEmission = false;

        audio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (inForge && ForgeTimer < ReadyTime)
            ForgeTimer += Time.fixedDeltaTime;
        else if (ForgeTimer > 0.0f)
            ForgeTimer -= Time.fixedDeltaTime;
        //if (ForgeTimer <= 0.0f)
        //{
        //    isMalleable = false;
        //    this.GetComponent<Renderer>().mat.materialerial = Default;
        //}
        if (ForgeTimer >= ReadyTime)
        {
            isMalleable = true;
            mat.material = Malleable;
        }

        if (isMalleable && !inForge)
            timer += Time.fixedDeltaTime;
        if (timer >= breakItDown)
        {
            isMalleable = false;
            isBrittle = true;
            mat.material = Brittle;
        }
        else if (timer >= bringItBack)
            mat.material.color = Malleable.color;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Head" && onlyOnce == true && OnAnvil)
        {
            count++;
            onlyOnce = false;
            if (timer <= bringItBack)
            {
                //change.GetComponent<MeshFilter>().mesh = preforge;
                //isMalleable = false;
                //this.GetComponent<Renderer>().mat.materialerial = Default;
                //count = 0;
                //change.GetComponent<MeshFilter>().mesh = mesh1;

                for (int i = 0; i < 10; i++)
                {
                    Instantiate(failSpawn, change.transform.position, change.transform.rotation);
                }
                //Miles Adding the detatch without destroying controller
                if (change.transform.parent)
                {
                    change.transform.parent.gameObject.GetComponent<Hand>().DetachObject(change);
                }
                change.SetActive(false);
                //end Miles' snippit.

                Destroy(change);
            }
            else if (isBrittle)
            {
                for (int i = 0; i < 10; i++)
                {
                    Instantiate(failSpawn, change.transform.position, change.transform.rotation);
                }
                //Miles Adding the detatch without destroying controller
                if (change.transform.parent)
                {
                    change.transform.parent.gameObject.GetComponent<Hand>().DetachObject(change);
                }
                change.SetActive(false);
                //end Miles' snippit.

                Destroy(change);
            }
            else if (isMalleable) {
                if (count < count_limit)
                {
                    change_mesh.mesh = meshes[count];
                    timer = 0.0f;
                    mat.material.color = mRed;
                }

                else
                {
                    if (OnAnvil)
                    {
                        //Miles Adding the detatch without destroying controller
                        if (change.transform.parent)
                        {
                            change.transform.parent.gameObject.GetComponent<Hand>().DetachObject(change);
                        }
                        change.SetActive(false);
                        //end Miles' snippit.

                        Instantiate(spawn, change.transform.position, change.transform.rotation).name = spawn.name;
                        Destroy(change);
                    }
                }
                
                //switch (count)
                //{
                //    case 0:
                //        change_mesh.mesh = mesh1;
                //        timer = 0.0f;
                //        mat.material.color = mRed;
                //        break;
                //    case 1:
                //        //gameObject.GetComponent<BaseObjCombination>().gameObject.GetComponent<MeshFilter>().mesh = mesh2;
                //        change_mesh.mesh = mesh2;
                //        timer = 0.0f;
                //        mat.material.color = mRed;
                //        break;
                //    case 2:
                //        //gameObject.GetComponent<BaseObjCombination>().gameObject.GetComponent<MeshFilter>().mesh = mesh3;
                //        change_mesh.mesh = mesh3;
                //        timer = 0.0f;
                //        mat.material.color = mRed;
                //        break;
                //    case 3:
                //        //gameObject.GetComponent<BaseObjCombination>().gameObject.GetComponent<MeshFilter>().mesh = mesh4;
                //        change.GetComponent<MeshFilter>().mesh = mesh4;
                //        timer = 0.0f;
                //        mat.material.color = mRed;
                //
                //        break;
                //    case 4:
                //        //gameObject.GetComponent<BaseObjCombination>().gameObject.GetComponent<MeshFilter>().mesh = mesh5;
                //        change_mesh.mesh = mesh5;
                //        timer = 0.0f;
                //        mat.material.color = mRed;
                //        break;
                //    case 5:
                //        if (OnAnvil)
                //            
                //        Instantiate(spawn, change.transform.position, change.transform.rotation).name = spawn.name;
                //        Destroy(change);
                //        break;
                //    default:
                //        break;
                //}

                // Make controller vibrate, create sparks, and play sound
                if (particles)
                {
                    particles.enableEmission = true;
                    particles.Play();
                }

                if(audio)
                {
                    AudioManager.instance.PlayCollisionSound(audio, AudioManager.AudioInteractionType.Bang, AudioManager.AudioObjectType.Metal, AudioManager.AudioObjectType.Metal);
                }

                //GameObject controller = null;
                //if (col.transform.parent)
                //{
                //    if (col.transform.parent.parent.tag == "Controller")
                //        controller = col.transform.parent.parent.gameObject;
                //}
                //if (controller)
                //    controller.GetComponent<InteractionController>().Vibrate(3999);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Head" && onlyOnce == false)
        {
            onlyOnce = true;
        }
    }

    //public void Tutorial()
    //{
    //    Tutorials.tutorialinstance.Tutorial(gameObject);
    //}

    public void StopTutorial()
    {
        Tutorials.tutorialinstance.StopForgeTutorial();
    }

}
