using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPlayerController : MonoBehaviour
{
    Rigidbody rb;
    SlowmotionController slowmotionController;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    [Tooltip("The maximum speed the round player can reach.")]
    public float maxSpeed;
    [Tooltip("The amount of force that will be applied when you click.")]
    public float clickStrength = 500f;


    public Vector3 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slowmotionController = FindObjectOfType<SlowmotionController>();
    }

    void Update()
    {
        InputManager();
        LookAtMousePosition();
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }        
    }

    private void InputManager()
    {
        bool canMove = Input.GetMouseButton(0);
        if (canMove)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;

            slowmotionController.DoSlowMotion();

            if (plane.Raycast(ray, out enter))
            {
                var hitPoint = ray.GetPoint(enter);
                var mouseDirection = hitPoint - gameObject.transform.position;
                mouseDirection = mouseDirection.normalized;
                rb.AddForce(mouseDirection * clickStrength, ForceMode.VelocityChange);
            }
        }
    }

    private void LookAtMousePosition()
    {
        Transform roundPlayerShootAt = transform.GetChild(0);

        Vector3 v3T = Input.mousePosition;
        v3T.z = Mathf.Abs(Camera.main.transform.position.y - roundPlayerShootAt.position.y);
        v3T = Camera.main.ScreenToWorldPoint(v3T);
        roundPlayerShootAt.LookAt(v3T);
    }
}
