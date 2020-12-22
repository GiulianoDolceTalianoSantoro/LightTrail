using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.ProBuilder;

public class BulletController : MonoBehaviour
{
    Rigidbody rb;

    [Tooltip("The speed with which the bullet will spawn.")]
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        StartCoroutine(GetDestroy(2));
    }

    private void Update()
    {
        if (GameManager.instance.gameIsPaused)
        {
            OnPause();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("RoundPlayer"))
        {
            Destroy(gameObject);
        }
    }

    [HideInInspector]
    public Vector3 bulletVelocityUntilPause;
    public void OnPause()
    {
        Time.timeScale = 0f;
        bulletVelocityUntilPause = rb.velocity;
        rb.velocity = Vector3.zero;
    }

    public void OnResume()
    {
        Time.timeScale = 1f;
        rb.velocity = bulletVelocityUntilPause;
    }

    IEnumerator GetDestroy(float time)
    {
        if (gameObject != null)
        {
            float trailStart = gameObject.transform.GetChild(0).GetComponent<TrailRenderer>().startWidth;

            Vector3 originalScale = transform.localScale;
            Vector3 destinationScale = Vector3.zero;

            float currentTime = 0.0f;

            do
            {
                transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
                gameObject.transform.GetChild(0).GetComponent<TrailRenderer>().startWidth = Mathf.Lerp(trailStart, 0f, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= time);

            Destroy(gameObject);
        }
    }
}
