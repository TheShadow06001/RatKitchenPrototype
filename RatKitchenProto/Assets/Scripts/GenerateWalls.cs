using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateWalls : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject windowedWallPrefab;
    public int maxWindows = 1;
    public int amountOfElements = 5;

    private void Awake()
    {
        Vector3 newPositionWindowed = windowedWallPrefab.transform.position;
        newPositionWindowed = new Vector3(0.008f, 0, 0);
        windowedWallPrefab.transform.position = newPositionWindowed; // how to assign new position to game object

        GenerateRandomWalls();
    }
    private void GenerateRandomWalls()
    {
        for (int i = 1; i <= amountOfElements; i++)
        {
            Vector3 spawnPosition = new Vector3(i * -4f, 0, -0.93f);
            
            int randomType = Random.Range(0, 2);
            GameObject prefabToSpawn = null;
            
            switch (randomType)
            {
                case 0:
                    if (maxWindows > 0)
                    {
                        prefabToSpawn = windowedWallPrefab;
                        maxWindows--;
                        
                    }
                    else
                    {
                        prefabToSpawn = wallPrefab;
                    }
                    break;
                case 1:
                    prefabToSpawn = wallPrefab;
                    break;
                
            }
            
            if (prefabToSpawn != null)
            {
                GameObject clone = Instantiate(prefabToSpawn, spawnPosition, Quaternion.Euler(0, 90, 0));
                clone.transform.SetParent(transform);
            }
        }
    }

}
