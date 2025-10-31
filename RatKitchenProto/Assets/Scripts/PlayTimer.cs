using TMPro;
using UnityEngine;

public class PlayTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    float _elapsedTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt((_elapsedTime * 1000f) % 1000f);
        _timeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
