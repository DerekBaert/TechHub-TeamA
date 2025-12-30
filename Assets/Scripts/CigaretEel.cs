using UnityEngine;
using System.Collections;

public class CigaretEel : MonoBehaviour, ISpawnable
{
    [SerializeField] private float moveSpeed = 10f;

    [Header("Interaction Settings")]
    [Tooltip("How many clicks before this Eel splits. Default is 1.")]
    [SerializeField] private int maxClicks = 1; 
    private int _currentClicks = 0;

    [Header("Spawn & Split Timing")]
    public float spawnDelay = 0f; 
    public float splitDelay = 0.5f;

    [Header("Split Physics")]
    [Tooltip("How far back (away from HomeBase) the clones spawn.")]
    [SerializeField] private float knockbackDistance = 1.5f;
    [SerializeField] private float minScaleToSplit = 0.25f;
    [SerializeField] private float splitOffset = 0.5f;

    private float _timer;
    private bool _isActive;
    private bool _isSplitting; 

    private bool isOutbound = false; 
    private Vector3 _currentDirection;
    private Transform _homeBase;

    void Start()
    {
        _timer = spawnDelay;
        _isActive = _timer <= 0f;

        GameObject home = GameObject.FindGameObjectWithTag("HomeBase");
        if (home != null)
        {
            _homeBase = home.transform;
            UpdateDirection();
        }
    }

    // Public API for Spawner
    public void SetSpawnDelay(float delay)
    {
        spawnDelay = delay;
        _timer = delay;
        _isActive = _timer <= 0f;
    }

    void Update()
    {
        // Stop all logic if time is frozen (Paused) or game is over
        // 1. THIS IS THE MOST IMPORTANT LINE:
        if (Mathf.Approximately(Time.timeScale, 0)) return;
    
        if (LevelManager.instance != null && LevelManager.instance.isGameOver) return;
        // Handle Initial Spawn Delay
        if (!_isActive)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) _isActive = true;
            else return;
        }

        // Handle Split Pause
        if (_isSplitting) return;

        // Movement Logic: Only update direction if NOT knocked away (isOutbound)
        if (!isOutbound && _homeBase != null)
        {
            UpdateDirection();
        }
        
        transform.position += _currentDirection * moveSpeed * Time.deltaTime;
    }

    void UpdateDirection()
    {
        if (_homeBase != null)
            _currentDirection = (_homeBase.position - transform.position).normalized;
    }

    // --- FIX: Logic for Melee and Manual Clicking ---
    private void OnMouseDown()
    {
        ReceiveClick();
    }

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

    private void Split()
    {
        if (transform.localScale.x <= minScaleToSplit)
        {
            Destroy(gameObject);
            return;
        }

        // Calculate "Backwards" direction (Away from HomeBase)
        Vector3 awayDirection = Vector3.up; 
        if (_homeBase != null)
        {
            awayDirection = (transform.position - _homeBase.position).normalized;
        }

        for (int i = 0; i < 2; i++)
        {
            // Spawn position: Nudge back + Side offset
            float sideOffset = (i == 0) ? -splitOffset : splitOffset;
            Vector3 spawnPosition = transform.position + (awayDirection * knockbackDistance);
            
            // Side-by-side positioning
            Vector3 perpendicular = new Vector3(-awayDirection.y, awayDirection.x, 0);
            spawnPosition += perpendicular * sideOffset;

            GameObject clone = Instantiate(gameObject, spawnPosition, transform.rotation);
            clone.transform.localScale = transform.localScale * 0.5f;

            // Initialize the clone
            CigaretEel eelScript = clone.GetComponent<CigaretEel>();
            if (eelScript != null)
            {
                eelScript.moveSpeed = this.moveSpeed * 1.2f;
                eelScript.isOutbound = false; // Reset so they head back to HomeBase
                eelScript.StartSplitPause();
            }
        }

        Destroy(gameObject);
    }

    public void StartSplitPause()
    {
        _isActive = true; 
        StartCoroutine(SplitPauseRoutine());
    }

    private IEnumerator SplitPauseRoutine()
    {
        _isSplitting = true;
        yield return new WaitForSeconds(splitDelay);
        _isSplitting = false;
    }

    // Required for the Melee script to knock it away if needed
    public void SendOutward(float speedMultiplier = 1f)
    {
        if (_homeBase != null)
            _currentDirection = (transform.position - _homeBase.position).normalized;
        else
            _currentDirection = Vector3.up;

        moveSpeed *= speedMultiplier;
        isOutbound = true; 
    }
}