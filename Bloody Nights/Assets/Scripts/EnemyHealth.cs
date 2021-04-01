using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float Health = 100.0f;
    public float GetHealth { get { return Health; } }

    public void LoseHealth(float damage)
    {
        BroadcastMessage("OnDamageTaken");

        Health -= damage;
        if(Health <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
