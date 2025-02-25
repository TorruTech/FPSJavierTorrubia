using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    public float normalSpeed = 12f;
    public float runSpeed = 14f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Entrada de movimiento - Teclado o Mando (Xbox)
        float x = Input.GetAxis("Horizontal"); // "A/D" o Joystick izquierdo X
        float z = Input.GetAxis("Vertical");   // "W/S" o Joystick izquierdo Y

        Vector3 move = transform.right * x + transform.forward * z;

        bool isRunning = (Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Sprint")) && (x != 0 || z != 0);

        // Si el jugador mantiene Shift presionado, corre; si no, camina
        float currentSpeed = (Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Sprint")) ? runSpeed : normalSpeed;

        // Aplicar la velocidad al movimiento
        characterController.Move(move * currentSpeed * Time.deltaTime);

        // Avisar al arma actual para que active la animación de correr
        if (GetComponent<Player>().weapon != null)
        {
            GetComponent<Player>().weapon.SetRunningState(isRunning);
        }

        // Saltar - Teclado o Mando (Xbox)
        if (Input.GetButtonDown("Jump") && isGrounded) // Botón A (Xbox) o barra espaciadora
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        // Aplicar la gravedad y movimiento de salto
        characterController.Move(velocity * Time.deltaTime);

        // Detectar si el jugador se mueve
        if (lastPosition != gameObject.transform.position && isGrounded)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        lastPosition = gameObject.transform.position;
    }
}
