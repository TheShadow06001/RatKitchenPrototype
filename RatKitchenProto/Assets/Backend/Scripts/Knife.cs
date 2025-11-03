using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float minRate = 0;
    [SerializeField] private float maxRate = 2;

    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private MeshRenderer line;

    private BoxCollider collider;
    private Vector3 startPosition;
    private Vector3 endPosition;

    private bool isComplete;



    void Start()
    {
        collider = GetComponent<BoxCollider>();

        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z );

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
        //collider.enabled = true;
        //line.enabled = true;
        //mesh.enabled = true;

        SoundManager.Instance.PlaySoundEffect(SoundEffects.KnifeTrapWhoosh);

        //yield return new WaitForSeconds(0.7f);

        int value1 = 0;


        for (float i = 0; i < startPosition.y; i += Time.deltaTime * speed)
        {
            //transform.rotation = Quaternion.Slerp(startRotation, endRotation, i);
            transform.position = Vector3.Lerp(startPosition, endPosition, i);
            yield return null;

            if (transform.position == endPosition && value1 == 0)
            {
                SoundManager.Instance.PlaySoundEffect(SoundEffects.KnifeTrapChop);
                value1++;
            }
        }

        

        //line.enabled = false;

        //yield return new WaitForSeconds(1.5f);


        for (float i = 0; i < startPosition.y; i += Time.deltaTime * speed)
        {
            //transform.rotation = Quaternion.Euler(i, 0, 0);
            transform.position = Vector3.Lerp(endPosition, startPosition, i);
            yield return null;
        }

        //collider.enabled = false;
        //mesh.enabled = false;


        //float value = Random.Range(minRate, maxRate);

        //yield return new WaitForSeconds(value);
        isComplete = true;
    }
}
