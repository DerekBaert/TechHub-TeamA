using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float startTime = 60f; // Initial time in seconds
    private float currentTime;
    public TextMeshProUGUI countdownText;

    // Static flag so other scripts can check if countdown is done
    public static bool isCountdownComplete = false;

    void Start()
    {
        currentTime = startTime;
        isCountdownComplete = false; // reset flag at start
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            DisplayTime(currentTime);
        }
        else
        {
            currentTime = 0;
            DisplayTime(currentTime);

            if (!isCountdownComplete)
            {
                isCountdownComplete = true;
                Debug.Log("Time's Up! Game starting...");
                HideCountdownText(); // Hide text after countdown
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void HideCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.enabled = false; // disable text display
            // Alternative: countdownText.text = ""; // or just clear the text
        }
    }
}
