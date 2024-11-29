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
    public AudioManager audioManager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

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
                AudioManager.instance.PlaySoundEffect("Eat");
                Instantiate(pickupEffect, transform.localPosition, Quaternion.identity);
                gameManager.CollectAcorn();
                acornCollider.enabled = false;
                acornMesh.enabled = false;

                isCollected = true;
            }
        }
    }
}
