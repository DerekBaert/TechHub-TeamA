using UnityEngine;

public class CigaretEel : MonoBehaviour, ISpawnable
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float waveFrequency = 5f;
    [SerializeField] private float waveAmplitude = 2f;

    private float _aliveTimer;
    private float _spawnTimer;
    private bool _isActive = false;
    private bool _isOutbound = false;
    private Vector3 _direction;
    private Transform _home;
    private SpriteRenderer _spriteRenderer; // Added to fix error

    void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

    void Start() {
        GameObject homeObj = GameObject.FindGameObjectWithTag("HomeBase");
        if (homeObj != null) _home = homeObj.transform;
    }

    public void SetSpawnDelay(float delay) {
        _spawnTimer = delay;
        _isActive = (delay <= 0);
    }

    void Update() {
        if (Time.timeScale == 0) return;

        if (!_isActive) {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0) _isActive = true;
            return;
        }

        if (!_isOutbound) {
            if (_home == null) return;
            _aliveTimer += Time.deltaTime;
            _direction = (_home.position - transform.position).normalized;
            Vector3 perpendicular = new Vector3(-_direction.y, _direction.x, 0);
            Vector3 waveOffset = perpendicular * Mathf.Sin(_aliveTimer * waveFrequency) * waveAmplitude;
            transform.position += (_direction * moveSpeed + waveOffset) * Time.deltaTime;
        } else {
            transform.position += _direction * moveSpeed * Time.deltaTime;
            CheckOffscreen();
        }
    }

    public void ReceiveClick() {
        if (!_isActive || _isOutbound) return;
        LevelManager.instance?.AddCombo();
        SendOutward(2.5f);
    }

    public void SendOutward(float multiplier) {
    _isOutbound = true;
    
    // Safety check: calculate direction ONLY if _home exists
    if (_home != null) {
        _direction = (transform.position - _home.position).normalized;
    } else {
        _direction = Vector3.up;
    }
    
    moveSpeed *= multiplier;
}

    private void CheckOffscreen() {
        Vector2 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < -0.5f || screenPos.x > 1.5f || screenPos.y < -0.5f || screenPos.y > 1.5f)
            Destroy(gameObject);
    }
}