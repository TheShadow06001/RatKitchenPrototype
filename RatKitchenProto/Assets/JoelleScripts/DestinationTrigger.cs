using UnityEngine;
using System.Collections;

public class DestinationTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPoint;
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
        if (fader == null || spawnPoint == null) yield break;

        // fade to black
        yield return StartCoroutine(fader.FadeOutRoutine(fadeDuration));

        // Move player back to SpawnPoint
        player.position = spawnPoint;

        // this one is optional which resets the velocity if/when using Rigidbody
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

