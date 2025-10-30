using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

public class KitchenPool : MonoBehaviour
{
    public static KitchenPool Instance;

    [SerializeField] private int poolSize = 10;
    private Dictionary<PlatformType, Queue<GameObject>> poolDictionary = new();
    private List<PlatformType> loadedKitchenPlatforms = new();
    
    public PlatformType[] kitchenPlatforms;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        kitchenPlatforms = Resources.LoadAll<PlatformType>("KitchenPlatforms");
        BuildPools(kitchenPlatforms);    
    }

    private void BuildPools(PlatformType[] kitchenPlatformList)
    {
        poolDictionary.Clear();
        loadedKitchenPlatforms.Clear();

        foreach (var type in kitchenPlatformList)
        {

            if (type == null || type.prefab == null)
            {
                Debug.LogWarning("Poolmanager: missing PlatformType or prefab for pool");
                continue;
            }

            if (poolDictionary.ContainsKey(type))
            {
                Debug.LogWarning($"Duplicate pool detected for {type.name}");
                continue;
            }

            loadedKitchenPlatforms.Add(type);

            Queue<GameObject> objectPool = new();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(type.GetRandomPrefab());
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(type, objectPool);
        }
    }

    public List<PlatformType> GetAllPlatformTypes()
    {
        return loadedKitchenPlatforms;
    }

    //double check
    public GameObject GetPooledObject(PlatformType type, Vector3 position, Quaternion rotation)
    {
        if (type == null || !poolDictionary.ContainsKey(type))
            return null;

        Queue<GameObject> objectPool = poolDictionary[type];
        GameObject obj; 

        if (objectPool.Count > 0)
        {
            obj = objectPool.Dequeue();
        }
        else
        {
            obj = Instantiate(type.GetRandomPrefab()); // utökar poolen om det inte finns tillräckligt att hämta
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    //double check
    public void ReturnToPool(PlatformType type, GameObject obj)
    {
        if (type == null || !poolDictionary.ContainsKey(type))
        {
            Destroy(obj); // utifall att pool saknas för objektet
            return;
        }

        obj.SetActive(false);
        poolDictionary[type].Enqueue(obj);
    }
}
