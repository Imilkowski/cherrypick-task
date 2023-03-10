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

        return new Vector2Int((int)(fixedPos.x / 32), -(int)(fixedPos.y / 32));
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
        Vector2Int indexPos = GetIndexPos(true);

        if(GridManager.Instance.gridElementsArray[indexPos.y, indexPos.x].type != GridManager.ElementType.Empty)
        {
            GridManager.Instance.ring.Reset();
            indexPos = GridManager.Instance.GetNearestEmpty(indexPos);

            rectTransform.localPosition = GridManager.Instance.startPos + new Vector3(indexPos.x * 32, -indexPos.y * 32, 0);
        }

        GridManager.Instance.MoveSpawnerIndex(indexPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = cameraComponent.ScreenToWorldPoint(Input.mousePosition);
    }
}
