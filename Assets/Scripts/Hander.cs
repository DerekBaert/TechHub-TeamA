using UnityEngine;

public class Hander : MonoBehaviour, ISpawnable
{
    [SerializeField] private float moveSpeed = 20f;
    
    [Header("Interaction Settings")]
    [Tooltip("How many clicks before this moves away. Default is 1.")]
    [SerializeField] private int maxClicks = 1; 
    private int _currentClicks = 0;

    [Header("Spawn timing")]
    public float spawnDelay = 0f; 

    [Header("HomeBase impact")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private string homeBaseTag = "HomeBase";
    public AudioClip impactSfx;
    [Range(0f, 1f)] public float impactSfxVolume = 1f;
    private static float _gameStartTime = -1f;
    private float _initialSpeed;
    private Camera _mainCam;
    
    private float _spawnTimer;
    private bool _isActive;
    private Transform homeTransform;
    private bool hasDamaged = false;
    private Vector3 currentDirection;
    private bool isOutbound = false;
    private Transform targetTransform; 

    void Awake()
    {
        if (_gameStartTime < 0) _gameStartTime = Time.time;
        _initialSpeed = moveSpeed;
        _mainCam = Camera.main;
    }

    void Start()
    {
        _spawnTimer = spawnDelay;
        _isActive = _spawnTimer <= 0f;

        var home = GameObject.FindGameObjectWithTag(homeBaseTag);
        if (home != null)
        {
            homeTransform = home.transform;
            targetTransform = homeTransform;
        }

        if (targetTransform != null)
            currentDirection = (targetTransform.position - transform.position).normalized;
        else
            currentDirection = Vector3.down;

        UpdateSpeedBasedOnTime();
    }

    void Update()
    {
        // If the game is over, stop moving
        if (LevelManager.instance != null && LevelManager.instance.isGameOver) return;
        UpdateSpeedBasedOnTime();

        if (!_isActive)
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f) _isActive = true;
            else return;
        }

        if (!isOutbound && targetTransform != null)
        {
            currentDirection = (targetTransform.position - transform.position).normalized;
        }

        transform.position += currentDirection * moveSpeed * Time.deltaTime;

        CheckBounds();
    }

    // --- NEW: Click detection logic ---
    // Replace your current OnMouseDown with this public method
public void ReceiveClick()
{
    if (!_isActive || isOutbound) return;

    _currentClicks++;

    // NEW: Add to combo when clicked
    if (LevelManager.instance != null)
    {
        LevelManager.instance.AddCombo();
    }

    if (_currentClicks >= maxClicks)
    {
        SendOutward(1.5f); 
        _currentClicks = 0; 
    }
}

// Keep OnMouseDown so it still works if the player clicks the object directly 
// without the Melee area overlapping it.
private void OnMouseDown()
{
    ReceiveClick();
}

    private void UpdateSpeedBasedOnTime()
{
    float timePassed = Time.time - _gameStartTime;
    
    // Linear Scaling: Increase speed by 10% every minute
    // This is much smoother than doubling it!
    float speedMultiplier = 1f + (timePassed / 60f) * 0.1f;
    
    // Cap the max speed so it never becomes "teleporting" fast
    float maxSpeed = _initialSpeed * 3f; 
    
    moveSpeed = Mathf.Min(_initialSpeed * speedMultiplier, maxSpeed);
}

    private void CheckBounds()
    {
        if (_mainCam == null) return;
        Vector3 viewPos = _mainCam.WorldToViewportPoint(transform.position);
        if (viewPos.x < -0.5f || viewPos.x > 1.5f || viewPos.y < -0.5f || viewPos.y > 1.5f)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpawnDelay(float delay)
    {
        spawnDelay = delay;
        _spawnTimer = delay;
        _isActive = _spawnTimer <= 0f;
    }

    public void SendTowardTarget(Transform newTarget)
    {
        targetTransform = newTarget;
        isOutbound = false;
    }

    public void SendOutward(float speedMultiplier = 1f)
    {
        if (homeTransform != null)
            currentDirection = (transform.position - homeTransform.position).normalized;
        else
            currentDirection = Vector3.up;

        moveSpeed *= speedMultiplier;
        isOutbound = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDamaged || isOutbound) return; // Don't damage if already moving away
        if (other.CompareTag(homeBaseTag))
        {
            ExecuteImpact(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasDamaged || isOutbound) return;
        if (collision.gameObject.CompareTag(homeBaseTag))
        {
            ExecuteImpact(collision.gameObject);
        }
    }

    private void ExecuteImpact(GameObject target)
    {
        target.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
        if (impactSfx != null)
        {
            AudioSource.PlayClipAtPoint(impactSfx, transform.position, impactSfxVolume);
        }
        hasDamaged = true;
        Destroy(gameObject);
    }
}