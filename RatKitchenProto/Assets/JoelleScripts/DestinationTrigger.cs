using UnityEngine;
using System.Collections;

public class DestinationTrigger : MonoBehaviour
{
    private Transform spawnPoint;
    [SerializeField] private float fadeDuration = 1f;
    private SceneFader fader;

    private void Awake()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Destination").transform;
        fader = FindFirstObjectByType<SceneFader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeTeleport());
            other.transform.position = spawnPoint.position;
            DifficultyManager.Instance.LevelComplete();
        }
    }

    private IEnumerator FadeTeleport()
    {
        //if (fader == null || spawnPoint == null) yield break;

        // fade to black
        yield return StartCoroutine(fader.FadeOutRoutine(fadeDuration));

        // Move player back to SpawnPoint
        //player.position = spawnPoint.position;

        // this one is optional which resets the velocity if/when using Rigidbody
        //Rigidbody rigidBody = player.GetComponent<Rigidbody>();
        //if (rigidBody != null)
        //{
        //    rigidBody.linearVelocity = Vector3.zero;
        //    rigidBody.angularVelocity = Vector3.zero;
        //}

        // Fade back in
        yield return StartCoroutine(fader.FadeInRoutine(fadeDuration));
    }
}

