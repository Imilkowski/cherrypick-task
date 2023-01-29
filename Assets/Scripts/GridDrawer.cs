using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridDrawer : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private RectTransform _boardBackground;
    [SerializeField] private RectTransform _wallsParent;
    [SerializeField] private RectTransform _itemsParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _itemPrefab;

    [Header("Item Sprites")]
    [SerializeField] private List<Sprite> _itemSpritesList;

    //sets overall visual size of the board
    public void SetBackgroundSize(Vector2Int newSize)
    {
        _boardBackground.sizeDelta = newSize;
    }

    //spawns all the walls
    public void DrawWalls()
    {
        //TODO: draw walls in more optimized manner
        for (int i = 0; i < GridManager.Instance.gridSize.y; i++)
        {
            for (int j = 0; j < GridManager.Instance.gridSize.x; j++)
            {
                if(GridManager.Instance.gridElementsArray[i, j].type == GridManager.ElementType.Wall)
                {
                    Transform wallTransform = Instantiate(_wallPrefab, Vector3.zero, Quaternion.identity, _wallsParent).transform;
                    Vector3 pos = new Vector3(j, -i, 0) * GridManager.ELEMENT_SIZE;
                    wallTransform.localPosition = GridManager.Instance.startPos + pos;
                }
            }
        }
    }

    //spawns an item
    public GameObject DrawItem(Vector2Int itemIndex, GridManager.ElementType itemType, Vector3 startPos)
    {
        //TODO: object pooling
        Transform itemTransform = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _itemsParent).transform;
        Vector3 pos = new Vector3(itemIndex.x, -itemIndex.y, 0) * GridManager.ELEMENT_SIZE;
        Vector3 targetPos = GridManager.Instance.startPos + pos;

        itemTransform.localPosition = startPos;

        Image itemImage = itemTransform.GetComponent<Image>();
        switch (itemType)
        {
            case GridManager.ElementType.ItemGreen:
                itemImage.sprite = _itemSpritesList[1];
                break;

            case GridManager.ElementType.ItemRed:
                itemImage.sprite = _itemSpritesList[0];
                break;

            case GridManager.ElementType.ItemBlue:
                itemImage.sprite = _itemSpritesList[2];
                break;

            default:
                break;
        }

        //TODO: change to not use LeanTween
        float distance = Vector3.Distance(startPos, targetPos);
        LeanTween.moveLocal(itemTransform.gameObject, targetPos, distance / 500);

        return itemTransform.gameObject;
    }
}
