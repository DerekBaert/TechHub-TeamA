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
    
    // CHANGE: Load the saved high score from the computer's memory
    highestTime = PlayerPrefs.GetFloat("HighScore", 0f);

    // ... (rest of your existing Start logic)
}

void Update()
{
    // ... (rest of your existing Update logic)

    elapsedTime += Time.deltaTime;

    if (elapsedTime > highestTime)
    {
        highestTime = elapsedTime;
        // CHANGE: Save the new record immediately to the disk
        PlayerPrefs.SetFloat("HighScore", highestTime);
    }

    if (timerDisplay != null)
    {
        // CHANGE: Simplified to seconds only
        timerDisplay.text = $"Time: {elapsedTime:F1}s"; 
    }
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
    // Get what is already saved, or 0 if it's the first time
    float totalSoFar = PlayerPrefs.GetFloat("TotalPlayTime", 0f);
    
    // Add the current run's time to the total
    float newTotal = totalSoFar + elapsedTime;
    
    // Save it back to the disk
    PlayerPrefs.SetFloat("TotalPlayTime", newTotal);
    PlayerPrefs.Save();
}

}
