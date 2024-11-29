using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
    bool canClimb;
    [SerializeField] float distanceToSurface;
    public LayerMask climbLayer;

    [SerializeField, Range(0, 20)] float maxStamina = 10f;    // Maximum stamina value
    public float currentStamina;       // Current stamina
    [SerializeField, Range(0, 10)] float staminaDrainRate = 2f; // Rate at which stamina drains when in air
    [SerializeField, Range(0, 10)] float staminaRegenRate = 5f; // Rate at which stamina refills when on the ground
    public StaminaBar staminaBar;

    float targetAngle;

    public GameManager manager;
    [SerializeField, Range(0, 0.5f)] float scaleX = 0.1f;
    [SerializeField, Range(0, 0.5f)] float scaleY = 0.1f;
    [SerializeField, Range(0, 0.5f)] float scaleZ = 0.1f;
    [SerializeField, Range(0, 0.5f)] float scaleMass = 0.05f;

    public GameObject acornPrefab; 
    public Transform firingPoint; 
    public float shootForce = 20f; 
    public float lifetime = 5f;

    private void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
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

        ScaleFactor();

        if (Input.GetKeyDown(KeyCode.C))
        {
            FireProjectile();
        }

        RaycastHit hitClimbable;
        if (Physics.Raycast(transform.position, Vector3.forward, out hitClimbable, distanceToSurface, climbLayer) && Input.GetKey(KeyCode.LeftShift) && canClimb)
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

            if (currentStamina > 0)
            {
                currentStamina -= staminaDrainRate * Time.deltaTime;
                staminaBar.SetStamina(currentStamina);
            }
            else if (currentStamina <= 0)
            {
                canClimb = false;
                currentStamina = 0f;
            }
        }

        if (!isClimbing)
        {
            Vector3 direction = new Vector3(inputX, rb.velocity.y, inputY).normalized;
            rb.velocity = new Vector3(inputX, rb.velocity.y, inputY);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, distanceToGround, groundLayer))
            {
                jumpPhase = 0;
                onGround = true;
                anim.SetBool("isGrounded", true);
                anim.SetBool("isJumping", false);
                isJumping = false;
                anim.SetBool("isFalling", false);

                if (currentStamina < maxStamina)
                {
                    currentStamina += staminaRegenRate * Time.deltaTime;
                    staminaBar.SetStamina(currentStamina);
                }
                else if (currentStamina >= maxStamina) 
                {
                    currentStamina = maxStamina;
                    canClimb = true;
                }
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

    void ScaleFactor()
    {
        int AcornAmount = manager.GetAcornCount();

        // Scale each axis independently based on the scaleFactor
        float xScale = 0.4f + (AcornAmount * scaleX);
        float yScale = 0.4f + (AcornAmount * scaleY); 
        float zScale = 0.4f + (AcornAmount * scaleZ);

        xScale = Mathf.Max(xScale, 0.4f);
        yScale = Mathf.Max(yScale, 0.4f);
        zScale = Mathf.Max(zScale, 0.4f);

        if (AcornAmount > 0)
        {
            transform.localScale = new Vector3(xScale, yScale, zScale);
        }

        if (AcornAmount > 5) 
        {
            maxAirJumps = 0;
        }
        else
        {
            maxAirJumps = 1;
        }

        rb.mass = 0.4f + (AcornAmount * scaleMass);

        distanceToGround = 1f + (AcornAmount * 0.2f);
        distanceToSurface = 1f + (AcornAmount * 0.2f);

        moveSpeed = 7f - (AcornAmount * 0.18f);
        jumpSpeed = 9f - (AcornAmount * 0.18f);

        gravityStrength = 2f + (AcornAmount * 0.05f);

        staminaDrainRate = 2f + (AcornAmount * 0.12f);
        staminaRegenRate = 7f - (AcornAmount * 0.3f);
    }
    public void ResetScale()
    {
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    void FireProjectile()
    {
        if (manager.acornCount > 0)
        {
            // Instantiate the projectile at the firing point
            GameObject projectile = Instantiate(acornPrefab, firingPoint.position, firingPoint.rotation);

            // Add force to the projectile to make it move forward
            Rigidbody acornRigidBody = projectile.GetComponent<Rigidbody>();
            if (acornRigidBody != null)
            {
                acornRigidBody.velocity = firingPoint.forward * shootForce; // Set the velocity to shoot forward
            }
            manager.ReduceAcorn();
            // Destroy the projectile after a certain amount of time if it doesn't hit anything
            Destroy(projectile, lifetime);
        }
    }
}
