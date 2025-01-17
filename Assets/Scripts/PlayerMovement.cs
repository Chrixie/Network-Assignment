using UnityEngine;
using Unity.Netcode;
using UnityEngine.EventSystems;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 7f;

    public Transform orientation;
    Vector3 moveDirection;
    Rigidbody rb;

    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public float groundDrag;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }
    void Update()
    {
        if (!IsOwner)
            return;

        SpeedControl();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }
    private void FixedUpdate()
    {
        if (!IsOwner) return;
        MovePlayer();
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput == 0 && verticalInput == 0)
        {
            // Stop the character by setting velocity to zero
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            // Determine movement direction
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitVel.x, rb.linearVelocity.y, limitVel.z);
        }
    }
}
