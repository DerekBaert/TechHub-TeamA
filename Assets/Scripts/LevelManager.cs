using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Game State Objects")]
    [Tooltip("The parent object of the Crab Trap to be disabled on death.")]
    [SerializeField] private GameObject crabTrapParent;

    [Header("UI Elements to Hide")]
    [Tooltip("Drag the HUD Timer object here so it can be hidden on death.")]
    [SerializeField] private GameObject hudTimerObject;

    private void Awake()
    {
        if (LevelManager.instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void GameOver()
{
    // 1. Hide the HUD Timer so it's not visible behind the death panel
    if (hudTimerObject != null)
    {
        hudTimerObject.SetActive(false);
    }

    // 2. Disable the Crab Trap
    if (crabTrapParent != null) 
    {
        crabTrapParent.SetActive(false);
    }

    if (crabTrapParent != null) crabTrapParent.SetActive(false);

    UIManager _ui = GetComponent<UIManager>();
    if (_ui != null)
    {
        Stopwatch sw = FindObjectOfType<Stopwatch>();

        if (sw != null)
        {
            // 1. Save the time to the lifetime total
            sw.SaveToTotalTime();

            float current = sw.GetElapsedTime();
            float best = PlayerPrefs.GetFloat("HighScore", 0f);
            float lifetime = PlayerPrefs.GetFloat("TotalPlayTime", 0f);

            string recordMessage = (current >= best) ? "<color=yellow>NEW BEST!</color>\n" : "";

            // 2. Format the string to include Lifetime stats
            // We convert lifetime to minutes if it's getting long
            string lifetimeStr = (lifetime > 60) ? $"{(lifetime / 60):F1}m" : $"{lifetime:F0}s";

            string formatted = $"{recordMessage}" +
                               $"Current: {current:F1}s\n" +
                               $"Personal Best: {best:F1}s\n" +
                               $"<size=80%>Total Time Played: {lifetimeStr}</size>";

            _ui.ShowDeathPanelWithFormattedTime(formatted);
        }
    }
}

}