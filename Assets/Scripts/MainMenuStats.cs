using UnityEngine;
using TMPro;

public class MainMenuStats : MonoBehaviour
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
        DisplayFormattedStats();
    }

    public void DisplayFormattedStats()
    {
        if (statsDisplayTrigger == null) return;

        // 1. Retrieve the data
        float bestTime = PlayerPrefs.GetFloat(highScoreKey, 0f);
        int totalTrash = PlayerPrefs.GetInt(totalTrashKey, 0);

        // 2. Format the time
        string minutes = ((int)bestTime / 60).ToString("00");
        string seconds = (bestTime % 60).ToString("00");
        string formattedBestTime = $"{minutes}:{seconds}";

        // 3. Convert the highlight color to a Hex String (Unity Rich Text uses Hex)
        string hexColor = ColorUtility.ToHtmlStringRGB(highlightColor);

        // 4. Construct the string with Rich Text Tags
        // <b> makes it bold, <color=#hex> changes color
        string finalReport = "--- <color=#" + hexColor + ">OCEAN RECORDS</color> ---\n\n" +
                             "BEST SURVIVAL: <color=#" + hexColor + ">" + formattedBestTime + "</color>\n" +
                             "TOTAL TRASH: <color=#" + hexColor + ">" + totalTrash + "</color>\n" +
                             "---------------------";

        statsDisplayTrigger.text = finalReport;
    }
}