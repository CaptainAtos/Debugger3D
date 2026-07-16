using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float sprintSpeed = 14f;
    public float gravity = -20f;

    private CharacterController controller;
    private float verticalVelocity = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * inputX + transform.forward * inputZ;
        if (move.magnitude > 1f)
        {
            move = move.normalized;
        }

        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        verticalVelocity = verticalVelocity + gravity * Time.deltaTime;

        Vector3 finalMove = move * speed;
        finalMove.y = verticalVelocity;

        controller.Move(finalMove * Time.deltaTime);
    }
}