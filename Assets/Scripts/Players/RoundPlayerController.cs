using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPlayerController : MonoBehaviour
{
    Rigidbody rb;
    SlowmotionController slowmotionController;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private Material roundPlayerMat;
    private Material roundPlayerTrailMat;

    [Tooltip("The maximum speed the round player can reach.")]
    public float maxSpeed;
    [Tooltip("The amount of force that will be applied when you click.")]
    public float clickStrength = 500f;

    private string currentCracksColorMat;
    private string currentTrailColorMat;
    private string currentEdgeColorMat;

    private List<Color> colorsToChange;
    private Queue<Color> colorToChangeStack;

    private GameObject[] wallGrids;
    private string currentWallGridsColorMat;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slowmotionController = FindObjectOfType<SlowmotionController>();

        roundPlayerMat = GetComponent<MeshRenderer>().material;
        roundPlayerTrailMat = GetComponent<TrailRenderer>().material;

        currentCracksColorMat = roundPlayerMat.shader.GetPropertyName(1);
        currentEdgeColorMat = roundPlayerMat.shader.GetPropertyName(2);
        currentTrailColorMat = roundPlayerTrailMat.shader.GetPropertyName(0);

        colorsToChange = new List<Color>();

        colorsToChange.Add(roundPlayerMat.GetColor("Color_D532411"));
        colorsToChange.Add(roundPlayerMat.GetColor("Color_1E16DB93"));

        colorToChangeStack = new Queue<Color>();

        for (int i = 0; i < colorsToChange.Count; i++)
        {
            colorToChangeStack.Enqueue(colorsToChange[i]);
        }

        wallGrids = GameObject.FindGameObjectsWithTag("WallGrid");
        currentWallGridsColorMat = wallGrids[0].GetComponent<MeshRenderer>().material.shader.GetPropertyName(0);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            AnimateMaterial();
        }
    }

    private void AnimateMaterial()
    {
        if (colorToChangeStack.Count > 0)
        {
            Color colorToChange = colorToChangeStack.Dequeue();

            Sequence s = DOTween.Sequence();
            s.Append(roundPlayerMat.DOColor(colorToChange, currentCracksColorMat, .25f));
            s.Join(roundPlayerMat.DOColor(colorToChange, currentEdgeColorMat, .25f));
            s.Join(roundPlayerTrailMat.DOColor(colorToChange, currentTrailColorMat, .25f));
            for (int i = 0; i < wallGrids.Length; i++)
            {
                s.Join(wallGrids[i].GetComponent<MeshRenderer>().material.DOColor(colorToChange, currentWallGridsColorMat, .25f));
            }
        }
    }
}