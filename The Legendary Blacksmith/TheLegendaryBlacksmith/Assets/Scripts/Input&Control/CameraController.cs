using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public bool UseReticle;
    public GameObject Reticle;
    private bool holding;
    private Camera camera;
    private RaycastHit hit;
    private Ray ray;
    private Transform m_object;
    private Vector3 prevPosition;
    private Vector3 prevVelocity;

    private void Start()
    {
        Reticle.SetActive(false);

        holding = false;
        camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.RotateAround(transform.position, Vector3.up, 2 * Input.GetAxis("Mouse X"));
            transform.RotateAround(transform.position, transform.right, 2 * -Input.GetAxis("Mouse Y"));
        }
        if (Input.GetKey(KeyCode.W))
            transform.position += (transform.forward * Time.deltaTime * 2);
        if (Input.GetKey(KeyCode.S))
            transform.position += (-transform.forward * Time.deltaTime * 2);
        if (Input.GetKey(KeyCode.D))
            transform.position += (transform.right * Time.deltaTime * 2);
        if (Input.GetKey(KeyCode.A))
            transform.position += (-transform.right * Time.deltaTime * 2);
        if (Input.GetKey(KeyCode.Space))
            transform.position += (transform.up * Time.deltaTime * 2);
        if (Input.GetKey(KeyCode.LeftShift))
            transform.position += (-transform.up * Time.deltaTime * 2);

        int layermask = 1 << 8;
        if (!holding && UseReticle && Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
        {
            Reticle.SetActive(true);
            Reticle.transform.position = hit.point;
        }
        else
        {
            Reticle.SetActive(false);
        }
        if (!holding && Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 100, layermask) && /*hit.transform.tag == "Selectable"*/ hit.transform.gameObject.layer == 8 && Input.GetKeyDown(KeyCode.E))
        {
            holding = true;
            GameObject objectHit = hit.collider.gameObject;
            m_object = objectHit.transform;
            while (m_object.parent != null)
                m_object = m_object.parent;
            m_object.parent = gameObject.transform;

            PickupObject(m_object);
        }
        else if (holding)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                holding = false;

                DropObject(m_object);
            }
            if (m_object)
            {
                prevPosition = m_object.transform.position;
                prevVelocity = m_object.GetComponent<Rigidbody>().velocity;
            }
        }

    }

    void PickupObject(Transform p_object)
    {
        p_object.GetComponent<Rigidbody>().useGravity = false;
        p_object.GetComponent<Rigidbody>().isKinematic = true;

        prevPosition = transform.position;
        prevVelocity = p_object.GetComponent<Rigidbody>().velocity;
    }

    void DropObject(Transform p_object)
    {
        if (p_object)
        {
            p_object.parent = null;
            p_object.GetComponent<Rigidbody>().useGravity = true;
            p_object.GetComponent<Rigidbody>().isKinematic = false;

            Vector3 throwVector = p_object.transform.position - prevPosition;
            float speed = throwVector.magnitude / Time.deltaTime;
            Vector3 throwVelocity = speed * throwVector.normalized;
            p_object.GetComponent<Rigidbody>().velocity = throwVelocity;
        }
    }

    //private void OnGUI()
    //{
    //    if (holding)
    //        GUILayout.Label("Object Selected : Yes");
    //    else
    //        GUILayout.Label("Object Selected : No");
    //}
}
