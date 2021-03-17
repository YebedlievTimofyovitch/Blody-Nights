using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputname = "", mouseYInputName = "";
    [SerializeField] private float mouseSensetivity = 1.0f;

    [SerializeField] Transform playerBody_Transform = null;

    private float xAxisClamp = 0.0f;

    private void Awake()
    {
        LockMouseCursorToCenter();
    }

    private void Update()
    {
        CameraRotation();
    }

    private void LockMouseCursorToCenter()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputname) * mouseSensetivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensetivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if(xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXaxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXaxisRotationToValue(90.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody_Transform.Rotate(Vector3.up * mouseX);
    }

    private void ClampXaxisRotationToValue(float value)
    {
        Vector3 cameraEulers = transform.eulerAngles;
        cameraEulers.x = value;
        transform.eulerAngles = cameraEulers;
    }


}
