using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private KeyCode fire_Button = KeyCode.None;
    [SerializeField] private Transform camera_Transform = null;
    [SerializeField] private float firing_Distance = 30.0f;

    [SerializeField] private float fire_Rate = 0.5f;
    private float next_Fire = 0.0f;

    private RaycastHit hit;

    private void Update()
    {
        if(Input.GetKey(fire_Button))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if(Physics.Raycast(camera_Transform.position , camera_Transform.forward , out hit , firing_Distance) && Time.time > next_Fire)
        {
            next_Fire = Time.time + fire_Rate;
            print(hit.collider.name);
        }
    }
}
