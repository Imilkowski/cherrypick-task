using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;

    private Vector3 touchStart;
    private bool isPanning;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Start()
    {
        SetZoomLevel(5);
    }

    public void SetZoomLevel(float level)
    {
        float sizeDependent = GridManager.Instance.gridSize.y / 100f;
        if(GridManager.Instance.gridSize.x > GridManager.Instance.gridSize.y)
        {
            sizeDependent = GridManager.Instance.gridSize.x / 100f;
        }

        mainCamera.orthographicSize = level * 320 * sizeDependent;
    }

    public void StartPanning()
    {
        touchStart = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        isPanning = true;
    }

    public void StopPanning()
    {
        isPanning = false;
    }

    void Update()
    {
        if (isPanning)
        {
            Vector3 moveDirection = touchStart - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mainCamera.transform.position += moveDirection;
        }
    }
}
