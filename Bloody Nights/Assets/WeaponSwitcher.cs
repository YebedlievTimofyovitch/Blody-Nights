using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private  KeyCode weapon_DownKey = KeyCode.None, weapon_UpKey = KeyCode.None;
    private List<GameObject> weapon_List = new List<GameObject>();
    private GameObject current_ActiveWeapon = null;

    private void Awake()
    {
        foreach(Transform weapon in transform)
        {
            if (weapon.tag == "Weapon")
            {
                weapon_List.Add(weapon.gameObject);
                weapon.gameObject.SetActive(false);
            }
        }
        current_ActiveWeapon = weapon_List[0];
        current_ActiveWeapon.SetActive(true);
        print(current_ActiveWeapon);
    }

    private void Update()
    {
        if(Input.GetKeyDown(weapon_UpKey))
        {
            SwitchWeapon(1);
        }
        else if(Input.GetKeyDown(weapon_DownKey))
        {
            SwitchWeapon(-1);
        }
    }

    private void SwitchWeapon(int how_muchtojump)
    {
        int currentWeaponIndex = weapon_List.IndexOf(current_ActiveWeapon);
        current_ActiveWeapon.SetActive(false);
        
        if(currentWeaponIndex == weapon_List.Count - 1 && how_muchtojump > 0)
        {
            currentWeaponIndex = 0;

        }
        else if(currentWeaponIndex == 0 && how_muchtojump < 0)
        {
            currentWeaponIndex = weapon_List.Count - 1;
        }
        else
        {
            currentWeaponIndex += how_muchtojump;
        }

        print(weapon_List.IndexOf(current_ActiveWeapon));

        weapon_List[currentWeaponIndex].SetActive(true);
        current_ActiveWeapon = weapon_List[currentWeaponIndex];
    }
}
