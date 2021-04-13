using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private AmmoType ammo_Type = AmmoType.Default;
    public AmmoType GetAmmoType { get{ return ammo_Type; } }

    [SerializeField] private KeyCode fire_Button = KeyCode.None;
    [SerializeField] private Transform camera_Transform = null;
    [SerializeField] private LayerMask player_Layer;
    [SerializeField] private string enemy_LayerString = null;
    [SerializeField] private ParticleSystem muzzle_Flash = null;

    [SerializeField] private KeyCode reload_Key = KeyCode.None;
    [SerializeField] private float reload_Time = 0.5f;
    private bool is_Reloading = false;
    private bool has_Ammo_InMag = true;
    [SerializeField] int ammo_Total_Cap = 100;
    [SerializeField] private int ammo_Total = 120;
    public int GetAmmoTotal { get { return ammo_Total; } }

    [SerializeField] private int ammo_Mag_Capacity = 30;
    [SerializeField] private int ammo_InMag = 30;
    public int GetAmmoInMag { get { return ammo_InMag; } }

    [SerializeField] private float firing_Distance = 30.0f;
    [SerializeField] private float damage = 1.0f;

    [SerializeField] private GameObject blood_Splatter = null;
    [SerializeField] private GameObject bullet_ImpactDUST = null;

    [SerializeField] private float RPM = 150.0f;
    private float fire_Rate = 0.0f;
    private float next_Fire = 0.0f;

    private RaycastHit hit;

    private void Awake()
    {
        fire_Rate =  60.0f/RPM;
    }

    private void Update()
    {
        if(ammo_InMag != ammo_Mag_Capacity && Input.GetKeyDown(reload_Key))
        {
            StartCoroutine(Reload());
        }
        if (Input.GetKey(fire_Button))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (has_Ammo_InMag && !is_Reloading && Time.time > next_Fire)
        {
            AmmoCounter();
            muzzle_Flash.Play();
            next_Fire = Time.time + fire_Rate;

            if (Physics.Raycast(camera_Transform.position, camera_Transform.forward, out hit, firing_Distance, ~player_Layer))
            {
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
        if (ammo_Total >= 0)
        {

            if (ammo_InMag > 0)
            {
                ammo_InMag -= 1;
                if(ammo_InMag == 0)
                {
                    has_Ammo_InMag = false;
                }
            }
            else if(ammo_InMag == 0)
            {
                has_Ammo_InMag = false;
            }
        }
    }

    private IEnumerator Reload()
    {
        if (ammo_Total == 0)
        {
            yield return null;
        }
        else
        {
            is_Reloading = true;
            yield return new WaitForSeconds(reload_Time);

            int a_in_m = ammo_InMag;

            int ammountToAdd = ammo_Mag_Capacity - a_in_m;

            if (ammountToAdd > ammo_Total)
            {
                ammountToAdd = ammo_Total;
                ammo_Total -= ammountToAdd;
            }
            else
                ammo_Total -= ammountToAdd;

            has_Ammo_InMag = true;
            ammo_InMag += ammountToAdd;
            is_Reloading = false;
        }
        
    }

    public bool AddAmmo(int ammo_To_Add)
    {
        if(ammo_Total == ammo_Total_Cap)
        {
            return false;
        }

        ammo_Total += ammo_To_Add;
        if(ammo_Total > ammo_Total_Cap)
        {
            ammo_Total = ammo_Total_Cap;
            return true;
        }

        return false;
    }

    private void SpawnHitFX(Vector3 position , Vector3 normal , GameObject particleSystem)
    {
        Instantiate(particleSystem , position, Quaternion.FromToRotation(Vector3.forward , normal));
    }
}
