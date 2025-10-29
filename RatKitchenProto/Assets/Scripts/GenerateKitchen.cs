using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateKitchen : MonoBehaviour
{
    public int piecesByLevel = 10;

    public GameObject[] counterElements;
    public GameObject[] sinkElements;
    public GameObject[] stoveElements;

    public int counterNumber = 2;
    public int sinkNumber = 2;
    public int stoveNumber = 2;

  
   

    private void Awake()
    {
        GeneratePieces();
    }

    private void GeneratePieces()
    {
        int targetPieces = piecesByLevel;
        float xOffset = 0; // 85
        
        for (int i = 0; i < counterNumber; i++)
        {
            xOffset += SpawnWithOffset(counterElements, xOffset);
        }
        for (int i = 0; i < sinkNumber; i++)
        {
            xOffset += SpawnWithOffset(sinkElements, xOffset);
        }
        for (int i = 0; i < counterNumber; i++)
        {
            xOffset += SpawnWithOffset(counterElements, xOffset);
        }
        for (int i = 0; i < stoveNumber; i++)
        {
            xOffset += SpawnWithOffset(stoveElements, xOffset);
        }
        for (int i = 0; i < counterNumber; i++)
        {
            xOffset += SpawnWithOffset(counterElements, xOffset);
        }


    }

    float SpawnWithOffset(GameObject[] pieceArray, float inputOffset)
    {
        Transform randomTransform = pieceArray[Random.Range(0, pieceArray.Length)].transform;
        GameObject clone = Instantiate(randomTransform.gameObject, this.transform.position 
            + new Vector3 (inputOffset, 0, 0), transform.rotation) as GameObject;
        Mesh cloneMesh = clone.GetComponentInChildren<MeshFilter>().mesh;
        Bounds bounds = cloneMesh.bounds;
        float xOffset = bounds.size.x;

        clone.transform.SetParent(this.transform);

        return xOffset;
    }
}
