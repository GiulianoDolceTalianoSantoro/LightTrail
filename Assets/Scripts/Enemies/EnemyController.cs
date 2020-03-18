using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody rb;
    Transform playerPivot;
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;

    public float minDist = 20f;
    public float maxDist = 50f;
    public float fireDist = 25f;

    float fireTime;
    float fireRate = .25f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPivot = GameObject.FindGameObjectWithTag("RoundPlayer").transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    void EnemyMovement()
    {
        transform.LookAt(playerPivot);
        float distance = Vector3.Distance(transform.position, playerPivot.position);

        if (distance >= minDist && distance <= maxDist)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        if(Vector3.Distance(transform.position, playerPivot.position) <= fireDist)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        fireTime += Time.deltaTime;

        if (fireTime >= fireRate)
        {
            fireTime = 0;
            Transform bulletPoint = transform.GetChild(0);
            Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation);
        }
    }
}
