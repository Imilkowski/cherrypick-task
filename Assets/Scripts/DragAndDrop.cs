using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Camera cameraComponent;

    private RectTransform rectTransform;

    private Vector2Int GetIndexPos(bool moveSpawner)
    {
        Vector3 relativePos = rectTransform.localPosition - GridManager.Instance.startPos;
        Vector3 fixedPos = new Vector3(Mathf.Round(relativePos.x / 32) * 32, Mathf.Round(relativePos.y / 32) * 32, 0);

        if (moveSpawner)
        {
            rectTransform.localPosition = GridManager.Instance.startPos + fixedPos;
        }

        return new Vector2Int(-(int)(fixedPos.y / 32), (int)(fixedPos.x / 32));
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cameraComponent = Camera.main;

        SetNewSpawnerPos();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2Int indexPos = GetIndexPos(false);
        GridManager.Instance.gridElementsArray[indexPos.y, indexPos.x].type = GridManager.ElementType.Empty;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetNewSpawnerPos();
    }

    private void SetNewSpawnerPos()
    {
        //TODO: check in the same system as items - check if there is no wall at this index
        Vector2Int indexPos = GetIndexPos(true);
        GridManager.Instance.MoveSpawnerIndex(indexPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta * (cameraComponent.orthographicSize / 320);
    }
}
