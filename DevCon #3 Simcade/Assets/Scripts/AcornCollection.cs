using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AcornCollection : MonoBehaviour
{
    public GameObject pickupEffect;

    private Collider acornCollider;
    private MeshRenderer acornMesh;

    private bool isCollected = false;
    public GameManager manager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        acornCollider = GetComponent<Collider>();
        acornMesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (isCollected && manager.acornCount <= 0)
        {
            acornCollider.enabled = true;
            acornMesh.enabled = true;

            isCollected = false;
        }
    }

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
                acornCollider.enabled = false;
                acornMesh.enabled = false;

                isCollected = true;
            }
        }
    }
}
