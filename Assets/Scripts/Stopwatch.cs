using UnityEngine;
using TMPro;

public class Stopwatch : MonoBehaviour
{
    private float elapsedTime = 0f;
    [SerializeField] private TextMeshProUGUI timerDisplay;

    void Start()
    {
        elapsedTime = 0f;
    }

    void Update()
    {
        // Only start counting after countdown is complete
        if (!CountdownTimer.isCountdownComplete)
            return;

        elapsedTime += Time.deltaTime;

        if (timerDisplay != null)
        {
            timerDisplay.text = $"Time: {elapsedTime:F2}s";
        }
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void Reset()
    {
        elapsedTime = 0f;
    }
}
