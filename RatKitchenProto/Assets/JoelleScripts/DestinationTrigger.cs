using UnityEngine;

public class DestinationTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "SceneB";  
    [SerializeField] private float fadeDuration = 1f;          
    [SerializeField] private SceneFader fader;                 

    private void Awake()
    {
        
        if (fader == null)
            fader = FindFirstObjectByType<SceneFader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && fader != null)
        {
            fader.FadeAndLoad(nextSceneName, fadeDuration);
        }
    }
}



// This script should be attached to the hole sprite or destination object (exit)
