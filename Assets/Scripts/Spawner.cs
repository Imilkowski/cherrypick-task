using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void SpawnAnItem()
    {
        GridManager.ElementType randomItemType;

        float randomNum = Random.Range(0f, 1f);
        if (randomNum <= 0.33f)
        {
            randomItemType = GridManager.ElementType.ItemRed;
        }
        else if (randomNum <= 0.66f)
        {
            randomItemType = GridManager.ElementType.ItemGreen;
        }
        else
        {
            randomItemType = GridManager.ElementType.ItemBlue;
        }

        GridManager.Instance.SpawnAnItem(randomItemType, transform.position);
    }
}
