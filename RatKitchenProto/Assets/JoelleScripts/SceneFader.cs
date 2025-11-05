using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image image;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    public void FadeAndLoad(string sceneName, float duration)
    {
        StartCoroutine(Fader(sceneName, duration));
    }

    IEnumerator Fader(string sceneName, float duration)
    {
        float t = 0;
        Color c = image.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = t / duration;
            image.color = c;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        Color c = image.color;
        while (t < 1)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / 1f);
            image.color = c;
            yield return null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FadeAndLoad("SceneA", 1);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            FadeAndLoad("SceneB", 1);
        }
    }

    public IEnumerator FadeOutRoutine(float duration)
    {
        float t = 0;
        Color c = image.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = t / duration;
            image.color = c;
            yield return null;
        }
    }

    public IEnumerator FadeInRoutine(float duration)
    {
        float t = 0;
        Color c = image.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / duration);
            image.color = c;
            yield return null;
        }
    }
}
