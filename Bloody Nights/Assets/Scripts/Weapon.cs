using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private KeyCode fire_Button = KeyCode.None;
    [SerializeField] private Transform camera_Transform = null;
    [SerializeField] private LayerMask player_Layer;
    [SerializeField] private ParticleSystem muzzle_Flash = null;

    [SerializeField] private float firing_Distance = 30.0f;
    [SerializeField] private float damage = 1.0f;
    

    [SerializeField] private float fire_Rate = 0.5f;
    private float next_Fire = 0.0f;

    private RaycastHit hit;

    private void Awake()
    {
        player_Layer = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        MuzzleFalsh();
        if(Input.GetKey(fire_Button))
        {
            Shoot();
        }
        
    }

    private void Shoot()
    {
        if(Physics.Raycast(camera_Transform.position , camera_Transform.forward , out hit , firing_Distance , ~player_Layer) && Time.time > next_Fire)
        { 
            next_Fire = Time.time + fire_Rate;
            print("jus hit == "+hit.collider.name);
            EnemyHealth enemyhit = hit.collider.GetComponent<EnemyHealth>();
            if(enemyhit != null)
            {
                enemyhit.LoseHealth(damage);
            }

           
        }
    }

    private void MuzzleFalsh()
    {
        if (Input.GetKey(fire_Button))
        {
            ParticleSystem.EmissionModule emmision = muzzle_Flash.emission;
            emmision.rateOverTime = 1000.0f;
        }
        else
        {
            ParticleSystem.EmissionModule emmision = muzzle_Flash.emission;
            emmision.rateOverTime = 0.0f;
        }
            
    }
}
