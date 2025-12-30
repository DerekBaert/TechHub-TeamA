using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Spawner> spawners; // Drag all 7 spawners here

    [Header("Global Timing")]
    [SerializeField] private float spawnInterval = 2.0f; // Total time between any spawn
    private float _timer;

    void Update()
    {
        // 1. Respect the Pause!
        if (Time.timeScale == 0) return;

        _timer += Time.deltaTime;

        if (_timer >= spawnInterval)
        {
            TriggerRandomSpawner();
            _timer = 0;
            
            // Optional: Gradually decrease interval over time to increase difficulty
            // spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.01f);
        }
    }

    void TriggerRandomSpawner()
    {
        if (spawners == null || spawners.Count == 0) return;

        // Pick a random spawner from your list
        int index = Random.Range(0, spawners.Count);
        
        // We will add a "ManualSpawn" method to your Spawner script next
        spawners[index].ManualSpawn();
    }
}