using UnityEngine;
using TMPro; // Use 'using UnityEngine.UI;' if using Legacy Text

public class MenuStats : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text totalTrashText;

    void Start()
    {
        DisplayStats();
    }

    public void DisplayStats()
    {
        // Pull the numbers from memory. 0 is the default if no data exists.
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        int totalTrash = PlayerPrefs.GetInt("TotalTrash", 0);

        // Update the text on screen
        if (highScoreText != null) 
            highScoreText.text = "Best Cleanup: " + highScore;

        if (totalTrashText != null) 
            totalTrashText.text = "Total Collected: " + totalTrash;
    }

    // Optional: A way to reset stats for testing
    [ContextMenu("Reset Stats")]
    public void ResetStats()
    {
        PlayerPrefs.DeleteAll();
        DisplayStats();
    }
}