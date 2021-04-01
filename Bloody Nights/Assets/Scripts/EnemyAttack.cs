using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private PlayerHealth player_Health = null;
    
    [SerializeField] float damage = 30.0f;

    private void Awake()
    {
        player_Health = FindObjectOfType<PlayerHealth>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AttackHitEvent()
    {
        if (player_Health == null)
            return;

        player_Health.SetHealth(-damage);
    }
}
