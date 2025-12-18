using UnityEngine;
using TMPro;

public class NumberOfTraps : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trapsText;
    [SerializeField] private float initialLockDuration = 30f;

    void Update()
    {
        if (trapsText == null) return;

        // 1. Check the global initial lock
        if (Time.timeSinceLevelLoad < initialLockDuration)
        {
            trapsText.text = "Crab Traps Available: 0";
            return;
        }

        // 2. Ask the Manager for the count (This is now very fast)
        int availableCount = TrapManager.GetAvailableTrapCount();

        // 3. Update UI
        trapsText.text = $"Crab Traps Available: {availableCount}";
    }
}