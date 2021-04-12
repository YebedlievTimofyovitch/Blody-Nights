using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [SerializeField] private int Ammo_Amount = 10;
    public int GetAmmo_InPack { get { return Ammo_Amount; } }

    [SerializeField] private AmmoType ammo_Type = AmmoType.Default;
    public AmmoType GetAmmoType { get { return ammo_Type; } }
}
