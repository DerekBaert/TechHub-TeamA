using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldForcePlayerDemo : MonoBehaviour
{
    private Vector2 _mouseWorldTarget;

    [Header("Physics Settings")]
    public float forceAmount = 10f; 
    public float slowDownFactor = 0.5f; 
    public float slowDownDuration = 2f; 
    private Rigidbody2D rigid;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private bool isAlive = true;
    public bool IsAlive => isAlive;

    [Header("Auto-facing")]
    [SerializeField] private string homeBaseTag = "HomeBase";
    private Transform homeTransform;
    [Tooltip("-90 usually works for sprites facing 'Up'. Adjust if your sprite looks sideways.")]
    [SerializeField] private float facingOffsetDeg = -90f;

    private Vector3 _baseScale; // This will store your 0.13 scale

    [Header("Screen bounds")]
    [SerializeField] private float horizontalPadding = 0.1f;
    private float minX, maxX, minY, maxY;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TrashObstacle") && isAlive)
        {
            StartCoroutine(SlowDown());
        }
    }

    private IEnumerator SlowDown()
    {
        float originalForceAmount = forceAmount;
        forceAmount *= slowDownFactor;
        yield return new WaitForSeconds(slowDownDuration);
        forceAmount = originalForceAmount;
    }

    void Start() 
    {
        // Store the exact scale you set in the Inspector (e.g., 0.13)
    _baseScale = transform.localScale;
        rigid.gravityScale = 0f;
        CalculateBounds();

        _mouseWorldTarget = rigid.position;
        var hb = GameObject.FindGameObjectWithTag(homeBaseTag);
        if (hb != null) homeTransform = hb.transform;
    }

    void CalculateBounds()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float zDist = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 leftWorld = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, zDist));
        Vector3 rightWorld = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, zDist));
        Vector3 bottomWorld = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, zDist));
        Vector3 topWorld = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, zDist));

        float halfWidth = 0f;
        float halfHeight = 0f;
        if (_spriteRenderer != null)
        {
            halfWidth = _spriteRenderer.bounds.extents.x;
            halfHeight = _spriteRenderer.bounds.extents.y;
        }

        minX = leftWorld.x + horizontalPadding + halfWidth;
        maxX = rightWorld.x - horizontalPadding - halfWidth;
        minY = bottomWorld.y + horizontalPadding + halfHeight;
        maxY = topWorld.y - horizontalPadding - halfHeight;
    }

    void Update() 
    {
        if (Time.timeScale == 0) return;

        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 mouseScreen = Input.mousePosition;
            mouseScreen.z = Mathf.Abs(cam.transform.position.z - transform.position.z);
            Vector3 world = cam.ScreenToWorldPoint(mouseScreen);
            _mouseWorldTarget = new Vector2(world.x, world.y);
        }
    }

    void FixedUpdate() 
{
    if (Time.timeScale == 0 || !isAlive) return;

    // 1. Determine Target with Bounds Clamping
    Vector2 target = _mouseWorldTarget;
    target.x = Mathf.Clamp(target.x, minX, maxX);
    target.y = Mathf.Clamp(target.y, minY, maxY);

    // 2. Prevent HomeBase Overlap
    if (homeTransform != null)
    {
        float minDistFromHome = 1f;
        float distToHome = Vector2.Distance(target, homeTransform.position);
        if (distToHome < minDistFromHome)
        {
            Vector2 dirFromHome = (target - (Vector2)homeTransform.position).normalized;
            target = (Vector2)homeTransform.position + dirFromHome * minDistFromHome;
        }
    }

    // 3. Move the Physics Body
    rigid.MovePosition(target);

    // 4. Handle Rotation & Flipping
    Vector2 moveVec = (target - rigid.position);

    if (moveVec.sqrMagnitude > 0.001f)
    {
        float moveAngle = Mathf.Atan2(moveVec.y, moveVec.x) * Mathf.Rad2Deg;

        // Hysteresis Flip (Standard left/right flipping)
        if (moveVec.x < -0.1f) _spriteRenderer.flipY = true;
        else if (moveVec.x > 0.1f) _spriteRenderer.flipY = false;

        // RESET SCALE: We force the scale to stay at your base size (e.g., 0.13)
        // We use _baseScale.y and multiply by -1 if flipped to keep rotation correct
        float yScale = _spriteRenderer.flipY ? -_baseScale.y : _baseScale.y;
        transform.localScale = new Vector3(_baseScale.x, yScale, _baseScale.z);

        // Apply Rotation
        transform.rotation = Quaternion.Euler(0f, 0f, moveAngle + facingOffsetDeg);
    }
}
}