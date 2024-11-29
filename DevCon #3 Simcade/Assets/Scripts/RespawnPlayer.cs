using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject respawnPoint;
    public GameManager manager;
    public AudioManager audioManager;
    PlayerMovement resetPlayer;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        resetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlaySoundEffect("Death");
            player.transform.position = respawnPoint.transform.position;
            manager.ResetAcorn();
            resetPlayer.ResetScale();
        }
    }
} 
