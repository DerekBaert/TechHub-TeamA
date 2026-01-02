using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CrabTrap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("UI / Template")]
    [SerializeField] private bool isTemplate = true;
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector2 offset;
    private Vector2 _originalAnchoredPos;

    [Header("Placed world prefab")]
    [SerializeField] private GameObject placedTrapPrefab;

    [Header("Trap mechanics")]
    [SerializeField] private float attractionRadius = 5f;
    [SerializeField] private int maxTrashCapacity = 5;

    [Header("Limits")]
    [SerializeField] private int maxActivePlaced = 2;
    private static int s_activePlacedCount = 0;

    [Header("Charges System")]
    [SerializeField] private int maxCharges = 2;
    private int _currentCharges = 0; 
    [SerializeField] private float restockDuration = 15f;
    private float _restockTimer = 0f;

    [Header("Initial global lock")]
    [SerializeField] private float initialLockDuration = 15f;

    public bool IsOnCooldown => _currentCharges <= 0;
    public int CurrentCharges => _currentCharges;

    public float GetRestockTimeRemaining()
{
    if (_currentCharges >= maxCharges) return 0f;
    return restockDuration - _restockTimer;
}

void Start() 
{
    // Reset static count on level start to prevent "ghost" traps from previous runs
    s_activePlacedCount = 0;
}

    private CanvasGroup canvasGroup;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (rectTransform != null) _originalAnchoredPos = rectTransform.anchoredPosition;
        TrapManager.RegisterTrap(this);
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void OnDestroy() => TrapManager.UnregisterTrap(this);

    void Update()
    {
        // 1. If we are in the initial lock period, keep charges at 0
        if (Time.timeSinceLevelLoad < initialLockDuration)
        {
            _currentCharges = 0;
            return;
        }

        // 2. Just finished initial lock? Fill charges immediately
        if (_currentCharges == 0 && _restockTimer == 0 && Time.timeSinceLevelLoad >= initialLockDuration)
        {
            _currentCharges = maxCharges;
        }

        // 3. Handle Restocking
        if (_currentCharges < maxCharges)
        {
            _restockTimer += Time.deltaTime;
            if (_restockTimer >= restockDuration)
            {
                _currentCharges++;
                _restockTimer = 0f;
            }
        }
    }

    // Static helpers
    public static bool CanPlaceMore() => s_activePlacedCount < InstanceMaxPlaced();
    public static void NotifyPlacedSpawned() { s_activePlacedCount++; }
    public static void NotifyPlacedDestroyed() { s_activePlacedCount = Mathf.Max(0, s_activePlacedCount - 1); }

    private static int InstanceMaxPlaced()
    {
        var any = FindObjectOfType<CrabTrap>();
        return any != null ? any.maxActivePlaced : 2;
    }

    private bool IsInitiallyLocked() => Time.timeSinceLevelLoad < initialLockDuration;

    public void OnPointerClick(PointerEventData eventData) => TryPlaceAtScreenPosition(eventData.position);

    public void OnBeginDrag(PointerEventData eventData)
{
    if (IsOnCooldown || IsInitiallyLocked() || canvas == null) return;
    
    // NEW: Allow the mouse to "see through" this icon while dragging
    canvasGroup.blocksRaycasts = false; 
    canvasGroup.alpha = 0.6f; // Optional: make it slightly transparent

    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
    offset = rectTransform.anchoredPosition - localPoint;
}

    public void OnDrag(PointerEventData eventData)
    {
        if (IsOnCooldown || IsInitiallyLocked() || canvas == null) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        rectTransform.anchoredPosition = localPoint + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
{
    // NEW: Turn raycasts back on so we can click it again later
    canvasGroup.blocksRaycasts = true;
    canvasGroup.alpha = 1.0f;

    if (IsOnCooldown || IsInitiallyLocked()) { ReturnTemplateToSlot(); return; }
    
    TryPlaceAtScreenPosition(eventData.position);
    ReturnTemplateToSlot();
}

    private void ReturnTemplateToSlot() { if (rectTransform != null) rectTransform.anchoredPosition = _originalAnchoredPos; }

    private bool IsTemplateAndPlaceAllowed()
{
    // Remove the s_activePlacedCount check from here
    if (!isTemplate || IsInitiallyLocked() || _currentCharges <= 0) return false;
    return true;
}

    private void TryPlaceAtScreenPosition(Vector2 screenPos)
{
    // If the static count got stuck, this is a safety reset if you have no traps in scene
    if (FindObjectsOfType<CrabTrapWorld>().Length == 0) s_activePlacedCount = 0;

    if (IsTemplateAndPlaceAllowed())
    {
        if (s_activePlacedCount >= maxActivePlaced) 
        {
            Debug.LogWarning($"LIMIT: {s_activePlacedCount}/{maxActivePlaced}. Cannot place more.");
            return; 
        }

        if (placedTrapPrefab != null && Camera.main != null)
        {
            float zDist = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, zDist));
            worldPos.z = 0f;

            GameObject spawned = Instantiate(placedTrapPrefab, worldPos, Quaternion.identity);
            
            // Explicitly verify the spawn happened
            if (spawned != null)
            {
                NotifyPlacedSpawned(); 
                _currentCharges--; 
                Debug.Log("Second Trap Placed Successfully!");
            }
        }
    }
}
}