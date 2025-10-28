using System.Collections;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private Vector3 startPosition;
    private Vector3 endPosition;

    private bool isComplete;


    void Start()
    {
        startPosition = transform.position;
        
        endPosition = new Vector3(startPosition.x, -0.65f, startPosition.z);

        StartCoroutine(Chop());
        isComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
            transform.position = Vector3.Lerp(startPosition, endPosition, i);
            yield return null;
        }
        for (float i = 0; i < 1; i += Time.deltaTime * speed)
        {
            transform.position = Vector3.Lerp(endPosition, startPosition, i);
            yield return null;
        }
        isComplete = true;
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
