using UnityEngine;

public class StatsManager : MonoBehaviour
{
    // Static instance so other scripts can find this easily
    public static StatsManager instance;

    void Awake()
    {
        // Singleton pattern to ensure only one manager exists
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Call this function when the player loses or the game ends.
    /// </summary>
    /// <param name="finalScore">The score the player achieved this round.</param>
    public void SaveGameResults(int finalScore)
    {
        // 1. Handle High Score
        int lastHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (finalScore > lastHighScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            Debug.Log("New High Score Saved: " + finalScore);
        }

        // 2. Handle Total Trash (Cumulative)
        int currentTotal = PlayerPrefs.GetInt("TotalTrash", 0);
        PlayerPrefs.SetInt("TotalTrash", currentTotal + finalScore);

        // 3. Save to disk
        PlayerPrefs.Save();
    }

    // You can call this from your Secret Level script too!
    public void MarkSecretFound()
    {
        PlayerPrefs.SetInt("SecretFound", 1);
        PlayerPrefs.Save();
    }
}