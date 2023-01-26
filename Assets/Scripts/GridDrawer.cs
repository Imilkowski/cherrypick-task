using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridDrawer : MonoBehaviour
{
    [SerializeField] private RectTransform _boardBackground;

    [Header("Prefabs")]
    [SerializeField] private GameObject _wallPrefab;

    //sets overall visual size of the board
    public void SetBackgroundSize(Vector2Int newSize)
    {
        _boardBackground.sizeDelta = newSize;
    }
}
