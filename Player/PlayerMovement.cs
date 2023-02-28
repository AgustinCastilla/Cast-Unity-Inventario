using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Controller
    private CharacterController controller;

    // Movimiento
    public float playerSpeed = 2.0f;
    public float MultiCorrer = 1.2f;

    // Rotación mouse
    public float mouseSensibilidad = 500.0f;
    public bool esconderCursor = true;
    private float xRotation = 0.0f, yRotation = 0.0f;

    // Salto
    public float alturaSalto = 1.0f;
    private float verticalVelocity;
    private float groundedTimer; // to allow jumping when going down ramps
    private static float gravity = 9.81f;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        if (esconderCursor) Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        bool groundedPlayer = controller.isGrounded;
        if (groundedPlayer) groundedTimer = 0.2f;
        if (groundedTimer > 0) groundedTimer -= Time.deltaTime;
        // -----------------------------------------------------------------------------

        if (groundedPlayer && verticalVelocity < 0) verticalVelocity = 0f; // hit ground
        verticalVelocity -= gravity * Time.deltaTime;
        // -----------------------------------------------------------------------------

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float sprint = 1.0f;
        if (Input.GetKey(KeyCode.LeftShift)) sprint = MultiCorrer;
        Vector3 move = (transform.right * x + transform.forward * z * sprint) * playerSpeed;

        // -----------------------------------------------------------------------------
        float mouseX = Input.GetAxis("Mouse X") * mouseSensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensibilidad * Time.deltaTime;
        xRotation += mouseX;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90.0f, 90.0f);
        transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0.0f);

        // -----------------------------------------------------------------------------
        // only align to motion if we are providing enough input
        //if (move.magnitude > 0.05f) gameObject.transform.forward = move;
        if (Input.GetButtonDown("Jump") && groundedTimer > 0)
        {
            groundedTimer = 0;
            verticalVelocity += Mathf.Sqrt(alturaSalto * 2 * gravity);
        }
        move.y = verticalVelocity; // inject Y velocity before we use it

        // -----------------------------------------------------------------------------
        controller.Move(move * Time.deltaTime); // call .Move() once only
    }
}
