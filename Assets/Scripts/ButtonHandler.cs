using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Spawner spawnerComponent;

    private bool pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }

    void Update()
    {
        if (pressed)
        {
            spawnerComponent.SpawnAnItem();
        }
    }
}
