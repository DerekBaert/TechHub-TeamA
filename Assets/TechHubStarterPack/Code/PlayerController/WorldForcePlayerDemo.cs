using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldForcePlayerDemo : MonoBehaviour
{
    // Removed per-axis input fields (mouse-driven)
    private Vector2 _mouseWorldTarget;

    // Physics Settings
    public float forceAmount = 10f; // used as movement speed (units/sec)
    public float slowDownFactor = 0.5f; // Factor to reduce speed
    public float slowDownDuration = 2f; // Duration of slowdown

    // Physics References
    private Rigidbody2D rigid;

    // Add these new fields
    [SerializeField]
    private bool isAlive = true;

    public bool IsAlive => isAlive; // Public property to check player state

    [Header("Auto-facing")]
    [SerializeField] private string homeBaseTag = "HomeBase";
    private Transform homeTransform;
    [Tooltip("Adjust the facing angle. Sprite faces away from HomeBase, flips 180 degrees when counter-clockwise circling starts. 0 = sprite faces right by default.")]
    [SerializeField] private float facingOffsetDeg = 0f;
    [Tooltip("Minimum angle change (degrees) to detect counter-clockwise circling and trigger flip.")]
    [SerializeField] private float angleChangeThreshold = 5f;

    [Header("Screen bounds")]
    [SerializeField] private float horizontalPadding = 0.1f; // world units from edge
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private float _previousMouseAngle = float.MinValue;
    private bool _isCirclingCounterClockwise;
    private bool _wasCirclingCounterClockwise = false;
    private bool _isFlipped = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Add this new method to handle collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TrashObstacle") && isAlive)
        {
            StartCoroutine(SlowDown());
        }
    }

    private IEnumerator SlowDown()
    {
        float originalForceAmount = forceAmount; // Store original speed
        forceAmount *= slowDownFactor; // Reduce speed

        Debug.Log("Player slowed down!");

        yield return new WaitForSeconds(slowDownDuration); // Wait for 2 seconds

        forceAmount = originalForceAmount; // Restore original speed
        Debug.Log("Player speed restored!");
    }
    
    // Start is called before the first frame update
    void Start() 
    {
        // ensure rigid reference
        if (rigid == null) rigid = GetComponent<Rigidbody2D>();

        // Prevent falling:
        rigid.gravityScale = 0f;

        // compute horizontal and vertical world bounds from main camera
        Camera cam = Camera.main;
        if (cam != null)
        {
            float zDist = Mathf.Abs(cam.transform.position.z - transform.position.z);
            Vector3 leftWorld = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, zDist));
            Vector3 rightWorld = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, zDist));
            Vector3 bottomWorld = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, zDist));
            Vector3 topWorld = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, zDist));

            // account for sprite/collider half extents so the player doesn't clip off-screen
            float halfWidth = 0f;
            float halfHeight = 0f;
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                halfWidth = sr.bounds.extents.x;
                halfHeight = sr.bounds.extents.y;
            }
            else
            {
                var col = GetComponent<Collider2D>();
                if (col != null)
                {
                    halfWidth = col.bounds.extents.x;
                    halfHeight = col.bounds.extents.y;
                }
            }

            minX = leftWorld.x + horizontalPadding + halfWidth;
            maxX = rightWorld.x - horizontalPadding - halfWidth;
            minY = bottomWorld.y + horizontalPadding + halfHeight;
            maxY = topWorld.y - horizontalPadding - halfHeight;
        }
        else
        {
            // fallback wide bounds if Camera.main missing
            minX = -100f;
            maxX = 100f;
            minY = -100f;
            maxY = 100f;
        }

        // initialize mouse target at start position
        _mouseWorldTarget = rigid.position;

        // find HomeBase once at start (falls back gracefully)
        var hb = GameObject.FindGameObjectWithTag(homeBaseTag);
        if (hb != null) homeTransform = hb.transform;
    }

    // Update reads mouse and converts to world space
    void Update() 
    {
        var cam = Camera.main;
        if (cam != null)
        {
            Vector3 mouseScreen = Input.mousePosition;
            // Use the player's z distance from camera for proper world conversion
            float zDist = Mathf.Abs(cam.transform.position.z - transform.position.z);
            mouseScreen.z = zDist;
            Vector3 world = cam.ScreenToWorldPoint(mouseScreen);
            _mouseWorldTarget = new Vector2(world.x, world.y);
        }

        // Detect if mouse is circling counter-clockwise around HomeBase
        if (homeTransform != null)
        {
            float currentAngle = Mathf.Atan2(_mouseWorldTarget.y - homeTransform.position.y, _mouseWorldTarget.x - homeTransform.position.x) * Mathf.Rad2Deg;
            if (_previousMouseAngle != float.MinValue)
            {
                float diff = Mathf.DeltaAngle(_previousMouseAngle, currentAngle);
                _isCirclingCounterClockwise = diff < -angleChangeThreshold;
                // Toggle flip on start of counter-clockwise circling
                if (!_wasCirclingCounterClockwise && _isCirclingCounterClockwise)
                {
                    _isFlipped = !_isFlipped;
                }
                _wasCirclingCounterClockwise = _isCirclingCounterClockwise;
            }
            _previousMouseAngle = currentAngle;
        }
    }

        // Physics Update: move toward mouse target
    void FixedUpdate() 
    {
        // compute target directly at mouse position (instantaneous snap)
        Vector2 target = _mouseWorldTarget;

        // clamp position to camera bounds on both axes
        target.x = Mathf.Clamp(target.x, minX, maxX);
        target.y = Mathf.Clamp(target.y, minY, maxY);

        // prevent overlap with HomeBase
        if (homeTransform != null)
        {
            float minDistFromHome = 1f; // adjust distance to prevent overlap (world units)
            Vector2 dirFromHome = (target - (Vector2)homeTransform.position).normalized;
            float distToHome = Vector2.Distance(target, homeTransform.position);

            if (distToHome < minDistFromHome)
            {
                // push player away from HomeBase
                target = (Vector2)homeTransform.position + dirFromHome * minDistFromHome;
            }
        }

        // move using physics (preserves interactions)
        rigid.MovePosition(target);

        // make player face away from or towards HomeBase based on flip state
        // Flips 180 degrees when counter-clockwise circling starts, stays until next flip
        if (homeTransform != null)
        {
            Vector2 dir = _isFlipped ? (Vector2)homeTransform.position - (Vector2)transform.position : (Vector2)transform.position - (Vector2)homeTransform.position;
            if (dir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + facingOffsetDeg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }
}
