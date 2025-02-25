using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    public float joystickSensitivity = 2f; // Sensibilidad para el joystick derecho

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Movimiento con el mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Movimiento con el joystick derecho (Right Joystick)
        float joystickX = Input.GetAxis("RightJoystickX") * joystickSensitivity;
        float joystickY = Input.GetAxis("RightJoystickY") * joystickSensitivity;

        // Actualizar la rotación con el mouse y el joystick
        xRotation -= mouseY + joystickY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        yRotation += mouseX + joystickX;

        // Aplicar la rotación de la cámara
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
