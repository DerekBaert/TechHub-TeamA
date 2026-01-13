using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Spawner> spawners;

    [Header("Wave Settings")]
    public int currentWave = 1;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int baseEnemiesPerWave = 10;
    [SerializeField] private float spawnRateInWave = 1.0f; // Seconds between spawns during wave

    private bool _isWaveActive = false;
    private int _enemiesSpawnedInCurrentWave = 0;

    void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        while (true) // Loop waves forever
        {
            _isWaveActive = false;

            int highestWave = PlayerPrefs.GetInt("HighestWave", 1);
            if (currentWave > highestWave)
            {
                PlayerPrefs.SetInt("HighestWave", currentWave);
                PlayerPrefs.Save();
            }
            
            // 1. Tell LevelManager or UI about the new wave
            LevelManager.instance?.TriggerAlert($"WAVE {currentWave}\nREADY?");
            
            // 2. Wait for the rest period
            yield return new WaitForSeconds(timeBetweenWaves);

            // 3. Start the Wave
            _isWaveActive = true;
            _enemiesSpawnedInCurrentWave = 0;
            int totalToSpawn = baseEnemiesPerWave + (currentWave * 5); // Waves get bigger
            float currentRate = Mathf.Max(0.2f, spawnRateInWave - (currentWave * 0.05f)); // Waves get faster

            LevelManager.instance?.TriggerAlert("CLEANUP START!");

            while (_enemiesSpawnedInCurrentWave < totalToSpawn)
            {
                if (Time.timeScale > 0)
                {
                    TriggerRandomSpawner();
                    _enemiesSpawnedInCurrentWave++;
                    yield return new WaitForSeconds(currentRate);
                }
                else
                {
                    yield return null; // Wait if paused
                }
            }

            // 4. Wave End
            currentWave++;
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
            LevelManager.instance?.TriggerAlert("WAVE CLEAR!");
            yield return new WaitForSeconds(2f);
        }
    }

    void TriggerRandomSpawner() 
    {
        if (spawners.Count == 0) return;
        
        // Occasionally spawn multiple to keep it hard
        int count = (Random.value > 0.8f) ? 2 : 1; 
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, spawners.Count);
            spawners[index].ManualSpawn();
        }
    }
}