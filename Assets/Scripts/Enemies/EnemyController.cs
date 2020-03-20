using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody rb;
    Transform playerPivot;

    [Header("Movement")]
    [Tooltip("The speed of movement.")]
    public float moveSpeed = 5f;
    [Tooltip("The minimum distance to the player. Once it's passed, he'll start chasing him.")]
    public float minDist = 20f;
    [Tooltip("The maximum distance to the player. If it's passed it will stop chasing him.")]
    public float maxDist = 50f;
    
    [Header("Fire")]
    [Tooltip("The prefab of the bullet to instantiate when shooting.")]
    public GameObject bulletPrefab;
    [Tooltip("The particles for the shot.")]
    public ParticleSystem firePart;
    [Tooltip("The distance from which he will start shooting.")]
    public float fireDist = 25f;
    [Tooltip("The interval between each shot.")]
    public float fireRate = .25f;
    float fireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPivot = GameObject.FindGameObjectWithTag("RoundPlayer").transform.Find("RoundPlayerPivot/RoundPlayerShootAt");
        Debug.Log(playerPivot);
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
            Fire();
        }
    }

    void Fire()
    {
        fireTime += Time.deltaTime;

        if (fireTime >= fireRate)
        {
            firePart.Play();

            fireTime = 0;
            Transform bulletPoint = transform.GetChild(0);
            Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation);
        }
    }
}
