using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeManager
{
    public static void Shake(CinemachineImpulseSource Impulsesource, float force = 1f)
    {


        // Generate an impulse with the specified force
        Impulsesource.GenerateImpulseWithForce(force);
    }

}
