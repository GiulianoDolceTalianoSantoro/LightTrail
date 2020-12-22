using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundPlayerController : MonoBehaviour
{
    public Rigidbody rb;

    SlowmotionController slowmotionController;
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

    public bool goalReached;
    public int currentLife;
    public bool isRetry;

    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    // Temp
    [HideInInspector]
    public string time;

    void Start()
    {
        InitPlayer();

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

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        
        if (!goalReached && !GameManager.instance.gameIsPaused)
        {
            InputManager();
            LookAtMousePosition();
        }
    }

    void InitPlayer()
    {
        transform.localPosition = new Vector3(0, 1f, 0f);
        transform.localScale = new Vector3(2f, 2f, 2f);
        rb.velocity = Vector3.zero;
    }

    private void InputManager()
    {
        bool canMove = Input.GetMouseButton(0);

        if (canMove)
        {
            var ray = GameManager.instance.gameCamera.ScreenPointToRay(Input.mousePosition);

            slowmotionController.slowdownLength = .5f;
            slowmotionController.DoSlowMotion(0.02f);

            if (plane.Raycast(ray, out float enter))
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
        v3T.z = Mathf.Abs(GameManager.instance.gameCamera.transform.position.y - roundPlayerShootAt.position.y);
        v3T = GameManager.instance.gameCamera.ScreenToWorldPoint(v3T);
        roundPlayerShootAt.LookAt(v3T);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            AnimateMaterial();
            currentLife--;
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