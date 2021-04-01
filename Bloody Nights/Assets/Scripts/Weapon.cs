using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private KeyCode fire_Button = KeyCode.None;
    [SerializeField] private Transform camera_Transform = null;
    [SerializeField] private LayerMask player_Layer;
    [SerializeField] private string enemy_LayerString = null;
    [SerializeField] private ParticleSystem muzzle_Flash = null;


    [SerializeField] private float firing_Distance = 30.0f;
    [SerializeField] private float damage = 1.0f;

    [SerializeField] private GameObject blood_Splatter = null;
    [SerializeField] private GameObject bullet_ImpactDUST = null;

    [SerializeField] private float fire_Rate = 0.5f;
    private float next_Fire = 0.0f;

    private RaycastHit hit;

    private void Awake()
    {
        
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

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(enemy_LayerString))
            {
                SpawnHitFX(hit.point, hit.normal, blood_Splatter);
            }
            else
                SpawnHitFX(hit.point, hit.normal, bullet_ImpactDUST);
           
        }
    }

    private void SpawnHitFX(Vector3 position , Vector3 normal , GameObject particleSystem)
    {
        Instantiate(particleSystem , position, Quaternion.FromToRotation(Vector3.forward , normal));
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
