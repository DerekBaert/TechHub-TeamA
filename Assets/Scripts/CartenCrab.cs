using UnityEngine;

public class CartenCrab : MonoBehaviour
{
    // --- STATIC REFERENCE: Ensures only one crab exists at a time ---
    private static CartenCrab _activeInstance;

    [SerializeField] private float moveSpeed = 2f;

    [Header("Spawn timing")]
    public float spawnDelay = 0f;

    [Header("Interaction Settings")]
    [Tooltip("How many clicks before this crab is sent away.")]
    [SerializeField] private int maxClicks = 3; 
    private int _clickCount = 0;

    [Header("Relocation")]
    public float relocationDelay = 1f;
    public float minDistanceFromHomeBase = 5f;

    private float _spawnTimer;
    private bool _isActive;
    private float _relocationTimer = 0f;

    private Transform homeTransform;
    private Transform targetTransform; 
    private Vector3 currentDirection;
    private bool isOutbound = false;

    void Awake()
    {
        // SINGLETON CHECK: Destroy duplicate crabs
        if (_activeInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        _activeInstance = this;
    }

    void OnDestroy()
    {
        // Clear reference so a new one can spawn later
        if (_activeInstance == this)
        {
            _activeInstance = null;
        }
    }

    void Start()
    {
        _spawnTimer = spawnDelay;
        _isActive = _spawnTimer <= 0f;

        var home = GameObject.FindGameObjectWithTag("HomeBase");
        if (home != null)
        {
            homeTransform = home.transform;
            targetTransform = homeTransform;
            currentDirection = (targetTransform.position - transform.position).normalized;
        }
        else
        {
            currentDirection = Vector3.down;
        }
    }

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

        if (_relocationTimer > 0)
        {
            _relocationTimer -= Time.deltaTime;
            return;
        }

        if (!isOutbound && targetTransform != null)
        {
            currentDirection = (targetTransform.position - transform.position).normalized;
        }

        transform.position += currentDirection * moveSpeed * Time.deltaTime;
    }

    // --- NEW: Universal Click Logic for Melee and Mouse ---
    void OnMouseDown()
    {
        ReceiveClick();
    }

    public void ReceiveClick()
    {
        if (!_isActive || isOutbound) return;

        _clickCount++;
        if (_clickCount >= maxClicks)
        {
            SendOutward(1.5f);
            _clickCount = 0; 
        }
        else
        {
            RelocateInCameraView();
        }
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

    void RelocateInCameraView()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 newPosition;
            int attempts = 0;
            const int maxAttempts = 10;
            do
            {
                float randomX, randomY;
                int edge = Random.Range(0, 4); 
                switch (edge)
                {
                    case 0: randomX = Random.Range(0.1f, 0.9f); randomY = Random.Range(0.8f, 1f); break;
                    case 1: randomX = Random.Range(0.1f, 0.9f); randomY = Random.Range(0f, 0.2f); break;
                    case 2: randomX = Random.Range(0f, 0.2f); randomY = Random.Range(0.1f, 0.9f); break;
                    case 3: randomX = Random.Range(0.8f, 1f); randomY = Random.Range(0.1f, 0.9f); break;
                    default: randomX = 0.5f; randomY = 0.5f; break;
                }

                Vector3 viewportPoint = new Vector3(randomX, randomY, cam.nearClipPlane);
                newPosition = cam.ViewportToWorldPoint(viewportPoint);
                newPosition.z = 0;
                attempts++;
            } while (homeTransform != null && Vector3.Distance(newPosition, homeTransform.position) < minDistanceFromHomeBase && attempts < maxAttempts);

            transform.position = newPosition;

            if (homeTransform != null)
            {
                targetTransform = homeTransform;
                currentDirection = (homeTransform.position - transform.position).normalized;
                isOutbound = false;
            }
        }

        _relocationTimer = relocationDelay;
    }
}