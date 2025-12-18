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

    // remember original UI slot position so template returns after drag/place
    private Vector2 _originalAnchoredPos;

    [Header("Placed world prefab")]
    [Tooltip("Prefab that will be instantiated in world space when placing a trap. Must have CrabTrapWorld, Collider2D (Is Trigger) and visuals.")]
    [SerializeField] private GameObject placedTrapPrefab;

    [Header("Trap mechanics")]
    [SerializeField] private float attractionRadius = 5f;
    [SerializeField] private int maxTrashCapacity = 5;

    [Header("Limits")]
    [Tooltip("How many traps can exist in the world at once.")]
    [SerializeField] private int maxActivePlaced = 2;
    private static int s_activePlacedCount = 0;

    [Header("Cooldown")]
    [SerializeField] private float cooldownDuration = 15f;
    private float _cooldownTimer = 0f;
    private bool _isOnCooldown = false;

    // THIS IS THE LINE THE ERROR IS ASKING FOR:
    public bool IsOnCooldown => _isOnCooldown;

    [Header("Initial global lock (seconds after level load)")]
    [SerializeField] private float initialLockDuration = 15f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (rectTransform != null)
        {
            _originalAnchoredPos = rectTransform.anchoredPosition;
        }

        // REGISTER: Tell the manager this trap exists
        TrapManager.RegisterTrap(this);
    }

    void OnDestroy()
    {
        // UNREGISTER: Cleanup when the trap/scene is destroyed
        TrapManager.UnregisterTrap(this);
    }

    void Update()
    {
        if (_isOnCooldown)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0f)
            {
                _isOnCooldown = false;
                _cooldownTimer = 0f;
            }
        }
    }

    // Static helpers used by world traps to update active count
    public static bool CanPlaceMore() => s_activePlacedCount < InstanceMaxPlaced();
    public static void NotifyPlacedSpawned() { s_activePlacedCount++; }
    public static void NotifyPlacedDestroyed() { s_activePlacedCount = Mathf.Max(0, s_activePlacedCount - 1); }

    // read back max (using a template instance if present) so static CanPlaceMore can check limit
    private static int InstanceMaxPlaced()
    {
        var any = FindObjectOfType<CrabTrap>();
        return any != null ? any.maxActivePlaced : 2;
    }

    private bool IsInitiallyLocked() => Time.timeSinceLevelLoad < initialLockDuration;

    // Click to place (also supports drag placement)
    public void OnPointerClick(PointerEventData eventData)
    {
        TryPlaceAtScreenPosition(eventData.position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isOnCooldown || IsInitiallyLocked()) return;
        if (canvas == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );
        offset = rectTransform.anchoredPosition - localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isOnCooldown || IsInitiallyLocked()) return;
        if (canvas == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );
        rectTransform.anchoredPosition = localPoint + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isOnCooldown || IsInitiallyLocked()) 
        {
            // ensure template returns to original slot if locked/cancelled
            ReturnTemplateToSlot();
            return;
        }

        // place world trap at mouse position when dropping the template
        TryPlaceAtScreenPosition(eventData.position);

        // always return the UI template to its original slot after drag
        ReturnTemplateToSlot();
    }

    private void ReturnTemplateToSlot()
    {
        if (rectTransform != null)
            rectTransform.anchoredPosition = _originalAnchoredPos;
    }

    private bool IsTemplateAndPlaceAllowed()
    {
        if (!isTemplate) return false;
        if (IsInitiallyLocked()) return false;
        if (_isOnCooldown) return false;
        if (s_activePlacedCount >= maxActivePlaced) return false;
        return true;
    }

    private void TryPlaceAtScreenPosition(Vector2 screenPos)
    {
        // guard: ensure template remains in UI and we only spawn a single world prefab per drop
        if (!_isOnCooldown && IsTemplateAndPlaceAllowed())
        {
            if (placedTrapPrefab != null && Camera.main != null)
            {
                // don't allow another placement if limit reached (re-check to avoid race)
                if (s_activePlacedCount >= maxActivePlaced) return;

                float zDist = Mathf.Abs(Camera.main.transform.position.z - 0f);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, zDist));
                worldPos.z = 0f;

                var spawned = Instantiate(placedTrapPrefab, worldPos, Quaternion.identity);

                // configure spawned world trap
                var world = spawned.GetComponent<CrabTrapWorld>();
                if (world != null)
                {
                    world.SetCapacity(maxTrashCapacity);
                    world.SetAttractionRadius(attractionRadius);
                }

                // increment global placed count
                NotifyPlacedSpawned();
            }

            // start template cooldown so player must wait before reusing this UI slot
            _isOnCooldown = true;
            _cooldownTimer = cooldownDuration;
        }
    }
}
