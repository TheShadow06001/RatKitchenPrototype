using System.Collections.Generic;
using UnityEngine;

public class KitchenGenerator : MonoBehaviour
{
    [SerializeField] private List<KitchenElement> KitchenElements = new();
    private int activePieces = 5;
    private List<KitchenElement> renderedPieces = new List<KitchenElement>();
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private void Start()
    {
        KitchenElements = KitchenPool.Instance.GetAllKitchenElements();
        Debug.Log(KitchenElements.Count);
        GenerateKitchen();
    }

    // 5 platforms active
    //return to pool when camera passed

    private void GenerateKitchen()
    {
        //for-loop to pick random elements from the KitchenElements-list
        for (int i = 0; i < activePieces; ++i)
        {
            renderedPieces[i] = KitchenElements[Random.Range(0, KitchenElements.Count)];
            KitchenPool.Instance.GetPooledObject(renderedPieces[i], spawnPosition, spawnRotation);
        }
    }

}
