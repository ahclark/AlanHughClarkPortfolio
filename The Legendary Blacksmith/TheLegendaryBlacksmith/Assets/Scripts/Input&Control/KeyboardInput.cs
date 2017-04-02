using UnityEngine;
using System.Collections;

public class KeyboardInput : MonoBehaviour
{
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    void Update()
    {
 
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            //moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = new Vector3(0, 0, 0);
            moveDirection += Camera.main.transform.right * Input.GetAxis("Horizontal");
            moveDirection += Camera.main.transform.forward * Input.GetAxis("Vertical");
            moveDirection.Normalize();
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
