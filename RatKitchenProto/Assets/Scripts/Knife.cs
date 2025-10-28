using System.Collections;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    private Quaternion startRotation;
    private Quaternion endRotation;

    private bool isComplete;


    void Start()
    {
        startRotation = transform.rotation;
        endRotation = new Quaternion(20, startRotation.y, startRotation.y, 21);

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
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, i);
            yield return null;
        }
        for (float i = 0; i < 1; i += Time.deltaTime * speed)
        {
            transform.rotation = Quaternion.Lerp(endRotation, startRotation, i);
            yield return null;
        }
        isComplete = true;
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
