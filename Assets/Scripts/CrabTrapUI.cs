using UnityEngine;
using TMPro;

public class CrabTrapUI : MonoBehaviour
{
    [SerializeField] private CrabTrap trapSource; 
    [SerializeField] private TextMeshProUGUI statusText;

    void Update()
    {
        if (trapSource == null || statusText == null) return;

        // 1. Get the restock time remaining
        float restockTime = trapSource.GetRestockTimeRemaining();
        int charges = trapSource.CurrentCharges;

        // 2. Decide what to display
        if (Time.timeSinceLevelLoad < 15f) // Using the same duration as your trap
        {
            float unlockIn = 15f - Time.timeSinceLevelLoad;
            statusText.text = $"Unlocking in: {unlockIn:F1}s";
        }
        else if (charges > 0)
        {
            statusText.text = $"Traps: x{charges}";
            if (charges < 2) // If restocking the second charge
            {
                statusText.text += $" (Next: {restockTime:F0}s)";
            }
        }
        else
        {
            statusText.text = $"Restocking: {restockTime:F1}s";
        }
    }
}