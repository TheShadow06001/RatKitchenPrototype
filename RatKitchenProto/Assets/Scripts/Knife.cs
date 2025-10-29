using System.Collections;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float minRate = 0;
    [SerializeField] private float maxRate = 2;

    [SerializeField] private MeshRenderer mesh;

    private float endRotation;

    private bool isComplete;


    void Start()
    {
        
        endRotation = 88f;

        isComplete = false;
        StartCoroutine(Chop());
        
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
        mesh.enabled = true;

        yield return new WaitForSeconds(0.3f);

        for (float i = 0; i < endRotation; i += Time.deltaTime * speed)
        {
            //transform.rotation = Quaternion.Slerp(startRotation, endRotation, i);
            transform.rotation = Quaternion.Euler(i, 0, 0);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        for (float i = 0; i < endRotation; i += Time.deltaTime * speed/15)
        {
            transform.rotation = Quaternion.Euler(endRotation - i, 0, 0);
            yield return null;
        }

        mesh.enabled = false;

        float value = Random.Range(minRate, maxRate);

        yield return new WaitForSeconds(value);
        isComplete = true;
    }

    

   

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
