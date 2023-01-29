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
        //TODO: how to take width into consideration?
        mainCamera.orthographicSize = level * 320 * (GridManager.Instance.gridSize.y / 100f);
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
