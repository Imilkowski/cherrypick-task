using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridDrawer : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private RectTransform _boardBackground;
    [SerializeField] private RectTransform _wallsParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject _wallPrefab;

    //sets overall visual size of the board
    public void SetBackgroundSize(Vector2Int newSize)
    {
        _boardBackground.sizeDelta = newSize;
    }

    public void DrawWalls()
    {
        //spawn walls
        Instantiate(_wallPrefab, GridManager.Instance.startPos, Quaternion.identity, _wallsParent);
        //TODO: instead of spawning in world pos, set the local pos from parent
    }
}
