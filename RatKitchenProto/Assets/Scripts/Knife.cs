using System.Collections;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private Transform startPosition;
    private Transform endPosition;

    private bool isComplete;


    void Start()
    {
        startPosition = gameObject.transform;
        endPosition = gameObject.transform;
        endPosition.position = new Vector3(endPosition.position.x, -9.5f, endPosition.position.z);

        Debug.Log(startPosition);
        Debug.Log(endPosition);

        StartCoroutine(Chop());
        isComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(startPosition.position);
        Debug.Log(endPosition.position);
        if (isComplete) 
        {
            isComplete = false;
            StartCoroutine(Chop());
        }
    }

    IEnumerator Chop()
    {
        for (float i = 0; i < 1; i += Time.deltaTime * speed)
        {
            transform.position = Vector3.Lerp(startPosition.position, endPosition.position, i);
            yield return null;
        }
        for (float i = 0; i < 1; i += Time.deltaTime * speed)
        {
            transform.position = Vector3.Lerp(endPosition.position, startPosition.position, i);
            yield return null;
        }
        isComplete = true;
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
