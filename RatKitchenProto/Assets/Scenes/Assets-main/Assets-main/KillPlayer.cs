using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    

    private bool canKill = true;

    // Start is called before the first frame update
    private void Start()
    {
        

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canKill) return;

        if (other.gameObject.CompareTag("Player"))
        {

            HealthDisplay.instance.TakeDamage();
        }
    }

    
}
