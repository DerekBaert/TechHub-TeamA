using UnityEngine;
using TMPro;

public class Stopwatch : MonoBehaviour
    {
        private float elapsedTime = 0f;
        private float highestTime = 0f;
        [SerializeField] private TextMeshProUGUI timerDisplay;
        [Tooltip("Optional reference to the Death Panel GameObject. When active, the stopwatch will pause so the final time is shown.")]
        public GameObject deathPanel;    void Start()
    {
        elapsedTime = 0f;

        // try to find a DeathPanel if one wasn't assigned in the Inspector
        if (deathPanel == null)
        {
            var found = GameObject.Find("DeathPanel");
            if (found != null) deathPanel = found;
            else
            {
                var d = FindObjectOfType<DeathPanelBGM>();
                if (d != null && d.deathPanel != null) deathPanel = d.deathPanel;
            }
        }
    }

    void Update()
    {
        // Only start counting after countdown is complete
        if (!CountdownTimer.isCountdownComplete)
            return;

        // If the death panel is active, stop increasing elapsed time so the final
        // survived time remains on-screen.
        if (deathPanel != null && deathPanel.activeSelf)
            return;

        elapsedTime += Time.deltaTime;

        // track the highest time reached
        if (elapsedTime > highestTime)
        {
            highestTime = elapsedTime;
        }

        if (timerDisplay != null)
        {
            timerDisplay.text = $"Time: {elapsedTime:F2}s";
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

    // Returns a formatted string in minute:seconds (with 2 decimal places for seconds, no suffix).
    // Example: "1:23.45"
    public string GetFormattedElapsedTime(string prefix = "")
    {
        int minutes = (int)elapsedTime / 60;
        float seconds = elapsedTime % 60f;
        string secondsText = seconds.ToString("00.00");
        return $"{prefix}{minutes}:{secondsText}";
    }

    // Returns a formatted string for the highest time reached in minute:seconds format (no suffix).
    // Example: "1:23.45"
    public string GetFormattedHighestTime(string prefix = "")
    {
        int minutes = (int)highestTime / 60;
        float seconds = highestTime % 60f;
        string secondsText = seconds.ToString("00.00");
        return $"{prefix}{minutes}:{secondsText}";
    }

    public void Reset()
    {
        elapsedTime = 0f;
        // Note: highestTime is NOT reset, so it persists across level restarts
    }
}
