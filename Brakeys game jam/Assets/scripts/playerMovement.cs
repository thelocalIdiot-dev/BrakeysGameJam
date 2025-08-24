using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    private float InputX;
    private Rigidbody2D rb;
    private float moveInput;
    public float speed = 5f;
    [Range(0, 0.3f)] public float movementSmoothing;
    private Vector3 velocity = Vector3.zero;
    private bool facingRight = true;

    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Vector3 groundCheckSize;
    private bool isGrounded;
    private bool jumping;
    [Header("Wall Jumping")]
    public Transform wallCheck;
    public Vector3 wallCheckSize;
    private bool isTouchingWall;
    private bool sliding;
    public float slideSpeed = 2f;
    public float wallJumpForceX;
    public float wallJumpForceY;

    private bool wallJumping;


    public float wallCoyoteTime = 0.3f;
    private float wallCoyoteCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        InputX = Input.GetAxisRaw("Horizontal");
        moveInput = InputX * speed;

        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }

        // Comprobamos sliding
        if (!isGrounded && isTouchingWall && InputX != 0)
        {
            sliding = true;
        }
        else
        {
            sliding = false;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
        isTouchingWall = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, groundLayer);

        // Manejo del "wall coyote time"
        if (isTouchingWall && InputX != 0)
        {
            wallCoyoteCounter = wallCoyoteTime;
        }
        else
        {
            wallCoyoteCounter -= Time.fixedDeltaTime;
        }

        Move(moveInput * Time.fixedDeltaTime, jumping);
        jumping = false;

        if (sliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slideSpeed, float.MaxValue));
        }
    }

    private void Move(float move, bool jump)
    {
        if (!wallJumping)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
        }

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        if (isGrounded && jump && !sliding)
        {
            Jump();
        }

        if (wallCoyoteCounter > 0f && jump && !isGrounded)
        {
            JumpWall();
        }
    }

    private void JumpWall()
    {
        wallCoyoteCounter = 0f;

        if (isTouchingWall)
        {
            rb.velocity = new Vector2(wallJumpForceX * -InputX, wallJumpForceY);
        }
        else
        {
            rb.velocity = new Vector2(wallJumpForceX * InputX, wallJumpForceY);
        }


        StartCoroutine(StopWallJumping());
    }


    private IEnumerator StopWallJumping()
    {
        wallJumping = true;
        yield return new WaitForSeconds(wallCoyoteTime);
        wallJumping = false;
        yield return new WaitForSeconds(1f);
        wallCoyoteCounter = 0f;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void Jump()
    {
        isGrounded = false;
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Saw"))
        {
            deathManager.instance.die();
        }

    }
}
