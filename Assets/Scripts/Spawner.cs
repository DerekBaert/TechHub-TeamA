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
    [SerializeField] private SpawnMode spawnMode = SpawnMode.OnViewEdgesAroundCamera; // Changed from AnywhereInView

    [Header("Timing")]
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    private float _timeTillNextSpawn = 0f;

    [Header("Optional player gating")]
    [SerializeField] private PlayerController player;

    [Header("Viewport spawn settings")]
    [Tooltip("Padding inside the viewport (0..0.5). 0 = full viewport, 0.1 = avoid 10% from each edge.")]
    [Range(0f, 0.45f)] [SerializeField] private float viewportPadding = 0.05f;

    [Tooltip("When spawning on edges, how far outside the viewport to place the spawn (world units)")]
    [SerializeField] private float edgeSpawnOffset = 1f;

    [Tooltip("Randomize spawned rotation")]
    [SerializeField] private bool randomizeRotation = false;

    void Update()
    {
        bool shouldSpawn = _timeTillNextSpawn <= 0f;
        if (player != null) shouldSpawn = shouldSpawn && player.IsAlive;

        if (!shouldSpawn)
        {
            _timeTillNextSpawn -= Time.deltaTime;
            return;
        }

        SpawnOne();
        _timeTillNextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnOne()
    {
        if (itemToSpawn == null)
        {
            Debug.LogWarning("Spawner: itemToSpawn is null.");
            return;
        }

        Vector3 spawnPos = transform.position;
        Quaternion spawnRot = transform.rotation;

        Camera cam = Camera.main;
        if (spawnMode == SpawnMode.AtSpawner || cam == null)
        {
            // keep transform.position (fallback if no camera)
            spawnPos = transform.position;
        }
        else if (spawnMode == SpawnMode.AnywhereInView)
        {
            // pick a random point inside the camera viewport, respecting padding
            float minV = viewportPadding;
            float maxV = 1f - viewportPadding;
            float vx = Random.Range(minV, maxV);
            float vy = Random.Range(minV, maxV);

            float zDist = Mathf.Abs(cam.transform.position.z - transform.position.z);
            spawnPos = cam.ViewportToWorldPoint(new Vector3(vx, vy, zDist));
        }
        else if (spawnMode == SpawnMode.OnViewEdgesAroundCamera)
        {
            // choose a random side and a coordinate along that side, spawn just outside viewport
            int side = Random.Range(0, 4); // 0=left,1=right,2=bottom,3=top
            float along = Random.Range(viewportPadding, 1f - viewportPadding);
            float zDist = Mathf.Abs(cam.transform.position.z - transform.position.z);

            switch (side)
            {
                case 0: // left
                    spawnPos = cam.ViewportToWorldPoint(new Vector3(0f - 0.001f, along, zDist));
                    spawnPos.x -= edgeSpawnOffset;
                    break;
                case 1: // right
                    spawnPos = cam.ViewportToWorldPoint(new Vector3(1f + 0.001f, along, zDist));
                    spawnPos.x += edgeSpawnOffset;
                    break;
                case 2: // bottom
                    spawnPos = cam.ViewportToWorldPoint(new Vector3(along, 0f - 0.001f, zDist));
                    spawnPos.y -= edgeSpawnOffset;
                    break;
                case 3: // top
                    spawnPos = cam.ViewportToWorldPoint(new Vector3(along, 1f + 0.001f, zDist));
                    spawnPos.y += edgeSpawnOffset;
                    break;
            }
        }

        if (randomizeRotation)
        {
            spawnRot = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        }

        Instantiate(itemToSpawn, spawnPos, spawnRot);
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
