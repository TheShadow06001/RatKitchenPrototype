using UnityEngine;
using System.Collections;
public class DestinationTrigger : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private SceneFader fader;

    private void Awake()
    {
        if (fader == null)
            fader = FindFirstObjectByType<SceneFader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeTeleport(other.transform));
        }
    }

    private IEnumerator FadeTeleport(Transform player)
    {
        if (fader == null) yield break;

        // Fade to black
        yield return StartCoroutine(fader.FadeOutRoutine(fadeDuration));

        // Move player back to spawn
        player.position = spawnPoint.position;

        // Optional: reset velocity if using Rigidbody
        var rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Fade back in
        yield return StartCoroutine(fader.FadeInRoutine(fadeDuration));
    }
}



// This script should be attached to the hole sprite or destination object (exit)
