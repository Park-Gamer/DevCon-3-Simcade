using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField, Range(0, 50)] float moveSpeed = 10f;
    [SerializeField, Range(0, 25)] float jumpSpeed = 5f;

    [SerializeField, Range(0, 5)] int maxAirJumps = 0; // The maximum number of air jumps
    public int jumpPhase = 0; // Tracks the current phase of jumping

    [SerializeField] float distanceToGround;
    public LayerMask groundLayer;
    bool onGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float inputY = Input.GetAxisRaw("Vertical") * moveSpeed;

        rb.velocity = new Vector3(inputX, rb.velocity.y, inputY);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanceToGround, groundLayer))
        {
            Debug.Log("Ground detected");
            jumpPhase = 0;
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround || Input.GetKeyDown(KeyCode.Space) && jumpPhase < maxAirJumps)
        {
            jumpPhase += 1; // Tracks air jumps
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        }

        transform.forward = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
}
