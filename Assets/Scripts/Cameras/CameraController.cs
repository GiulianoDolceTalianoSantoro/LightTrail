using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static Camera gameCamera;
    private Vector3 offset;
    [HideInInspector]
    public GameObject target;

    public float zoomFOV = 10f;
    float normalFOV;
    public Ease ease;

    // Start is called before the first frame update
    void Start()
    {
        gameCamera = this.GetComponent<Camera>();

        target = GameObject.FindGameObjectWithTag("RoundPlayer");

        normalFOV = gameCamera.fieldOfView;
        transform.parent.position = new Vector3(target.transform.position.x, target.transform.position.y + 50f, 0f);
        offset = transform.parent.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameCamera != null && !GameManager.instance.gameIsPaused)
        {
            transform.parent.position = target.transform.localPosition + offset;
            DoZoom();
        }
    }

    void Zoom(bool state)
    {
        float fov = state ? zoomFOV : normalFOV;

        DOVirtual.Float(gameCamera.fieldOfView, fov, .1f, FieldOfView).SetEase(ease);
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
        gameCamera.fieldOfView = fov;
    }
}
