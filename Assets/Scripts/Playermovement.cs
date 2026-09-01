using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactRange = 3f;

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
        TryInteract();

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

    private void TryInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable == null)
                {
                    interactable = hit.collider.GetComponentInChildren<IInteractable>();
                }
                if (interactable != null && interactable.IsInteractable)
                {
                    interactable.Interact();
                }
            }
        }
    }
}