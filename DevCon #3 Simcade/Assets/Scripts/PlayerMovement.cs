using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;

    [SerializeField, Range(0, 50)] float moveSpeed = 10f;
    [SerializeField, Range(0, 25)] float jumpSpeed = 5f;

    public Vector3 gravityDirection = new Vector3(0, -1f, 0); // Default gravity direction 
    [SerializeField, Range(0, 25)] float gravityStrength = 9f;  // The strength of the gravity force

    [SerializeField, Range(0, 5)] int maxAirJumps = 0; // The maximum number of air jumps
    public int jumpPhase = 0; // Tracks the current phase of jumping

    [SerializeField] float distanceToGround;
    public LayerMask groundLayer;
    bool onGround;
    bool isJumping;

    private Vector3 climbDirection;
    [SerializeField, Range(0, 1)] float climbSpeed = 0.3f;
    public Vector3 climbJumpDirection = new Vector3(0, 0, -1f); // Default gravity direction
    [SerializeField, Range(0, 100)] float climbingJumpForce = 5f;
    bool isClimbing;
    [SerializeField] float distanceToSurface;
    public LayerMask climbLayer;

    float targetAngle;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log(isClimbing);
        float inputX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float inputY = Input.GetAxisRaw("Vertical") * moveSpeed;

        if (Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputY) > 0.1f)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        RaycastHit hitClimbable;
        if (Physics.Raycast(transform.position, Vector3.forward, out hitClimbable, distanceToSurface, climbLayer) && Input.GetKey(KeyCode.LeftShift))
        {
            if (!isClimbing && hitClimbable.normal.y < 0.5f)
            {
                isClimbing = true;
                anim.SetBool("isClimbing", true);
            }
        }
        else
        {
            isClimbing = false;
            anim.SetBool("isClimbing", false);
        }

        if (isClimbing)
        {
            // Calculate climbing direction based on surface normal
            climbDirection = Vector3.up; // Assume climbing vertically up

            // Apply climbing force based on input
            rb.velocity = new Vector3(inputX * climbSpeed, inputY * climbSpeed, rb.velocity.z);

            if (climbDirection.magnitude >= 0.1f)
            {
                targetAngle = Mathf.Atan2(climbDirection.x, climbDirection.y) * Mathf.Rad2Deg;
                transform.up = climbDirection * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isClimbing)
            {
                jumpPhase += 1; // Tracks air jumps
                Vector3 climbJumpForce = climbJumpDirection.normalized * climbingJumpForce;
                rb.AddForce(climbJumpForce, ForceMode.Impulse);

                anim.SetBool("isJumping", true);
                isJumping = true;
            }
            jumpPhase = 0;
        }

        if (!isClimbing)
        {
            Vector3 direction = new Vector3(inputX, rb.velocity.y, inputY).normalized;
            rb.velocity = new Vector3(inputX, rb.velocity.y, inputY);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, distanceToGround, groundLayer))
            {
                Debug.Log("Ground detected");
                jumpPhase = 0;
                onGround = true;
                anim.SetBool("isGrounded", true);
                anim.SetBool("isJumping", false);
                isJumping = false;
                anim.SetBool("isFalling", false);
            }
            else
            {
                anim.SetBool("isGrounded", false);
                onGround = false;

                if ((isJumping && rb.velocity.y < 0) || rb.velocity.y < -2)
                {
                    anim.SetBool("isFalling", true);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && onGround || Input.GetKeyDown(KeyCode.Space) && jumpPhase < maxAirJumps)
            {
                jumpPhase += 1; // Tracks air jumps
                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
                anim.SetBool("isJumping", true);
                isJumping = true;
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
}
