using UnityEngine;

// This class is responsible for spawning a specified item at timed intervals.
public class Spawner : MonoBehaviour
{
    public enum SpawnMode
    {
        AtSpawner,
        AnywhereInView,
        OnViewEdgesAroundCamera
    }

    [SerializeField] private GameObject itemToSpawn;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;

    // optional: per-spawn delay range applied to each spawned Hander instance
    [Header("Spawned object delay (before they start moving)")]
    [Tooltip("If min==max, that exact delay is used. Otherwise a random value in [min,max] is chosen.")]
    [SerializeField] private float spawnedDelayMin = 0f;
    [SerializeField] private float spawnedDelayMax = 0f;

    private float _timeTillNextSpawn = 0f;

    void Update()
    {
        if (_timeTillNextSpawn > 0f)
        {
            _timeTillNextSpawn -= Time.deltaTime;
            return;
        }

        SpawnOne();
        _timeTillNextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnOne()
    {
        if (itemToSpawn == null) return;

        // compute spawnPos / spawnRot per your existing logic
        Vector3 spawnPos = transform.position;
        Quaternion spawnRot = transform.rotation;

        GameObject spawned = Instantiate(itemToSpawn, spawnPos, spawnRot);

        // set a per-spawn delay on the spawned Hander (if present)
        float delay = Mathf.Approximately(spawnedDelayMin, spawnedDelayMax)
            ? spawnedDelayMin
            : Random.Range(spawnedDelayMin, spawnedDelayMax);

        var handler = spawned.GetComponent<Hander>();
        if (handler != null)
        {
            handler.SetSpawnDelay(delay);
        }
        else
        {
            // If the spawned prefab doesn't have Hander, you can optionally try a component name or log
            // Debug.LogWarning("Spawned object has no Hander component to set spawn delay on.");
        }
    }
}

// A simple PlayerController class to demonstrate the dependency check.
// This would need to be a separate script on your player GameObject.
public class PlayerController : MonoBehaviour
{
    public bool IsAlive { get; private set; } = true;

    // A method that can be called by other scripts to set the player's status.
    public void Die()
    {
        IsAlive = false;
        Debug.Log("Player is dead, spawner will stop.");
    }
}
