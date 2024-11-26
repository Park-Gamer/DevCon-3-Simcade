using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AcornCollection : MonoBehaviour
{
    public GameObject pickupEffect;

    void OnTriggerEnter(Collider other)
    {
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        GameManager gameManager = manager.GetComponent<GameManager>();

        if (gameManager != null)
        {
            if (other.CompareTag("Player"))
            {
                Instantiate(pickupEffect, transform.localPosition, Quaternion.identity);
                gameManager.CollectAcorn();
                Destroy(gameObject);
            }
        }
    }
}
