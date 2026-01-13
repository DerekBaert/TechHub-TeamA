using UnityEngine;
using TMPro;

public class MenuStats : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI statsDisplayTrigger;

    [Header("Settings")]
    public string highScoreKey = "HighScore";
    public string totalTrashKey = "TotalTrash";
    
    [Header("Visual Styling")]
    [SerializeField] private Color highlightColor = Color.yellow;

    void Start()
    {
        // Make sure this name matches the function below!
        DisplayFormattedStats(); 
    }

    public void DisplayFormattedStats()
    {
        if (statsDisplayTrigger == null) return;

        float bestTime = PlayerPrefs.GetFloat(highScoreKey, 0f);
        int totalTrash = PlayerPrefs.GetInt(totalTrashKey, 0);
        int highestWave = PlayerPrefs.GetInt("HighestWave", 1);

        string minutes = ((int)bestTime / 60).ToString("00");
        string seconds = (bestTime % 60).ToString("00");
        string formattedBestTime = $"{minutes}:{seconds}";

        string hexColor = ColorUtility.ToHtmlStringRGB(highlightColor);

        string finalReport = "--- <color=#" + hexColor + ">OCEAN RECORDS</color> ---\n\n" +
                             "BEST SURVIVAL: <color=#" + hexColor + ">" + formattedBestTime + "</color>\n" +
                             "HIGHEST WAVE: <color=#" + hexColor + ">" + highestWave + "</color>\n" +
                             "TOTAL TRASH: <color=#" + hexColor + ">" + totalTrash + "</color>\n" +
                             "---------------------";

        statsDisplayTrigger.text = finalReport;
    }
}