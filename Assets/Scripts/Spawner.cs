using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeightedTrash
{
    public GameObject prefab;
    [Range(1, 100)] public int weight = 10;
    
    // NEW: Slider for spawn delay specific to this item type
    [Range(0f, 100f)] public float specificSpawnDelay = 0.5f;
}

public class Spawner : MonoBehaviour
{
    [Header("Trash Configuration")]
    [SerializeField] private List<WeightedTrash> itemsToSpawn;
    
    [Header("Fixed Timing")]
    [SerializeField] private float spawnInterval = 1f;

    [Header("Placement Randomization")]
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private bool randomizeRotation = true;

    private float _timeTillNextSpawn = 0f;

    void Update()
    {
        if (itemsToSpawn == null || itemsToSpawn.Count == 0) return;

        if (_timeTillNextSpawn > 0f)
        {
            _timeTillNextSpawn -= Time.deltaTime;
            return;
        }

        SpawnOne();
        _timeTillNextSpawn = spawnInterval; 
    }

    private void SpawnOne()
    {
        // 1. Get the random weighted item data
        WeightedTrash selectedData = GetWeightedRandomItem();
        if (selectedData == null || selectedData.prefab == null) return;

        // 2. Random Position & Rotation
        Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = transform.position + new Vector3(randomCirclePoint.x, randomCirclePoint.y, 0f);

        Quaternion spawnRot = randomizeRotation 
            ? Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) 
            : transform.rotation;

        GameObject spawned = Instantiate(selectedData.prefab, spawnPos, spawnRot);

        // 3. APPLY THE SPECIFIC DELAY
        // We use the delay value defined in the slider for this specific item
        float delayToApply = selectedData.specificSpawnDelay;

        if (spawned.TryGetComponent(out Hander h)) h.SetSpawnDelay(delayToApply);
        else if (spawned.TryGetComponent(out CigaretEel e)) e.SetSpawnDelay(delayToApply);
        else if (spawned.TryGetComponent(out CartenCrab c)) c.SetSpawnDelay(delayToApply);
    }

    // Modified to return the whole data object so we can access the delay slider
    private WeightedTrash GetWeightedRandomItem()
    {
        int totalWeight = 0;
        foreach (var item in itemsToSpawn) totalWeight += item.weight;

        int randomRoll = Random.Range(0, totalWeight);
        int currentWeightSum = 0;

        foreach (var item in itemsToSpawn)
        {
            currentWeightSum += item.weight;
            if (randomRoll < currentWeightSum) return item;
        }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}