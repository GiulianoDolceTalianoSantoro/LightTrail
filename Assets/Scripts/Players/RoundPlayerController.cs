using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private SlowmotionController slowmotionController;

    public float maxSpeed;
    public float clickStrength = 500f;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slowmotionController = FindObjectOfType<SlowmotionController>();
    }

    void Update()
    {
        InputManager();
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
}
