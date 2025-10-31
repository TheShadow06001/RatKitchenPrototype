using System.Collections.Generic;
using UnityEngine;

public class KitchenPool : MonoBehaviour
{
    public static KitchenPool Instance;

    [SerializeField] private int poolSize = 10;
    private Dictionary<KitchenElement, Queue<GameObject>> poolDictionary = new();
    public KitchenElement[] kitchenElements;
    private List<KitchenElement> loadedKitchenElements = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        kitchenElements = Resources.LoadAll<KitchenElement>("KitchenElements");
        BuildPools(kitchenElements);
    }

    private void BuildPools(KitchenElement[] kitchenElementList)
    {
        poolDictionary.Clear();
        loadedKitchenElements.Clear();

        foreach (var element in kitchenElementList)
        {

            if (element == null || element.Prefab == null)
            {
                Debug.LogWarning("Poolmanager: missing KitchenElement or prefab for pool");
                continue;
            }

            if (poolDictionary.ContainsKey(element))
            {
                Debug.LogWarning($"Duplicate pool detected for {element.name}");
                continue;
            }

            loadedKitchenElements.Add(element);

            Queue<GameObject> objectPool = new();

            for (int i = 0; i < poolSize; i++)
            {

                GameObject obj = Instantiate(element.Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(element, objectPool);
        }
    }

    public List<KitchenElement> GetAllKitchenElements()
    {
        return loadedKitchenElements;
    }

    //double check
    public GameObject GetPooledObject(KitchenElement element, Vector3 position, Quaternion rotation)
    {
        if (element == null || !poolDictionary.ContainsKey(element))
            return null;

        var objectPool = poolDictionary[element];

        if (objectPool.Count == 0)
            return null;

        GameObject obj = objectPool.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    //double check
    public void ReturnToPool(KitchenElement element, GameObject obj)
    {
        if (element == null || !poolDictionary.ContainsKey(element))
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        poolDictionary[element].Enqueue(obj);
    }
}
