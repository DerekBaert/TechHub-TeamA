using UnityEngine;
using TMPro;

public class Stopwatch : MonoBehaviour
    {
        private float elapsedTime = 0f;
        private float highestTime = 0f;
        [SerializeField] private TextMeshProUGUI timerDisplay;
        [Tooltip("Optional reference to the Death Panel GameObject. When active, the stopwatch will pause so the final time is shown.")]
        public GameObject deathPanel;    
        void Start()
{
    elapsedTime = 0f;
    highestTime = PlayerPrefs.GetFloat("HighScore", 0f);
    
    // Update the HUD immediately
    if (timerDisplay != null)
        timerDisplay.text = $"Time: 0.0s";

    Debug.Log($"Session Started. Personal Best to beat: {highestTime:F1}s");
}

void Update()
{
    // If time is 0, exit the function immediately
    if (Time.timeScale == 0) return;
    elapsedTime += Time.deltaTime;
    // We still update the variable so the UI knows it's a new record, 
    // but we STOP saving to PlayerPrefs here.
    if (elapsedTime > highestTime)
    {
        highestTime = elapsedTime;
    }

    if (timerDisplay != null)
    {
        timerDisplay.text = $"Time: {elapsedTime:F1}s"; 
    }
}

public float GetCurrentTimeRaw()
{
    // This should return the float variable you use to track time.
    // If your variable is named 'currentTime' or 'elapsedTime', use that name here!
    return elapsedTime; 
}

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public float GetHighestTime()
    {
        return highestTime;
    }

    // Writes the current (final) elapsed time into the provided TextMeshProUGUI.
    // If the stopwatch is still running this writes the current value; call this
    // when the Death Panel becomes active to display the time survived.
    public void WriteFinalTimeTo(TMPro.TextMeshProUGUI textField)
    {
        if (textField == null) return;

        textField.text = GetFormattedElapsedTime("Time Survived: ");
    }

    // CHANGE: Replace your formatting method to use seconds only
public string GetFormattedElapsedTime(string prefix = "")
{
    return $"{prefix}{elapsedTime:F1}s";
}

public string GetFormattedHighestTime(string prefix = "")
{
    return $"{prefix}{highestTime:F1}s";
}

    public void Reset()
    {
        elapsedTime = 0f;
        // Note: highestTime is NOT reset, so it persists across level restarts
    }

   public void SaveToTotalTime()
{
    // 1. Save Total Play Time
    float totalSoFar = PlayerPrefs.GetFloat("TotalPlayTime", 0f);
    PlayerPrefs.SetFloat("TotalPlayTime", totalSoFar + elapsedTime);
    
    // 2. Save High Score (Only happens once here!)
    float savedHigh = PlayerPrefs.GetFloat("HighScore", 0f);
    if (highestTime > savedHigh)
    {
        PlayerPrefs.SetFloat("HighScore", highestTime);
    }

    // Write everything to disk at once
    PlayerPrefs.Save();
}

}
