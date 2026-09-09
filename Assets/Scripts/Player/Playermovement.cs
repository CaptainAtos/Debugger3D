using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private GameObject interactPromptPanel;

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float sprintSpeed = 14f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private float verticalVelocity = 0f;
    private IInteractable currentInteractable;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnDisable()
    {
        if (interactPromptPanel != null)
        {
            interactPromptPanel.SetActive(false);
        }
    }

    void Update()
    {
        UpdateInteractTarget();
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

    private void UpdateInteractTarget()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        IInteractable hitInteractable = null;

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            hitInteractable = hit.collider.GetComponentInParent<IInteractable>();
            if (hitInteractable == null)
            {
                hitInteractable = hit.collider.GetComponentInChildren<IInteractable>();
            }
        }

        if (hitInteractable != null && hitInteractable.IsInteractable)
        {
            currentInteractable = hitInteractable;
        }
        else
        {
            currentInteractable = null;
        }

        if (interactPromptPanel != null)
        {
            interactPromptPanel.SetActive(currentInteractable != null);
        }
    }

    private void TryInteract()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
}