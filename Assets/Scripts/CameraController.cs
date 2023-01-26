using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    public void SetZoomLevel(float level)
    {
        mainCamera.orthographicSize = level * 320;
    }
}
