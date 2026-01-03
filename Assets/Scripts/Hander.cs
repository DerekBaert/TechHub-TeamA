using UnityEngine;
using System.Collections;

public class Hander : MonoBehaviour, ISpawnable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 currentDirection;
    private bool isOutbound = false;

    [Header("Interaction")]
    [SerializeField] private int maxClicks = 1; 
    private int _currentClicks = 0;
    private bool _isActive = false;
    private float _spawnTimer;

    [Header("Visual Feedback")]
    private SpriteRenderer _spriteRenderer;
    private bool _isFlickering = false;

    [Header("Impact")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private string homeBaseTag = "HomeBase";
    private Transform homeTransform;
    private bool hasDamaged = false;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Find the HomeBase safely
        GameObject home = GameObject.FindGameObjectWithTag(homeBaseTag);
        if (home != null) 
        {
            homeTransform = home.transform;
            currentDirection = (homeTransform.position - transform.position).normalized;
        }
        else 
        {
            currentDirection = Vector3.down; // Fallback direction
        }
    }

    void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0)) return;

        // 1. Handle Spawn Delay
        if (!_isActive)
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f) _isActive = true;
            return;
        }

        // 2. Movement
        transform.position += currentDirection * moveSpeed * Time.deltaTime;

        // 3. Rotation Look-At
        if (currentDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 5f);
        }

        CheckOffscreen();
    }

    public void ReceiveClick()
    {
        if (!_isActive || isOutbound) return;
        
        _currentClicks++;
        LevelManager.instance?.AddCombo();

        if (_currentClicks >= maxClicks)
        {
            SendOutward(1.5f);
        }
        else
        {
            StartCoroutine(HitFlickerRoutine());
        }
    }

    public void SendOutward(float multiplier)
    {
        if (isOutbound) return; // Don't trigger twice
        
        isOutbound = true;

        // CRITICAL FIX: The Null Check to prevent crashes
        if (homeTransform != null)
            currentDirection = (transform.position - homeTransform.position).normalized;
        else
            currentDirection = Vector3.up;

        moveSpeed *= multiplier;

        if (_spriteRenderer != null) _spriteRenderer.color = Color.white;
    }

    private IEnumerator HitFlickerRoutine()
    {
        if (_isFlickering || _spriteRenderer == null) yield break;
        _isFlickering = true;

        for (int i = 0; i < 2; i++)
        {
            _spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            yield return new WaitForSeconds(0.05f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }
        _isFlickering = false;
    }

    public void SetSpawnDelay(float delay)
    {
        _spawnTimer = delay;
        _isActive = (delay <= 0);
    }

    private void CheckOffscreen()
    {
        if (!isOutbound) return;

        Vector2 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < -0.2f || screenPos.x > 1.2f || screenPos.y < -0.2f || screenPos.y > 1.2f)
        {
            Destroy(gameObject);
        }
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