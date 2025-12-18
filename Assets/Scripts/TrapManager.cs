using UnityEngine;
using System.Collections.Generic;

public static class TrapManager 
{
    // A static list that holds all active CrabTraps
    private static List<CrabTrap> _allTraps = new List<CrabTrap>();

    public static void RegisterTrap(CrabTrap trap)
    {
        if (!_allTraps.Contains(trap)) _allTraps.Add(trap);
    }

    public static void UnregisterTrap(CrabTrap trap)
    {
        if (_allTraps.Contains(trap)) _allTraps.Remove(trap);
    }

    // This replaces the expensive FindObjectsOfType call
    public static int GetAvailableTrapCount()
    {
        int count = 0;
        foreach (var trap in _allTraps)
        {
            // Only count if it's NOT on cooldown
            if (trap != null && !trap.IsOnCooldown)
            {
                count++;
            }
        }
        return count;
    }
}