using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    public float Health
    {
        set
        {
            health = value;
            
            if (health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    private float health = 1; // Private field to hold health value

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Method to handle what happens when the enemy is defeated
    public void Defeated()
    {
        animator.SetTrigger("Defeated");
       
    }

    public void RemoveEnemy()
    {
         Destroy(gameObject); // Destroy the GameObject when health drops to zero or below
    }
}