using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private SceneLoader scene_Loader = null;
    [SerializeField] GameObject death_Canvas = null;
    [SerializeField] GameObject reticle_Canvas = null;
    public static bool is_Dead = false;

    [SerializeField] private float Health = 100.0f;
    public float GetHealth { get { return Health; } }

    private void Awake()
    {
        scene_Loader = FindObjectOfType<SceneLoader>();
    }

    public void SetHealth(float healthDelta)
    {
        Health += healthDelta;

        if (healthDelta < 0.0f)
            print("took damage");
        else
            print("recieved health");

        if (Health > 100.0f)
        {
            Health = 100.0f;
        }

        if(Health <= 0.0f)
        {
            Death();
        }
    }

    private void Death()
    {
        is_Dead = true;

        print("you died");
        if (reticle_Canvas != null)
            reticle_Canvas.SetActive(false);

        if (death_Canvas != null)
            death_Canvas.SetActive(true);
    }
}
