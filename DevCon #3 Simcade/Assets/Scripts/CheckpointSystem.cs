using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    private RespawnPlayer respawn;
    private BoxCollider checkCollider;

    public GameObject killBox;
    // Offset value to raise the object
    public float yOffset = 5f;

    private void Awake()
    {
        killBox = GameObject.FindGameObjectWithTag("Respawn");
        checkCollider = GetComponent<BoxCollider>();
        respawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RespawnPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            respawn.respawnPoint = this.gameObject;
            checkCollider.enabled = false;

            // Raise the object only on the Y-axis by the offset value
            Vector3 newPosition = transform.position;
            newPosition.y -= yOffset;
            killBox.transform.position = newPosition;
        }
    }
}
