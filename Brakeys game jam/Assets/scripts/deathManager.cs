using Cinemachine;
//using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class deathManager : MonoBehaviour
{
    public bool ded;

    public GameObject plr;

    public Transform position;

    //public GameObject deathParticale;

    //public GameObject DeathScreen;

    public static deathManager instance;

    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {

        ded = false;
        instance = this;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void effects()
    {
        // Instantiate(deathParticale, position.position, Quaternion.identity);
    }



    public void die()
    {
        //SoundManager.PlaySound(SoundType.death);
        //GameManager.instance.restart();
        //DeathScreen.SetActive(true);
        //ShakeManager.Shake(impulseSource);
        //effects();
        ded = true;
        Destroy(plr);
    }
}
