using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    public float Speed = 1.5f;

    private CharacterController m_characterController;
    private void Start()
    {
        m_characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Vector3 moveResult = new Vector3(0.0f, 0.0f, 0.0f);

        if (Input.GetKey(KeyCode.D))
            moveResult += transform.right;
        if (Input.GetKey(KeyCode.A))
            moveResult -= transform.right;
        if (Input.GetKey(KeyCode.E))
            moveResult += transform.up;
        if (Input.GetKey(KeyCode.Q))
            moveResult -= transform.up;
        if (Input.GetKey(KeyCode.W))
            moveResult += transform.forward;
        if (Input.GetKey(KeyCode.S))
            moveResult -= transform.forward;

        moveResult.Normalize();

        if (null != m_characterController)
        {
            m_characterController.Move(moveResult * Speed * Time.deltaTime);
        }

        Vector2 lookResult = new Vector2(0.0f, 0.0f);

        if (Input.GetMouseButton(1))
        {
            lookResult.x = -Input.GetAxis("Mouse Y");
            lookResult.y = Input.GetAxis("Mouse X");
        }

        transform.Rotate(Vector3.right, lookResult.x, Space.Self);
        transform.Rotate(Vector3.up, lookResult.y, Space.World);
    }
}