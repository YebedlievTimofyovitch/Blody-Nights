using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_manager : MonoBehaviour
{
    [SerializeField] private TMP_Text Health_Text = null;
    private PlayerHealth player_Health = null;
    private string health_DefaultText = "000";
    

    [SerializeField] private TMP_Text AmmoHeld_Text = null;
    private GameObject active_Weapon = null;
    private WeaponSwitcher weapon_Switcher = null;
    private string default_Ammo_InMag = "00";
    private string default_Ammo_Total = "00";

    void Awake()
    {
        player_Health = FindObjectOfType<PlayerHealth>();
        weapon_Switcher = FindObjectOfType<WeaponSwitcher>();
        AmmoHeld_Text.text = default_Ammo_InMag + "/" + default_Ammo_Total;

    }

    void Update()
    {
        UpdateAmmoStatus();
        UpdateHealthStatus();
    }

    private void UpdateAmmoStatus()
    {
        active_Weapon = weapon_Switcher.GetActiveWeapon;
        Weapon activeW = active_Weapon.GetComponent<Weapon>();

        AmmoHeld_Text.text = activeW.GetAmmoInMag.ToString() + "/" + activeW.GetAmmoTotal.ToString();
    }

    private void UpdateHealthStatus()
    {
        Health_Text.text = player_Health.GetHealth.ToString();
    }
}
