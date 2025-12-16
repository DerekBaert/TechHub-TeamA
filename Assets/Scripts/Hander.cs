using UnityEngine;

public class Hander : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 20f;

    // new: per-instance spawn delay (time before this instance becomes active/moves)
    [Header("Spawn timing")]
    [Tooltip("Time after Instantiate before this object starts moving")]
    public float spawnDelay = 0f; 

    // internal timer and active flag
    private float _spawnTimer;
    private bool _isActive;

    [Header("HomeBase impact")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private string homeBaseTag = "HomeBase";
    [Tooltip("Optional sound to play when this trash collides with the HomeBase.")]
    public AudioClip impactSfx;

    [Tooltip("Volume to play the impact SFX at (0.0 - 1.0).")]
    [Range(0f, 1f)]
    public float impactSfxVolume = 1f;

    private Transform homeTransform;
    private bool hasDamaged = false;

    // NEW: current travel direction and outbound flag
    private Vector3 currentDirection;
    private bool isOutbound = false;
    private Transform targetTransform; // can be HomeBase or CrabTrap
    void Start()
    {
        // initialize spawn timer from the public spawnDelay (can be set by Spawner after Instantiate)
        _spawnTimer = spawnDelay;
        _isActive = _spawnTimer <= 0f;

        // find HomeBase once
        var home = GameObject.FindGameObjectWithTag(homeBaseTag);
        if (home != null)
        {
            homeTransform = home.transform;
            targetTransform = homeTransform;
        }
        else
        {
            Debug.LogWarning($"Hander: No object with tag '{homeBaseTag}' found. Falling back to downward movement.");
        }

        // initialize direction toward target if available, otherwise down
        if (targetTransform != null)
            currentDirection = (targetTransform.position - transform.position).normalized;
        else
            currentDirection = Vector3.down;
    }

    // public API so Spawner can set a custom per-spawn delay immediately after Instantiate
    public void SetSpawnDelay(float delay)
    {
        spawnDelay = delay;
        _spawnTimer = delay;
        _isActive = _spawnTimer <= 0f;
    }

    void Update()
    {
        if (!_isActive)
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f) _isActive = true;
            else return;
        }

        // move toward current target (HomeBase or CrabTrap)
        if (!isOutbound && targetTransform != null)
        {
            currentDirection = (targetTransform.position - transform.position).normalized;
        }

        transform.position += currentDirection * moveSpeed * Time.deltaTime;
    }

    // new: called by CrabTrap to redirect this trash toward the trap
    public void SendTowardTarget(Transform newTarget)
    {
        targetTransform = newTarget;
        isOutbound = false;
    }

    public void SendOutward(float speedMultiplier = 1f)
    {
        // ...existing code...
        if (homeTransform != null)
        {
            currentDirection = (transform.position - homeTransform.position).normalized;
        }
        else
        {
            currentDirection = Vector3.up;
        }

        moveSpeed *= speedMultiplier;
        isOutbound = true;
    }

    // Use trigger collisions to detect contact with HomeBase.
    // Recommended setup:
    // - Trash prefab: Collider2D (Is Trigger = true) + Rigidbody2D (Body Type: Kinematic)
    // - HomeBase: Collider2D (Is Trigger = false) + tag set to "HomeBase"
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDamaged) return;
        if (other.CompareTag(homeBaseTag))
        {
            other.gameObject.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
            // play impact SFX at this object's position if assigned
            if (impactSfx != null)
            {
                AudioSource.PlayClipAtPoint(impactSfx, transform.position, impactSfxVolume);
            }

            hasDamaged = true;
            Destroy(gameObject);
        }
    }

    // In case you use non-trigger colliders, you can also handle collision-based contact:
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasDamaged) return;

        if (collision.gameObject.CompareTag(homeBaseTag))
        {
            collision.gameObject.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
            collision.gameObject.SendMessage("health", damageAmount, SendMessageOptions.DontRequireReceiver);

            // play impact SFX at this object's position if assigned
            if (impactSfx != null)
            {
                AudioSource.PlayClipAtPoint(impactSfx, transform.position, impactSfxVolume);
            }

            hasDamaged = true;
            Destroy(gameObject);
        }
    }
}
