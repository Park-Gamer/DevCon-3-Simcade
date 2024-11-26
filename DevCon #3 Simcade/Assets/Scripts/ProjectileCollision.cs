using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if it hits an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Apply damage to the enemy 
            Destroy(collision.gameObject); 
        }
        else
        {
            // If it's not an enemy destroy the projectile when it hits anything else
            Destroy(gameObject); 
        }
    }
}
