using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomIn_Out : MonoBehaviour
{
    [SerializeField] float default_FOV = 60.0f, ZoomIn_Fov = 50.0f;
    [SerializeField] private float zoom_Speed = 1.0f;

    [SerializeField] private KeyCode zoom_Key = KeyCode.None;

    [SerializeField] private Camera weapon_Camera = null;
    private Camera this_Cam = null;

    private void Awake()
    {
        this_Cam = GetComponent<Camera>();
    }

    void Update()
    {
            Zoom_InAndOut();
    }

    private void Zoom_InAndOut()
    {
        if (Input.GetKey(zoom_Key))
        {
            this_Cam.fieldOfView = Mathf.Lerp(this_Cam.fieldOfView, ZoomIn_Fov, zoom_Speed * Time.deltaTime);
            weapon_Camera.fieldOfView = Mathf.Lerp(weapon_Camera.fieldOfView, ZoomIn_Fov, zoom_Speed * Time.deltaTime);
        }
        else
        {
            this_Cam.fieldOfView = Mathf.Lerp(this_Cam.fieldOfView, default_FOV , zoom_Speed * Time.deltaTime);
            weapon_Camera.fieldOfView = Mathf.Lerp(weapon_Camera.fieldOfView, default_FOV , zoom_Speed * Time.deltaTime);
        }
    }
}
