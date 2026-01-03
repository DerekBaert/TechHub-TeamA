using UnityEngine;

public class CartenCrab : MonoBehaviour, ISpawnable
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 1.5f;
    [SerializeField] private float sideStepSpeed = 4f;
    [SerializeField] private float stepDuration = 1.2f;

    private float _stepTimer;
    private float _spawnTimer;
    private bool _isActive = false;
    private bool _isOutbound = false;
    private bool _isMovingSideWays = true;
    private int _sideDirection = 1;
    private Vector3 _outboundDir;
    private Transform _home;

    void Start() {
        GameObject homeObj = GameObject.FindGameObjectWithTag("HomeBase");
        if (homeObj != null) _home = homeObj.transform;
        
        _sideDirection = Random.value > 0.5f ? 1 : -1;
        _stepTimer = stepDuration;
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
            HandleMovement();
        } else {
            transform.position += _outboundDir * (sideStepSpeed * 1.5f) * Time.deltaTime;
            CheckOffscreen();
        }
    }

    private void HandleMovement() {
        _stepTimer -= Time.deltaTime;
        if (_stepTimer <= 0) {
            _isMovingSideWays = !_isMovingSideWays;
            _stepTimer = stepDuration;
        }

        Vector3 forward = (_home.position - transform.position).normalized;
        if (_isMovingSideWays) {
            Vector3 side = new Vector3(-forward.y, forward.x, 0);
            transform.position += side * _sideDirection * sideStepSpeed * Time.deltaTime;
        } else {
            transform.position += forward * forwardSpeed * Time.deltaTime;
        }
    }

    public void ReceiveClick() {
        if (!_isActive || _isOutbound) return;
        
        LevelManager.instance?.AddCombo();
        SendOutward(2.5f);
    }

    public void SendOutward(float multiplier) {
    _isOutbound = true;
    
    // Safety check: if _home is missing, use Vector3.up as a backup
    if (_home != null) {
        _outboundDir = (transform.position - _home.position).normalized;
    } else {
        _outboundDir = Vector3.up; 
    }
}

    private void CheckOffscreen() {
        Vector2 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < -0.5f || screenPos.x > 1.5f || screenPos.y < -0.5f || screenPos.y > 1.5f)
            Destroy(gameObject);
    }
}