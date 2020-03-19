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

    // Start is called before the first frame update
    void Start()
    {
        normalFOV = Camera.main.fieldOfView;
        offset = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + offset;
        DoZoom();
    }

    void Zoom(bool state)
    {
        float fov = state ? zoomFOV : normalFOV;

        DOVirtual.Float(Camera.main.fieldOfView, fov, .1f, FieldOfView).SetEase(ease);
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
        Camera.main.fieldOfView = fov;
    }
}
