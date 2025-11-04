using UnityEngine;

// This class is responsible for spawning a specified item at timed intervals.
public class Spawner : MonoBehaviour
{
    // [SerializeField] makes this private field visible in the Inspector.
    // It's a best practice for providing editor access while maintaining encapsulation.
    [SerializeField]
    private GameObject itemToSpawn;

    // Use a range for spawning items to add variety.
    [SerializeField]
    private float minSpawnInterval = 1f;

    [SerializeField]
    private float maxSpawnInterval = 3f;

    // A private float to count down to the next spawn event.
    // Initialized to 0 so the first item can spawn immediately.
    private float _timeTillNextSpawn = 0f;

    // Optional: Add a reference to check the player's status, just like the obstacle spawner.
    // This makes the spawner smarter and more game-aware.
    // [SerializeField] allows us to drag and drop the PlayerController in the Inspector.
    [SerializeField]
    private PlayerController player;

    // Update is called once per frame. This is where the core spawning logic runs.
    void Update()
    {
        // Check if the timer has run out. If you have a player, also check if they're alive.
        // We use a null check for 'player' to make this optional.
        bool shouldSpawn = _timeTillNextSpawn <= 0;
        if (player != null)
        {
            shouldSpawn = shouldSpawn && player.IsAlive;
        }

        if (shouldSpawn)
        {
            // Calculate the next spawn time.
            float nextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);

            // Spawn the object at the spawner's position and rotation.
            // Using `transform.position` and `transform.rotation` is clean.
            Instantiate(itemToSpawn, transform.position, transform.rotation);

            // Reset the timer for the next spawn cycle.
            _timeTillNextSpawn = nextSpawn;
        }
        else
        {
            // If the timer is still running, decrement it by the time passed since the last frame.
            _timeTillNextSpawn -= Time.deltaTime;
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
