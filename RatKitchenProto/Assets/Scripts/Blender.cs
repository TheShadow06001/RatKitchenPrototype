using System.Collections;
using UnityEngine;

public class Blender : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float range;
    [SerializeField] private float fallPercentage;
    [SerializeField] private float fallSpeed;

    private bool hasFallen = false;
    private float endRotation = 90;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        SoundManager.Instance.PlaySoundEffect(SoundEffects.BlenderBlending);
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckRange()) FallOver();
    }

    private void FallOver()
    {
        float value = Random.Range(0, 100);
        if (value >= (100 - fallPercentage) && !hasFallen)
        {
            StartCoroutine(Fall());
        }
    }

    private bool CheckRange()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= range) return true;
        else return false;
    }

    IEnumerator Fall()
    {
        for (float i = 0; i < endRotation; i += Time.deltaTime * fallSpeed)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.z, transform.rotation.y, -i);
            yield return null;
        }
        hasFallen = true;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.right * range, Color.red);
    }

}
