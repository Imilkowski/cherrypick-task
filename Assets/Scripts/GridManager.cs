using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int ELEMENT_SIZE = 32;

    public Vector2Int gridSize { get; private set; }
    public Vector3 startPos { get; private set; }
    public Vector2Int spawnerIndexPos;

    private Ring ring = new Ring();

    public enum ElementType
    {
        Empty,
        Wall,
        Spawner,
        ItemGreen,
        ItemRed,
        ItemBlue
    }

    private class Ring
    {
        public int level;
        public Vector2Int posFromCenter;

        public void Reset()
        {
            level = 1;
            posFromCenter = new Vector2Int(0, -1);
        }

        public void RaiseLevel()
        {
            level += 1;
            posFromCenter = new Vector2Int(0, -level);
        }

        public void NextPos()
        {
            if(posFromCenter.y == -level && posFromCenter.x == -1)
            {
                RaiseLevel();
                return;
            }

            if (posFromCenter.y == -level && posFromCenter.x != level)
            {
                posFromCenter += new Vector2Int(1, 0);
                return;
            }

            if (posFromCenter.x == level && posFromCenter.y != level)
            {
                posFromCenter += new Vector2Int(0, 1);
                return;
            }

            if (posFromCenter.y == level && posFromCenter.x != -level)
            {
                posFromCenter += new Vector2Int(-1, 0);
                return;
            }

            if (posFromCenter.x == -level && posFromCenter.y != -level)
            {
                posFromCenter += new Vector2Int(0, -1);
                return;
            }
        }
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

        public void Clear()
        {
            //TODO: could use object pooling
            type = ElementType.Empty;
            Destroy(heldElement);
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
            pos.x = -((gridSize.x / 2) * ELEMENT_SIZE - (int)(ELEMENT_SIZE * 0.5f));
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
            pos.y = (int)Mathf.Floor(gridSize.y / 2) * ELEMENT_SIZE;
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

    //deletes previous spawner position in array and adds in new place
    public void MoveSpawnerIndex(Vector2Int newIndex)
    {
        gridElementsArray[spawnerIndexPos.y, spawnerIndexPos.x].type = ElementType.Empty;
        gridElementsArray[newIndex.y, newIndex.x].type = ElementType.Spawner;
        spawnerIndexPos = newIndex;

        ring.Reset();
    }

    //converts world space to index in array
    private Vector2Int GetIndexPos(Vector3 worldPos)
    {
        Vector3 relativePos = worldPos - GridManager.Instance.startPos;
        Vector3 fixedPos = new Vector3(Mathf.Round(relativePos.x / 32) * 32, Mathf.Round(relativePos.y / 32) * 32, 0);

        return new Vector2Int((int)(fixedPos.x / 32), -(int)(fixedPos.y / 32));
    }

    //returns value of the element on given index position
    private ElementType ReadElement(Vector2Int indexPos)
    {
        return gridElementsArray[indexPos.y, indexPos.x].type;
    }

    //checks if the index position is inbounds
    private bool IsInbounds(Vector2Int indexPos)
    {
        if(indexPos.y >= 0 && indexPos.y < gridSize.y && indexPos.x >= 0 && indexPos.x < gridSize.x)
        {
            return true;
        }

        return false;
    }

    //check where is the nearest empty place
    private Vector2Int GetNearestEmpty(Vector2Int centerLocation)
    {
        if(ring.level > gridSize.x && ring.level > gridSize.y)
        {
            return new Vector2Int(-1, -1);
        }

        Vector2Int locationToCheck = centerLocation + ring.posFromCenter;

        if (IsInbounds(locationToCheck))
        {
            if (ReadElement(locationToCheck) == ElementType.Empty)
            {
                return locationToCheck;
            }
            else
            {
                //check next one
                ring.NextPos();
                return GetNearestEmpty(centerLocation);
            }
        }
        else
        {
            //check next one
            ring.NextPos();
            return GetNearestEmpty(centerLocation);
        }
    }

    //sets an item to selected index in array
    public void SpawnAnItem(ElementType itemType, Vector3 spawnerPos)
    {
        try
        {
            Vector2Int location = GetNearestEmpty(GetIndexPos(spawnerPos));
            if (location != new Vector2Int(-1, -1)) // if there is any empty space left
            {
                SpawnAnItemAtPos(location, itemType, spawnerPos);
            }
            else
            {
                Debug.Log("No more empty space");
            }
        }
        catch
        {
            Debug.Log("No more empty space");
        }
    }

    //sets an item to selected index in array
    public void SpawnAnItemAtPos(Vector2Int itemIndex, ElementType itemType, Vector3 spawnerPos)
    {
        GameObject newItem = gridDrawerComponent.DrawItem(itemIndex, itemType, spawnerPos);
        gridElementsArray[itemIndex.y, itemIndex.x].type = itemType;
        gridElementsArray[itemIndex.y, itemIndex.x].heldElement = newItem;
    }

    private void ClearItem(Vector2Int itemIndex)
    {
        gridElementsArray[itemIndex.y, itemIndex.x].Clear();
    }

    //checks an item and clear if needed
    private bool CheckNeighbour(Vector2Int itemIndex, ElementType type)
    {
        if (IsInbounds(itemIndex))
        {
            if(ReadElement(itemIndex) == type)
            {
                ClearItem(itemIndex);
                return true;
            }
        }

        return false;
    }

    //checks adjacent neighbours to the given item and clears if they're the same type
    private bool CheckAndClearNeighbours(Vector2Int itemIndex, ElementType type)
    {
        bool hasNeighbours = false;

        //up
        Vector2Int upElementIndex = itemIndex - Vector2Int.up;
        if(CheckNeighbour(upElementIndex, type))
        {
            hasNeighbours = true;
        }

        //right
        Vector2Int rightElementIndex = itemIndex + Vector2Int.right;
        if (CheckNeighbour(rightElementIndex, type))
        {
            hasNeighbours = true;
        }

        //bottom
        Vector2Int bottomElementIndex = itemIndex - Vector2Int.down;
        if (CheckNeighbour(bottomElementIndex, type))
        {
            hasNeighbours = true;
        }

        //left
        Vector2Int leftElementIndex = itemIndex + Vector2Int.left;
        if (CheckNeighbour(leftElementIndex, type))
        {
            hasNeighbours = true;
        }

        return hasNeighbours;
    }

    //clears all the items that has neighbours
    public void ClearNeighbours()
    {
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                ElementType elementType = ReadElement(new Vector2Int(j, i));

                if(elementType == ElementType.ItemRed || elementType == ElementType.ItemGreen || elementType == ElementType.ItemBlue)
                {
                    bool hasNeighbours = CheckAndClearNeighbours(new Vector2Int(j, i), elementType);

                    if (hasNeighbours)
                    {
                        ClearItem(new Vector2Int(j, i));
                    }
                }
            }
        }

        ring.Reset();
    }
}
