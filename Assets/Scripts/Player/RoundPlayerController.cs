using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPlayerController : MonoBehaviour
{
    private Rigidbody rb;

    //private SlowMotionController slowMotionController;

    public float mass;
    public float maxSpeed;
    public float clickStrength;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    private GameObject dieParticles;

    public static bool isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //slowMotionController = FindObjectOfType<SlowMotionController>();
        isDead = false;
    }

    void Update()
    {
        rb.mass = mass;
        InputManager();
    }

    private void InputManager()
    {
        bool canMove = Input.GetMouseButton(0);
        if (canMove)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;

            //slowMotionController.DoSlowMotion();

            if (plane.Raycast(ray, out enter))
            {
                var hitPoint = ray.GetPoint(enter);
                var mouseDirection = hitPoint - gameObject.transform.position;
                mouseDirection = mouseDirection.normalized;
                rb.AddForce(mouseDirection * clickStrength, ForceMode.VelocityChange);
            }
        }

        //AudioManager.instance.PlaySound("");
    }
}
