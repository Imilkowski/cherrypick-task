using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class GridDrawer : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private RectTransform _boardBackground;
    [SerializeField] private RectTransform _itemsParent;

    [SerializeField] private Grid _tileMapGrid;
    private Tilemap _tileMap;

    [Header("Prefabs")]
    [SerializeField] private GameObject _itemPrefab;

    [Header("Item Sprites")]
    [SerializeField] private List<Sprite> _itemSpritesList;

    [SerializeField] private TileBase _wallTile;
    [SerializeField] private TileBase _itemRedTile;
    [SerializeField] private TileBase _itemGreenTile;
    [SerializeField] private TileBase _itemBlueTile;

    void Awake()
    {
        LeanTween.init(10000);

        _tileMap = _tileMapGrid.transform.GetChild(0).GetComponent<Tilemap>();
    }

    //sets overall visual size of the board
    public void SetBackgroundSize(Vector2Int newSize)
    {
        _boardBackground.sizeDelta = newSize;
    }

    //spawns all the walls
    public void DrawWalls()
    {
        _tileMapGrid.transform.localPosition = GridManager.Instance.startPos - new Vector3(16, 16, 0);

        for (int i = 0; i < GridManager.Instance.gridSize.y; i++)
        {
            for (int j = 0; j < GridManager.Instance.gridSize.x; j++)
            {
                if (GridManager.Instance.gridElementsArray[i, j].type == GridManager.ElementType.Wall)
                {
                    _tileMap.SetTile((new Vector3Int(j, -i, 0)), _wallTile);
                }
            }
        }
    }

    public void ClearTile(Vector2Int indexPos)
    {
        _tileMap.SetTile(new Vector3Int(indexPos.x, -indexPos.y, 0), null);
    }

    //spawns an item
    public GameObject DrawItem(Vector2Int itemIndex, GridManager.ElementType itemType, Vector3 startPos)
    {
        //TODO: could use object pooling
        Transform itemTransform = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _itemsParent).transform;
        Vector3 pos = new Vector3(itemIndex.x, -itemIndex.y, 0) * GridManager.ELEMENT_SIZE;
        Vector3 targetPos = GridManager.Instance.startPos + pos;

        itemTransform.localPosition = startPos;

        Image itemImage = itemTransform.GetComponent<Image>();
        TileBase tileType = null;
        switch (itemType)
        {
            case GridManager.ElementType.ItemGreen:
                itemImage.sprite = _itemSpritesList[1];
                tileType = _itemGreenTile;
                break;

            case GridManager.ElementType.ItemRed:
                itemImage.sprite = _itemSpritesList[0];
                tileType = _itemRedTile;
                break;

            case GridManager.ElementType.ItemBlue:
                itemImage.sprite = _itemSpritesList[2];
                tileType = _itemBlueTile;
                break;

            default:
                break;
        }

        //TODO: change to not use LeanTween
        float distance = Vector3.Distance(startPos, targetPos);
        LeanTween.moveLocal(itemTransform.gameObject, targetPos, distance / 500).setOnComplete(ConvertToTile);

        void ConvertToTile()
        {
            Vector2Int indexPos = GridManager.Instance.GetIndexPos(targetPos);
            //TODO: could use object pooling
            Destroy(GridManager.Instance.gridElementsArray[indexPos.y, indexPos.x].heldElement.gameObject);

            _tileMap.SetTile(new Vector3Int(indexPos.x, -indexPos.y, 0), tileType);
        }

        return itemTransform.gameObject;
    }
}
