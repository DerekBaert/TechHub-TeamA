using UnityEngine;
using TMPro;
using System.Reflection;

public class numberOftraps : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trapsText; // drag TextMeshProUGUI in Inspector

    [Header("Initial lock")]
    [Tooltip("Seconds at level start during which traps are not available.")]
    [SerializeField] private float initialLockDuration = 30f;

    void Update()
    {
        if (trapsText == null) return;

        // while the global initial lock is active show 0 available (no countdown)
        if (Time.timeSinceLevelLoad < initialLockDuration)
        {
            trapsText.text = "Crab Traps Available: 0";
            return;
        }

        // find all CrabTrap instances in scene
        var traps = FindObjectsOfType<CrabTrap>();
        int available = 0;

        foreach (var t in traps)
        {
            // Prefer an explicit public UsesRemaining property if CrabTrap exposes it
            var prop = t.GetType().GetProperty("UsesRemaining", BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.PropertyType == typeof(int))
            {
                int uses = (int)prop.GetValue(t);
                if (uses > 0) available++;
                continue;
            }

            // Fallback: if CrabTrap exposes a public isOnCooldown (bool) property, count as available when not on cooldown
            var cooldownProp = t.GetType().GetProperty("isOnCooldown", BindingFlags.Public | BindingFlags.Instance)
                               ?? t.GetType().GetProperty("IsOnCooldown", BindingFlags.Public | BindingFlags.Instance);
            if (cooldownProp != null && cooldownProp.PropertyType == typeof(bool))
            {
                bool onCd = (bool)cooldownProp.GetValue(t);
                if (!onCd && t.gameObject.activeInHierarchy) available++;
                continue;
            }

            // Last fallback: count active templates (assume active and not on cooldown are available)
            if (t.gameObject.activeInHierarchy) available++;
        }

        trapsText.text = $"Crab Traps Available: {available}";
    }
}

