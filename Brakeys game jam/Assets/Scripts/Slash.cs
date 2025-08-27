using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Slash : MonoBehaviour
{
    public GameObject effect;
    [HideInInspector] public AnimationCurve Speed;
    [HideInInspector] public float Timer = 0;
    public float lifeTimer;
    public float damage;
    void Awake()
    {
        SoundManager.PlaySound(SoundType.slash);
    }

    // Update is called once per frame
    void Update()
    {
        
        Timer += Time.deltaTime;
        float Finalspeed = Speed.Evaluate(Timer);

        if(transform.eulerAngles.z == 0)
            transform.position = new Vector3(transform.position.x + Finalspeed, transform.position.y, 0f);
        else
            transform.position = new Vector3(transform.position.x - Finalspeed, transform.position.y, 0f);

        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();
        if(health != null)
        {
            health.TakeDammage(damage);
            GameObject slash = Instantiate(effect, Physics2D.ClosestPoint(collision.transform.position, this.GetComponent<Collider2D>()), Quaternion.identity);
            SoundManager.PlaySound(SoundType.hit);
            Destroy(slash, .7f);
        }
    }
    
}
