//using Dialog;
//using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class playerMovement : MonoBehaviour
{
    [Header("values")]
    public float speed = 10;
    public float jumpPower = 20;
    public float upGravity = 5;
    public float fallingGravity = 7;
    [Header("Reference")]
    public Transform groundCheck;
    public Transform WallJumpCheck;
    public Rigidbody2D rb;
    public LayerMask groundLayer;
    public Animator PlayerAnimator;
    public Transform landPosition;
    [Header("Input")]
    public float HorizontalInput;
    public bool Jumping;
    [Header("Debug")]
    public bool facingRight = true;
    public GameObject land;
    [Header("Input")]
    public float wallJumpForceX = 30;
    public float wallJumpForceY = 20;
    public float wallJumpDetectDistance = 0.8f;
    public float wallJumpTimer = 0.5f;
    public LayerMask wallJumpable;
    public bool onWall;
    public bool wallJumping;
    RaycastHit2D wallLeft;
    RaycastHit2D wallRight;


    public static playerMovement instance;
    GameObject LSP;
    public float TG;
    void Start()
    {
        instance = this;
        PlayerAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        GetInput();


        //---Land Smoke---//
        if (grounded())
            TG += 1;        
        else       
            TG = 0;

        if (TG == 1)       
            LSP = Instantiate(land, landPosition.position, Quaternion.LookRotation(new Vector3(0, 90, 0)));       


        Destroy(LSP, .5f);
        //---Horizontal Movement---//
        Vector2 movement = new Vector2(HorizontalInput * speed, rb.velocity.y);        
        if (!wallJumping)
        {
            rb.velocity = movement;
        }
        //---Jumping---//
        if (!wallJumping)
        {
            if (Jumping && grounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                //SoundManager.PlaySound(SoundType.jump);
            }
            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }
        
        //---Custom Gravity---//
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravity;
        }
        else
        {
            rb.gravityScale = upGravity;
        }

        //---Visuals---//
        HandleFlip();
        SetAnimationParameters();


        //---wall jump---//
        DetectWallJump();
    }

    void DetectWallJump()
    {
        wallLeft = Physics2D.Raycast(WallJumpCheck.position, Vector2.left, wallJumpDetectDistance, wallJumpable);
        wallRight = Physics2D.Raycast(WallJumpCheck.position, Vector2.right, wallJumpDetectDistance, wallJumpable);

        if(wallLeft || wallRight)
        {
            onWall = true;
        }
        else
        {
            onWall = false;
        }

        if (onWall && Jumping)
        {
            if(wallLeft.collider != null)
            {
                wallJump(wallJumpForceX, wallJumpForceY);
            }
            else if (wallRight.collider != null)
            {
                wallJump(-wallJumpForceX, wallJumpForceY);
            }
        }
    }

    void wallJump(float side, float up)
    {
        wallJumping = true;
        rb.velocity = new Vector2(side, up);
        StartCoroutine(stopWallJump());
    }

    IEnumerator stopWallJump()
    {
        yield return new WaitForSeconds(wallJumpTimer);
        wallJumping = false;
    }

    void GetInput()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        Jumping = Input.GetButtonDown("Jump");
    }


    public bool grounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void HandleFlip()
    {
        if (facingRight && HorizontalInput < 0f || !facingRight && HorizontalInput > 0f)
        {
            facingRight = !facingRight;
            Vector3 LocalScale = transform.localScale;
            LocalScale.x *= -1f;
            transform.localScale = LocalScale;
        }
    }

    void SetAnimationParameters()
    {
        PlayerAnimator.SetFloat("Speed X", math.abs(HorizontalInput));
        PlayerAnimator.SetFloat("Speed Y", rb.velocity.y);
        PlayerAnimator.SetBool("grounded", grounded());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.layer == 8 && !deathManager.instance.ded)
        {
            GameManager.instance.restart();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.layer == 8 && !deathManager.instance.ded)
        {
            deathManager.instance.die();
        }
        if (grounded())
        {
            //SoundManager.PlaySound(SoundType.land);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(WallJumpCheck.position, WallJumpCheck.position + Vector3.left * wallJumpDetectDistance);
        Gizmos.DrawLine(WallJumpCheck.position, WallJumpCheck.position + Vector3.right * wallJumpDetectDistance);
    }
}
