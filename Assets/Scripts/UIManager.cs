using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;
    [SerializeField] private string highScoreKey = "HighScore";

    [Header("References")]
    public TextMeshProUGUI deathPanelTimerText;
    public string deathPanelTimerTextPath;
    
    [Tooltip("The Stopwatch script used in the level.")]
    public Stopwatch stopwatch;

    // Call this to show the final screen
    public void ShowDeathPanelWithFormattedTime(string formattedTime)
    {
        if (deathPanel == null) return;

        deathPanel.SetActive(true);

        // This is the key: If your Stopwatch script checks 'deathPanel.activeSelf',
        // the timer will now stop because we just set it to true.

        if (deathPanelTimerText == null)
        {
            if (!string.IsNullOrEmpty(deathPanelTimerTextPath))
            {
                var child = deathPanel.transform.Find(deathPanelTimerTextPath);
                if (child != null) deathPanelTimerText = child.GetComponent<TextMeshProUGUI>();
            }

            if (deathPanelTimerText == null)
                deathPanelTimerText = deathPanel.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (deathPanelTimerText != null)
        {
            deathPanelTimerText.text = formattedTime;
        }
       
        // This saves the raw seconds so the Menu script can format it
        PlayerPrefs.SetFloat("HighScore", stopwatch.GetCurrentTimeRaw()); 
        PlayerPrefs.Save();
    }
    private void SaveFinalTime(string currentFormattedTime)
{
    if (stopwatch != null)
    {
        float currentTime = stopwatch.GetCurrentTimeRaw(); 
        float bestTime = PlayerPrefs.GetFloat(highScoreKey, 0f);

        // If the new time is HIGHER (longer survival), save it
        if (currentTime > bestTime)
        {
            PlayerPrefs.SetFloat(highScoreKey, currentTime);
            PlayerPrefs.Save();
            Debug.Log("New Best Time Saved: " + currentTime);
        }
    }
}
}