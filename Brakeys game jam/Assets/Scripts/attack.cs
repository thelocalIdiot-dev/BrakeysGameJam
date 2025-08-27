using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class attack : MonoBehaviour
{
    bool a1, CanAttack;

    public float attackCooldown = 0.3f;
    public float damage = 10;
    public AnimationCurve Speed;
    public Vector2 slashSize;
    public Transform origin;
    void Start()
    {
        CanAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && CanAttack)
        {
            slashAttack();
        }   
    }

    void slashAttack()
    {
        CanAttack = false;
        a1 = !a1;
        Animator animator = GetComponentInChildren<Animator>();


        if (!GetComponent<playerMovement>().facingRight)
            SlashManager.instance.Slash(transform.position, Speed, -180, slashSize, damage);
        else
            SlashManager.instance.Slash(transform.position, Speed, 0, slashSize, damage);


        animator.SetBool("IsSlash1", a1);
        animator.SetTrigger("attack");
        Invoke(nameof(resetAttack), attackCooldown);
    }

    void resetAttack()
    {
        CanAttack = true;
    }
}
