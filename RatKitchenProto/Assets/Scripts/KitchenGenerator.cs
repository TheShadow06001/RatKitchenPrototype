using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class KitchenGenerator : MonoBehaviour
{
    [SerializeField] private List<KitchenElement> KitchenElements = new();

    [SerializeField] private GameObject counter;
    [SerializeField] private Transform generationPoint;

    [SerializeField] private float elementWidth;

    private void Start()
    {
        KitchenElements = KitchenPool.Instance.GetAllKitchenElements();
        

    }

    // 5 platforms active
    //return to pool when camera passed

    private void GenerateKitchen()
    {
        //for-loop to pick random elements from the KitchenElements-list
        for (int i = 0; i < KitchenElements.Count; ++i)
        {

        }
    }

}
