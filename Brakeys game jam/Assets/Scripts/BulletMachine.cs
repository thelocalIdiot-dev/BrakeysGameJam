using UnityEngine;

public class BulletMachine : MonoBehaviour
{
    public enum Direccion { right, left, up, down }
    public Direccion direccion = Direccion.right;

    public GameObject balaPrefab;
    public float velocity = 5f;
    public float time = 2f;

    private float temporizador;

    void Update()
    {
        temporizador += Time.deltaTime;

        if (temporizador >= time)
        {
            Disparar();
            temporizador = 0f;
        }
    }

    void Disparar()
    {
        Vector2 dir = Vector2.right;

        switch (direccion)
        {
            case Direccion.right: dir = Vector2.right; break;
            case Direccion.left: dir = Vector2.left; break;
            case Direccion.up: dir = Vector2.up; break;
            case Direccion.down: dir = Vector2.down; break;
        }

        GameObject bullet = Instantiate(balaPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = dir * velocity;
    }
}
