using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeightedTrash
{
    public GameObject prefab;
    [Range(1, 100)] public int weight = 10;
    [Range(0f, 120f)] public float specificSpawnDelay = 0.5f;
}

public class Spawner : MonoBehaviour
{
    enum SpawnerType { Stationary, Spin, BackAndForth }

    [Header("Movement (The Pro Logic)")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float spinSpeed = 100f;
    [SerializeField] private float moveSpeed = 2f;
    public Transform pos1, pos2;
    private bool _movingToPos2 = true;

    [Header("Trash Configuration")]
    [SerializeField] private List<WeightedTrash> itemsToSpawn;
    [SerializeField] private float spawnRadius = 2f;
    [SerializeField] private bool randomizeRotation = true;

    // Triggered by SpawnManager
    public void ManualSpawn() 
    {
        WeightedTrash selectedData = GetWeightedRandomItem();
        if (selectedData == null || selectedData.prefab == null) return;

        Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = transform.position + new Vector3(randomCirclePoint.x, randomCirclePoint.y, 0f);

        Quaternion spawnRot = randomizeRotation 
            ? Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) 
            : transform.rotation;

        GameObject spawned = Instantiate(selectedData.prefab, spawnPos, spawnRot);

        if (spawned.TryGetComponent(out ISpawnable s)) 
        {
            s.SetSpawnDelay(selectedData.specificSpawnDelay);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        // Pro Rotation Logic
        if (spawnerType == SpawnerType.Spin)
        {
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        }

        // Pro Movement Logic
        if (spawnerType == SpawnerType.BackAndForth && pos1 != null && pos2 != null)
        {
            Transform target = _movingToPos2 ? pos2 : pos1;
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < 0.01f)
                _movingToPos2 = !_movingToPos2;
        }
    }

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
        if(pos1 && pos2) Gizmos.DrawLine(pos1.position, pos2.position);
    }
}