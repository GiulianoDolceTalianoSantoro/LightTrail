using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowmotionController : MonoBehaviour
{
    public const float slowdownFactor = 0.1f;

    public float slowdownLength = 0.8f;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void DoSlowMotion(float SlowdownFactor = slowdownFactor)
    {
        Time.timeScale = SlowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
