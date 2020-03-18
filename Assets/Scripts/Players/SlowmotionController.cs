using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowmotionController : MonoBehaviour
{
    public float slowdownFactor = 0.5f;
    public float slowdownLength = 0.5f;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
