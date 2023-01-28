using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanningChecker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CameraController cameraController;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        cameraController.StartPanning();
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        cameraController.StopPanning();
    }
}
