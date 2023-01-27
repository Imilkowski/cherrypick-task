using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSON_Loader : MonoBehaviour
{
    [SerializeField] private string _fileName;

    private class Size
    {
        public Vector2Int size;

        public Size(Vector2Int size)
        {
            this.size = size;
        }
    }

    void Awake()
    {
        Size gridSize = ReadGridData(Path.Combine(Application.streamingAssetsPath, _fileName));

        if (gridSize != null)
        {
            GridManager.Instance.InitializeGrid(gridSize.size);
        }
        else
        {
            Debug.Log("Grid data not found");
        }
    }

    //reads size data from json file
    private Size ReadGridData(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }
        else
        {
            StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();

            Size gridSize = JsonUtility.FromJson<Size>(json);
            reader.Close();

            return gridSize;
        }
    }
}
