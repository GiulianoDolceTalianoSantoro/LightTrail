using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    private Vector3 offset;

    public float zoomFOV = 10f;
    float normalFOV;
    public Ease ease;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        normalFOV = gameManager.mainCamera.fieldOfView;
        offset = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.mainCamera != null)
        {
            transform.position = target.transform.position + offset;
            DoZoom();
        }
    }

    void Zoom(bool state)
    {
        float fov = state ? zoomFOV : normalFOV;

        DOVirtual.Float(gameManager.mainCamera.fieldOfView, fov, .1f, FieldOfView).SetEase(ease);
    }

    void DoZoom()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Zoom(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Zoom(false);
        }
    }

    void FieldOfView(float fov)
    {
        gameManager.mainCamera.fieldOfView = fov;
    }
}
