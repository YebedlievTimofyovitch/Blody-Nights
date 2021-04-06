using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private PlayerMove player_Move = null;

    [SerializeField] private string mouseXInputname = "", mouseYInputName = "";
    private bool is_CursorLocked = true;
    [SerializeField] private float mouseSensetivity = 1.0f;

    [SerializeField] Transform playerBody_Transform = null;

    [SerializeField] float headBobb_Speed = 1.0f;
    [SerializeField] float headBobb_Multiplyer = 1.0f;
    [SerializeField] float headBobb_VerticalDistance = 0.15f;
    [SerializeField] float headBobb_HorizontalDistance = 0.05f;
    private Vector3 original_Position = Vector3.zero;

    private float xAxisClamp = 0.0f;

    private void Awake()
    {
        player_Move = GetComponentInParent<PlayerMove>();
        original_Position = transform.localPosition;
        
    }

    private void Update()
    {
        LockMouseCursorToCenter();

        CameraRotation();

       if (player_Move.IsMoving && player_Move.IsGrounded)
            HeadBobb();
       else
            ReturnHeadToOrigin();
    }

    private void LockMouseCursorToCenter()
    {
        if(PlayerHealth.is_Dead)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.0f;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            is_CursorLocked = !is_CursorLocked;
        }


        if(is_CursorLocked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
        
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

    private void HeadBobb()
    {
        float headBobbVert = Mathf.Sin(Time.time * headBobb_Speed) * headBobb_Multiplyer * Time.deltaTime;
        float headBobbHori = Mathf.Cos(Time.time * headBobb_Speed) * headBobb_Multiplyer *Time.deltaTime;

        float clampedHBVert = Mathf.Clamp(headBobbVert, -headBobb_VerticalDistance, headBobb_VerticalDistance);
        float clampedHBhori = Mathf.Clamp(headBobbHori , -headBobb_HorizontalDistance , headBobb_HorizontalDistance);

        clampedHBhori -= clampedHBhori / 2.0f;
        clampedHBVert -= clampedHBVert / 2.0f;

        transform.localPosition += new Vector3(clampedHBhori, clampedHBVert, 0.0f);
    }

    private void ReturnHeadToOrigin()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, original_Position, headBobb_Speed * Time.deltaTime);
    }
}
