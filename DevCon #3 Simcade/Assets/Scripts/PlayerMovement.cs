using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField, Range(0, 50)] float moveSpeed = 10f;
    [SerializeField, Range(0, 25)] float jumpSpeed = 5f;

    public Vector3 gravityDirection = new Vector3(0, -1f, 0); // Default gravity direction 
    [SerializeField, Range(0, 25)] float gravityStrength = 9f;  // The strength of the gravity force

    [SerializeField, Range(0, 5)] int maxAirJumps = 0; // The maximum number of air jumps
    public int jumpPhase = 0; // Tracks the current phase of jumping

    [SerializeField] float distanceToGround;
    public LayerMask groundLayer;
    bool onGround;

    float targetAngle;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float inputY = Input.GetAxisRaw("Vertical") * moveSpeed;
        Vector3 direction = new Vector3(inputX, rb.velocity.y, inputY).normalized;

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

        if (direction.magnitude >= 0.1f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            
            transform.forward = direction * Time.deltaTime;
        }

        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        Vector3 gravityForce = gravityDirection.normalized * gravityStrength;
        rb.AddForce(gravityForce, ForceMode.Acceleration);
    }
}
