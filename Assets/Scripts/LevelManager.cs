using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public bool isGameOver = false;

    void Awake()   
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    [Header("Game State Objects")]
    [Tooltip("The parent object of the Crab Trap to be disabled on death.")]
    [SerializeField] private GameObject crabTrapParent;

    [Header("UI Elements to Hide")]
    [Tooltip("Drag the HUD Timer object here so it can be hidden on death.")]
    [SerializeField] private GameObject hudTimerObject;

    [Header("Combo System")]
    public int currentCombo = 0;
    public float comboTimer = 0f;
    public float comboExpiryTime = 1.5f;

    [Header("Difficulty Notifications")]
[SerializeField] private TextMeshProUGUI alertText; // A large text element in the center of the screen
private int _lastDifficultyLevel = 0;

    [Header("Slow Motion Settings")]
    [SerializeField] private float slowMoFactor = 0.05f; // How slow time goes (0.05 is very slow)
    [SerializeField] private float slowMoDuration = 0.1f; // How long it stays slow

    [SerializeField] private TextMeshProUGUI comboText;

    void Update()
{
    // Decrease the combo timer over time
    if (comboTimer > 0)
    {
        comboTimer -= Time.deltaTime;
        if (comboTimer <= 0)
        {
            currentCombo = 0; // Reset combo if player is too slow
        }
    }
    // Inside Update() after the timer logic:
    if (comboText != null)
    {
        comboText.text = currentCombo > 1 ? $"COMBO X{currentCombo}" : "";
    }
    if (comboText != null)
{
    // Smoothly scale back to normal size
    comboText.transform.localScale = Vector3.Lerp(comboText.transform.localScale, Vector3.one, Time.deltaTime * 10f);
}
// ONLY recover time if timeScale is NOT zero (not paused) 
    // and not game over
    if (Time.timeScale > 0f && Time.timeScale < 1f && !isGameOver)
    {
        Time.timeScale += (1f / 0.5f) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    
    CheckDifficultyIncrease();
}

private void CheckDifficultyIncrease()
{
    Stopwatch sw = FindObjectOfType<Stopwatch>();
    if (sw == null) return;

    int currentLevel = Mathf.FloorToInt(sw.GetElapsedTime() / 120f);

    if (currentLevel > _lastDifficultyLevel)
    {
        _lastDifficultyLevel = currentLevel;
        
        // Trigger both the warning and the reward!
        TriggerSurvivalReward();
        TriggerSlowMo(); 
    }
}

public void TriggerAlert(string message)
{
    if (alertText == null) return;
    
    alertText.text = message;
    alertText.gameObject.SetActive(true);
    
    // Stop any existing fade and start a new one
    StopCoroutine("FadeAlert");
    StartCoroutine(FadeAlert());
}

private IEnumerator FadeAlert()
{
    // Simple fade out over 2 seconds
    float elapsed = 0f;
    float duration = 2f;
    Color startColor = alertText.color;
    startColor.a = 1f;
    alertText.color = startColor;

    yield return new WaitForSeconds(1f); // Stay solid for 1 second

    while (elapsed < duration)
    {
        elapsed += Time.unscaledDeltaTime;
        startColor.a = Mathf.Lerp(1f, 0f, elapsed / duration);
        alertText.color = startColor;
        yield return null;
    }

    alertText.gameObject.SetActive(false);
}

public void TriggerSlowMo()
{
    Time.timeScale = slowMoFactor;
    Time.fixedDeltaTime = Time.timeScale * 0.02f;
}

public void AddCombo()
{
    currentCombo++;
    comboTimer = comboExpiryTime;

    // Find the camera shake script
    CameraShake shaker = Camera.main.GetComponent<CameraShake>();
    if (shaker != null)
    {
        // Default small shake for every hit
        float intensity = 0.05f;
        float duration = 0.1f;

        // "Juice" it up: Bigger shake every 5 combo hits
        if (currentCombo % 5 == 0)
        {
            intensity = 0.2f;
            duration = 0.2f;
            Debug.Log("MEGA COMBO SHAKE!");
        }

        shaker.Shake(duration, intensity);
    }
    if (comboText != null)
{
    comboText.transform.localScale = Vector3.one * 1.5f; // Pop to 150% size
}
// Trigger Slow-Mo every 10 hits
    if (currentCombo > 0 && currentCombo % 10 == 0)
    {
        TriggerSlowMo();
        Debug.Log("SLOW MOTION IMPACT!");
    }
}

public void TriggerSurvivalReward()
{
    // 1. Show a special message
    TriggerAlert("<color=green>SURVIVAL REWARD:\nSHOCKWAVE!</color>");

    // 2. Find every object that uses our ISpawnable interface
    // This finds Hander, CigaretEel, and CartenCrab all at once!
    MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();
    
    foreach (MonoBehaviour script in allScripts)
    {
        if (script is ISpawnable)
        {
            // Use the SendOutward logic if it's a Hander-style script
            // We use SendMessage as a quick way to trigger the behavior 
            // without knowing the exact class type.
            script.SendMessage("SendOutward", 2.0f, SendMessageOptions.DontRequireReceiver);
        }
    }

    // 3. Add a big screen shake for the "Blast"
    CameraShake shaker = Camera.main.GetComponent<CameraShake>();
    if (shaker != null) shaker.Shake(0.5f, 0.4f);
}

    public void GameOver()
{
    isGameOver = true;
    Time.timeScale = 0; // Freeze the world!

    // IMPORTANT: Hide the HUD timer so it doesn't stay on screen
    // If you have a reference to the gameplay timer object:
    // if (hudTimerObject != null) hudTimerObject.SetActive(false);

    // 1. Handle Trap/Timer UI
    if (crabTrapParent != null) crabTrapParent.SetActive(false);
    // if (hudTimerObject != null) hudTimerObject.SetActive(false); // Enable this if you have the HUD reference

    UIManager _ui = GetComponent<UIManager>();
    Stopwatch sw = FindObjectOfType<Stopwatch>();

    if (_ui != null && sw != null)
    {
        sw.SaveToTotalTime();

        float current = sw.GetElapsedTime();
        float best = PlayerPrefs.GetFloat("HighScore", 0f);

        // 2. DECLARE Rank and RecordMessage FIRST
        string rank = (current < 60) ? "BRONZE" : (current < 120) ? "SILVER" : "GOLD";
        string recordMessage = "";

        if (current >= best) 
        {
            recordMessage = "<color=yellow>NEW BEST!</color>\n";
            PlayerPrefs.SetFloat("HighScore", current);
            PlayerPrefs.Save();
            best = current; // Update best so the UI shows the new score
        }

        // 3. Create the formatted string ONCE
        float multiplier = GetCurrentMultiplier();
        string finalDisplay = $"{recordMessage}" +
                              $"Rank: {rank}\n" +
                              $"Multiplier: {multiplier:F1}x\n" +
                              $"Survived: {current:F1}s\n" +
                              $"Best: {best:F1}s";

        _ui.ShowDeathPanelWithFormattedTime(finalDisplay);
    }
}

public float GetCurrentMultiplier()
{
    Stopwatch sw = FindObjectOfType<Stopwatch>();
    float timeBonus = 1f;

    if (sw != null)
    {
        // Survival Bonus: +0.5x every 30 seconds
        timeBonus = 1f + (Mathf.Floor(sw.GetElapsedTime() / 30f) * 0.5f);
    }

    // Combo Bonus: Every combo point adds 0.1x to the multiplier
    float comboBonus = currentCombo * 0.1f;

    return timeBonus + comboBonus;
}
}