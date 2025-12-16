using UnityEngine;
using TMPro;
using System.Reflection;

public class cooldownTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cooldownText;

    [Header("Initial global lock")]
    [Tooltip("Seconds to wait before trap UI and availability logic runs (show countdown).")]
    [SerializeField] private float initialCountdown = 30f;
    private float _initialRemaining;

    void Start()
    {
        if (cooldownText == null)
        {
            Debug.LogWarning("cooldownTime: cooldownText not assigned in Inspector.");
        }

        _initialRemaining = Mathf.Max(0f, initialCountdown);
    }

    void Update()
    {
        if (cooldownText == null) return;

        // Initial global countdown before trap logic runs
        if (_initialRemaining > 0f)
        {
            _initialRemaining -= Time.deltaTime;
            float display = Mathf.Max(0f, _initialRemaining);
            cooldownText.text = $"Traps unlock in: {display:F1}s";
            return;
        }

        // existing logic: find all CrabTrap instances in scene
        var traps = FindObjectsOfType<CrabTrap>();
        if (traps == null || traps.Length == 0)
        {
            cooldownText.text = "No traps";
            return;
        }

        // compute minimum remaining cooldown among traps (0 if any trap already ready)
        float minRemaining = float.MaxValue;
        int totalUsed = 0;

        foreach (var t in traps)
        {
            // try to read public properties first
            bool onCd = false;
            float remaining = 0f;
            try
            {
                // public read-only properties that exist on CrabTrap
                var isOnCdProp = t.GetType().GetProperty("isOnCooldown", BindingFlags.Public | BindingFlags.Instance);
                var cdTimerProp = t.GetType().GetProperty("cooldownTimer", BindingFlags.Public | BindingFlags.Instance);

                if (isOnCdProp != null && cdTimerProp != null)
                {
                    onCd = (bool)isOnCdProp.GetValue(t);
                    remaining = (float)cdTimerProp.GetValue(t);
                }
                else
                {
                    // fallback to fields (private)
                    var isOnCdField = t.GetType().GetField("_isOnCooldown", BindingFlags.NonPublic | BindingFlags.Instance)
                                     ?? t.GetType().GetField("isOnCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
                    var cdTimerField = t.GetType().GetField("_cooldownTimer", BindingFlags.NonPublic | BindingFlags.Instance)
                                     ?? t.GetType().GetField("cooldownTimer", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (isOnCdField != null && cdTimerField != null)
                    {
                        onCd = (bool)isOnCdField.GetValue(t);
                        remaining = (float)cdTimerField.GetValue(t);
                    }
                    else
                    {
                        // if nothing found, assume ready
                        onCd = false;
                        remaining = 0f;
                    }
                }
            }
            catch
            {
                onCd = false;
                remaining = 0f;
            }

            if (!onCd)
            {
                minRemaining = 0f;
            }
            else
            {
                minRemaining = Mathf.Min(minRemaining, remaining);
            }

            // compute how many times this trap has been used (maxUses - usesRemaining)
            int maxUses = 0;
            int usesRemaining = 0;
            bool gotCounts = false;

            // try fields first
            var maxField = t.GetType().GetField("maxUses", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var usesField = t.GetType().GetField("usesRemaining", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (maxField != null && usesField != null)
            {
                try
                {
                    maxUses = (int)maxField.GetValue(t);
                    usesRemaining = (int)usesField.GetValue(t);
                    gotCounts = true;
                }
                catch { gotCounts = false; }
            }

            // fallback to properties named UsesRemaining / MaxUses
            if (!gotCounts)
            {
                var maxProp = t.GetType().GetProperty("MaxUses", BindingFlags.Public | BindingFlags.Instance)
                              ?? t.GetType().GetProperty("maxUses", BindingFlags.Public | BindingFlags.Instance);
                var usesProp = t.GetType().GetProperty("UsesRemaining", BindingFlags.Public | BindingFlags.Instance)
                              ?? t.GetType().GetProperty("usesRemaining", BindingFlags.Public | BindingFlags.Instance);

                if (maxProp != null && usesProp != null)
                {
                    try
                    {
                        maxUses = (int)maxProp.GetValue(t);
                        usesRemaining = (int)usesProp.GetValue(t);
                        gotCounts = true;
                    }
                    catch { gotCounts = false; }
                }
            }

            if (gotCounts)
            {
                int used = Mathf.Clamp(maxUses - usesRemaining, 0, maxUses);
                totalUsed += used;
            }
        }

        // if minRemaining stayed float.MaxValue then no trap had cooldown field/readable -> treat as available
        if (minRemaining == float.MaxValue) minRemaining = 0f;

        // add 15 seconds for every crab trap used
        float extra = totalUsed * 15f;
        float nextAvailableIn = Mathf.Max(0f, minRemaining + extra);

        if (nextAvailableIn <= 0f)
        {
            cooldownText.text = "Trap Available";
        }
        else
        {
            cooldownText.text = $"Next Trap In: {nextAvailableIn:F1}s";
        }
    }
}