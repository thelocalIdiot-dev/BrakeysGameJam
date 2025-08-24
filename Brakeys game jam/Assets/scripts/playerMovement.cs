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

    public static playerMovement instance;
    void Start()
    {
        instance = this;
        PlayerAnimator = GetComponentInChildren<Animator>();       
    }

    void Update()
    {
        GetInput();

        //---Horizontal Movement---//
        Vector2 movement = new Vector2 (HorizontalInput * speed, rb.velocity.y);
        rb.velocity = movement;

        //---Jumping---//
        if (Input.GetButtonDown("Jump") && grounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            //SoundManager.PlaySound(SoundType.jump);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
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
        //footstepSounds();
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
            deathManager.instance.die();
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
            Instantiate(land, landPosition.position, Quaternion.identity);
            
        }
    }
}
