using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    private float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0f, mouseX, 0f);

        verticalRotation = verticalRotation - mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}