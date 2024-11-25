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

        if (gameManager != null)
        {
            if (other.CompareTag("Player"))
            {
                gameManager.CollectAcorn();
                Destroy(gameObject);
            }
        }
    }
}
