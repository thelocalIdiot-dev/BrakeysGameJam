using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstepManager : MonoBehaviour
{
    public void footStep()
    {
        if(playerMovement.instance.grounded() && Input.GetAxisRaw("Horizontal") != 0)
        {
            SoundManager.PlaySound(SoundType.footstep);
        }
    }
}
