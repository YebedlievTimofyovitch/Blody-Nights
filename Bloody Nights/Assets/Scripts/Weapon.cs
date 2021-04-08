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

    [SerializeField] private KeyCode reload_Key = KeyCode.None;
    [SerializeField] private float reload_Time = 0.5f;
    private bool has_Ammo_AtAll = true;
    private bool has_Ammo_InMag = true;
    [SerializeField] private int ammo_Total = 120;
    [SerializeField] private int ammo_Mag_Capacity = 30;
    [SerializeField] private int ammo_InMag = 30;

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

        if(ammo_InMag != ammo_Mag_Capacity && Input.GetKeyDown(reload_Key))
        {
            StartCoroutine(Reload());
        }

        if(Input.GetKey(fire_Button))
        {
            Shoot();
        }
        
    }

    private void Shoot()
    {
        if (has_Ammo_InMag)
        {

            if (Physics.Raycast(camera_Transform.position, camera_Transform.forward, out hit, firing_Distance, ~player_Layer) && Time.time > next_Fire)
            {
                AmmoCounter();

                next_Fire = Time.time + fire_Rate;
                print("jus hit == " + hit.collider.name);
                EnemyHealth enemyhit = hit.collider.GetComponent<EnemyHealth>();

                if (enemyhit != null)
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
    }

    private void AmmoCounter()
    {
        if (ammo_Total > 0)
        {

            if (ammo_InMag > 0)
            {
                print("Ammo In Mag = " + ammo_InMag);
                ammo_InMag -= 1;
            }
            else
            {
                has_Ammo_InMag = false;
                print("RELOAD!");
            }
        }
        else if (ammo_Total == 0)
        {
            if (ammo_InMag > 0)
            {
                print("Ammo In Mag = " + ammo_InMag);
                ammo_InMag -= 1;
            }
            else
            {
                has_Ammo_InMag = false;
                has_Ammo_AtAll = false;
                print("RELOAD!");
            }
        }
    }

    private IEnumerator Reload()
    {
        if (ammo_Total == 0)
        {
            has_Ammo_AtAll = false;
            print("OUT OF AMMO!");
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(reload_Time);

            int a_in_m = ammo_InMag;

            int ammountToAdd = ammo_Mag_Capacity - a_in_m;

            if (ammountToAdd > ammo_Total)
            {
                ammountToAdd = ammo_Total;
                has_Ammo_AtAll = false;
                ammo_Total -= ammountToAdd;
            }
            else
                ammo_Total -= ammountToAdd;

            print("Ammo Total = :" + ammo_Total);
            has_Ammo_InMag = true;
            ammo_InMag += ammountToAdd;
        }
    }

    private void SpawnHitFX(Vector3 position , Vector3 normal , GameObject particleSystem)
    {
        Instantiate(particleSystem , position, Quaternion.FromToRotation(Vector3.forward , normal));
    }

    private void MuzzleFalsh()
    {
        if (Input.GetKey(fire_Button) && has_Ammo_InMag)
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
