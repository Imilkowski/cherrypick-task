using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int ELEMENT_SIZE = 32;

    public Vector2Int gridSize { get; private set; }
    public Vector3 startPos { get; private set; }

    public enum ElementType
    {
        Empty,
        Wall,
        Spawner,
        ItemGreen,
        ItemRed,
        ItemBlue
    }

    public class GridElement
    {
        public ElementType type;
        public GameObject heldElement;

        public GridElement(ElementType type, GameObject heldElement)
        {
            this.type = type;
            this.heldElement = heldElement;
        }
    }

    public static GridManager Instance;

    [Header("Assignables")]
    [SerializeField] private GridDrawer gridDrawerComponent;

    public GridElement[,] gridElementsArray;

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
        this.gridSize = gridSize;
        gridDrawerComponent.SetBackgroundSize(gridSize * 32);

        startPos = CalculateStartPos();

        gridElementsArray = new GridElement[gridSize.y, gridSize.x];
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                gridElementsArray[i, j] = new GridElement(ElementType.Empty, null);
            }
        }

        SetUpWalls();
    }
    
    //calculates top-left (0, 0 in the array) position in world space
    private Vector3 CalculateStartPos()
    {
        Vector3 pos = new Vector3();

        if(gridSize.x % 2 == 0)
        {
            pos.x = (gridSize.x / 2) * ELEMENT_SIZE - (int)(ELEMENT_SIZE * 0.5f);
        }
        else
        {
            pos.x = -(int)Mathf.Floor(gridSize.x / 2) * ELEMENT_SIZE;
        }

        if (gridSize.y % 2 == 0)
        {
            pos.y = (gridSize.y / 2) * ELEMENT_SIZE - (int)(ELEMENT_SIZE * 0.5f);
        }
        else
        {
            pos.y = -(int)Mathf.Floor(gridSize.y / 2) * ELEMENT_SIZE;
        }

        return pos;
    }

    //puts walls to the gridElementsArray
    private void SetUpWalls()
    {
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                if (Random.Range(0f, 1f) <= 0.25f)
                {
                    gridElementsArray[i, j].type = ElementType.Wall;
                }
            }
        }

        gridDrawerComponent.DrawWalls();
    }

    //sets an item to selected index in array
    public void SpawnAnItem(Vector2Int itemIndex, ElementType itemType)
    {
        GameObject newItem = gridDrawerComponent.DrawItem(itemIndex, itemType);
        gridElementsArray[itemIndex.y, itemIndex.x].type = itemType;
        gridElementsArray[itemIndex.y, itemIndex.x].heldElement = newItem;
    }
}
