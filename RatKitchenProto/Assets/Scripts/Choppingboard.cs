using UnityEngine;

public class Choppingboard : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private int maxScale = 8;
    private int spawnedKnifes;

    [Header("References")]
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private Transform referencePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int value = Random.Range(2, maxScale);
        transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
        transform.localPosition = new Vector3(value/2, 0, 0);

        

        for (int i = 0; i < transform.localScale.x/2; i++) 
        {
            spawnedKnifes++;
            Vector3 targetPosition = new Vector3((transform.localScale.x/spawnedKnifes) - i, referencePosition.position.y, referencePosition.position.z);
            Instantiate(knifePrefab, targetPosition, Quaternion.identity, parentTransform);
            
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
