using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Assignables")]
    [SerializeField] private GridDrawer gridDrawerComponent;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //initializes the array containg all the needed data and a visual grid
    public void InitializeGrid(Vector2Int gridSize)
    {
        gridDrawerComponent.SetBackgroundSize(gridSize * 32);
    }
}
