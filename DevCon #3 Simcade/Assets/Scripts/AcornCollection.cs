using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AcornCollection : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        GameManager gameManager = manager.GetComponent<GameManager>();

        // Check if the player has collided with the acorn (assuming the player has a tag "Player").
        if (other.CompareTag("Player"))
        {
            // Collect the acorn by calling the method on the manager.
            gameManager.CollectAcorn();

            // Destroy the acorn after collection.
            Destroy(gameObject);
        }
    }
}
