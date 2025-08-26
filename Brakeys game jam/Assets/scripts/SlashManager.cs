using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashManager : MonoBehaviour
{
    public GameObject SlashObj;

    public static SlashManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void Slash(Vector3 origin, AnimationCurve speed, float direction, Vector2 size, float damage)
    {        
        GameObject slash = Instantiate(SlashObj, origin, Quaternion.Euler(0, 0, direction));
        Slash SlashCode = slash.GetComponent<Slash>();
        SlashCode.lifeTimer = speed.keys[speed.length - 1].time;
        SlashCode.Speed = speed;
        SlashCode.damage = damage;
        slash.transform.localScale = size;
        
    }
}
