using System.Collections;
using UnityEngine;

public class Blender : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float range;
    [SerializeField] private float fallPercentage;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float checkCooldown;

    private bool canFall = true;
    private bool hasFallen = false;
    private float endRotation = -90;
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
        if (value >= (100 - fallPercentage) && canFall)
        {
            if (!hasFallen) 
            {
                StartCoroutine(Fall());
            }
            
        }
        else
        {
            canFall = false;
            StartCoroutine(Timer());
        }
    }

    private bool CheckRange()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= range) return true;
        else return false;
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(checkCooldown);
        canFall = true;

    }

    IEnumerator Fall()
    {
        

        for (float i = 0; i > endRotation; i -= Time.deltaTime * fallSpeed)
        {

            if (transform.rotation.z > endRotation)
            {
                transform.Rotate(transform.forward, i);
            }

            

            //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, -i, 0);
            yield return null;
        }

        

        hasFallen = true;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector2.right * range, Color.red);
    }

}
