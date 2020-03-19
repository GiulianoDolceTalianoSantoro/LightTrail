using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Animator camShake;

    public void CamShake()
    {
        camShake.SetTrigger("Shake");
    }
}
