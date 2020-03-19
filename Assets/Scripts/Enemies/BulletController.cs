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
        StartCoroutine(GetDestroy());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("RoundPlayer"))
        {
            // Shake camera
            Destroy(gameObject);
        }
    }

    IEnumerator GetDestroy()
    {
        if (gameObject != null)
        {
            float trailStart = gameObject.transform.GetChild(0).GetComponent<TrailRenderer>().startWidth;
            float _zero = 0f;

            Sequence s = DOTween.Sequence();

            yield return new WaitForSeconds(2f);

            s.Append(gameObject.transform.DOScale(0f, 1f));
            DOVirtual.Float(trailStart, _zero, 1f, (zero) => gameObject.transform.GetChild(0).GetComponent<TrailRenderer>().startWidth = zero);

            // WaitForSeconds runs out of time with respect to the DOTween sequence, so we must take it into account to indicate how many seconds to wait.
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
    }
}
