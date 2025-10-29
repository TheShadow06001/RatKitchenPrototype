using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class KitchenGenerator : MonoBehaviour
{
    [SerializeField] private List<KitchenElement> KitchenElements = new();
    private int slotCount = 10;
    private int slotSpacing = 0; 

    private void Start()
    {
        KitchenElements = KitchenPool.Instance.GetAllKitchenElements();

    }


    // first and last element always a counter
    // then:
    // Pick element from remaining pool to spawn in
    // Pick possible variant of prefab (empty sink, filled sink)
    // instantiate/set active at the correct position

    // 5 platforms active
    // new platforms are set active at trigger point?
    // return to pool when camera passed

    //private void GenerateKitchen()
    //{
    //    //for-loop to pick random elements from the KitchenElements-list
    //    for (int i = 0; i < KitchenElements.Count; ++i)
    //    {

    //    }
    //}

}
