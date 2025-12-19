using UnityEngine;

public class ScoreResetter : MonoBehaviour
{
    public void ResetHighScore()
    {
        // Deletes the specific "HighScore" key from the computer's memory
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("TotalPlayTime");
        PlayerPrefs.Save();
        Debug.Log("All Stats Cleared!");
    }
}