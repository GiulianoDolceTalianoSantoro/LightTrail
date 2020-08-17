using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCameraController : MonoBehaviour
{
    RoundPlayerController target;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<RoundPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
    }

    void LookAtPlayer()
    {
        transform.LookAt(target.transform);
    }
}
