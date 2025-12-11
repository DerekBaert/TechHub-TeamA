using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;

    [Header("References for Death Panel")]
    [Tooltip("Text element on the Death Panel where the final time will be shown. If left empty, the script can use a path to locate it.")]
    public TextMeshProUGUI deathPanelTimerText;

    [Tooltip("If you want the UI manager to locate the timer text by a child path under the death panel, set the path here (e.g. 'Panel/TimeText').")]
    public string deathPanelTimerTextPath;

    [Tooltip("Optional reference to the Stopwatch. If left empty it will be found at runtime.")]
    public Stopwatch stopwatch;

    // Toggles the death panel on/off. This no longer writes the final time; that is
    // handled by LevelManager.GameOver() which will call ShowDeathPanelWithFormattedTime().
    public void ToggleDeathPanel()
    {
        if (deathPanel == null) return;
        deathPanel.SetActive(!deathPanel.activeSelf);
    }

    // Show the death panel and populate the timer text using a formatted time string.
    // LevelManager.GameOver() should call this, passing in the formatted string from the Stopwatch.
    public void ShowDeathPanelWithFormattedTime(string formattedTime)
    {
        if (deathPanel == null) return;

        deathPanel.SetActive(true);

        // locate the timer text field by explicit reference first
        if (deathPanelTimerText == null)
        {
            // try explicit path under the death panel (if provided)
            if (!string.IsNullOrEmpty(deathPanelTimerTextPath))
            {
                var child = deathPanel.transform.Find(deathPanelTimerTextPath);
                if (child != null)
                {
                    deathPanelTimerText = child.GetComponent<TextMeshProUGUI>();
                }
            }

            // fallback: try to find first TextMeshProUGUI in children
            if (deathPanelTimerText == null)
            {
                deathPanelTimerText = deathPanel.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        if (deathPanelTimerText != null)
        {
            deathPanelTimerText.text = formattedTime;
        }
    }
}
