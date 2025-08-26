using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth;

    public float health;

    public GameObject deathPartical;

    private void Awake()
    {
        health = MaxHealth;
    }

    public void TakeDammage(float damage)
    {
        Debug.Log("damage");

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathPartical, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
