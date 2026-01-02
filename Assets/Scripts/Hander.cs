using UnityEngine;

public class Hander : MonoBehaviour, ISpawnable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private float _initialSpeed;
    private Vector3 currentDirection;
    private bool isOutbound = false;

    [Header("Interaction")]
    [SerializeField] private int maxClicks = 1; 
    private int _currentClicks = 0;
    private bool _isActive = false;
    private float _spawnTimer;

    [Header("Impact")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private string homeBaseTag = "HomeBase";
    private Transform homeTransform;
    private bool hasDamaged = false;

    void Start()
    {
        _initialSpeed = moveSpeed;
        var home = GameObject.FindGameObjectWithTag(homeBaseTag);
        if (home != null) homeTransform = home.transform;

        // Set initial direction toward home
        if (homeTransform != null)
            currentDirection = (homeTransform.position - transform.position).normalized;
        else
            currentDirection = Vector3.down;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        // Handle Spawn Delay
        if (!_isActive)
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f) _isActive = true;
            return;
        }

        // Move the trash
        transform.position += currentDirection * moveSpeed * Time.deltaTime;

        // Rotate slightly to look where it's going (Purely visual "Pro" touch)
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 5f);

        CheckOffscreen();
    }

    public void ReceiveClick()
    {
        if (!_isActive || isOutbound) return;
        _currentClicks++;
        if (LevelManager.instance != null) LevelManager.instance.AddCombo();

        if (_currentClicks >= maxClicks)
        {
            SendOutward(1.5f);
        }
    }

    private void SendOutward(float multiplier)
    {
        if (homeTransform != null)
            currentDirection = (transform.position - homeTransform.position).normalized;
        isOutbound = true;
        moveSpeed *= multiplier;
    }

    public void SetSpawnDelay(float delay)
    {
        _spawnTimer = delay;
        _isActive = delay <= 0;
    }

    private void CheckOffscreen()
    {
        Vector2 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (isOutbound && (screenPos.x < -0.5f || screenPos.x > 1.5f || screenPos.y < -0.5f || screenPos.y > 1.5f))
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDamaged || isOutbound) return;
        if (other.CompareTag(homeBaseTag))
        {
            other.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
            hasDamaged = true;
            Destroy(gameObject);
        }
    }
}