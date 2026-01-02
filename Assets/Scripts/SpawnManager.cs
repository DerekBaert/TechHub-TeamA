using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Spawner> spawners; // Drag all 7 spawners here

    [SerializeField] private float minInterval = 0.5f; // Faster it can go
    [SerializeField] private float difficultyIncreaseRate = 0.05f; // Speed up per spawn

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
        if (_timer >= spawnInterval) 
        {
            TriggerRandomSpawner();
            _timer = 0;
            // This ensures it gets harder the longer you survive
            spawnInterval = Mathf.Max(minInterval, spawnInterval - difficultyIncreaseRate); 
        }
    }

    void TriggerRandomSpawner() 
    {
        int itemsToSpawn = (Random.value > 0.8f) ? 3 : 1; // 20% chance to spawn 3 items at once
        for (int i = 0; i < itemsToSpawn; i++) {
        int index = Random.Range(0, spawners.Count);
        spawners[index].ManualSpawn();
    }
}
}