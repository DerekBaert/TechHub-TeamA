using UnityEngine;

public class Hander : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 20f;

    // Bottom cutoff: when object's y <= this value it will be destroyed.
    [SerializeField]
    private float cutOffBottom = -30f;

    // Optional top cutoff if you ever need it
    [SerializeField]
    private float cutOffTop = 25f;

    [Header("HomeBase impact")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private string homeBaseTag = "HomeBase";

    private Transform homeTransform;
    private bool hasDamaged = false;

    // NEW: current travel direction and outbound flag
    private Vector3 currentDirection;
    private bool isOutbound = false;

    void Start()
    {
        var home = GameObject.FindGameObjectWithTag(homeBaseTag);
        if (home != null) homeTransform = home.transform;
        else Debug.LogWarning($"Hander: No object with tag '{homeBaseTag}' found. Falling back to downward movement.");

        // initialize direction toward home if available, otherwise down
        if (homeTransform != null)
            currentDirection = (homeTransform.position - transform.position).normalized;
        else
            currentDirection = Vector3.down;
    }

    void Update()
    {
        // If not outbound and home exists, keep targeting home (handles moving home if it moves)
        if (!isOutbound && homeTransform != null)
        {
            currentDirection = (homeTransform.position - transform.position).normalized;
        }

        transform.position += currentDirection * moveSpeed * Time.deltaTime;

        // existing bottom cutoff cleanup (keep)
        if (transform.position.y <= -30f) // or use your cutOffBottom variable
        {
            Destroy(gameObject);
        }
    }

    // NEW: called by Melee to send this trash outward (away from HomeBase)
    public void SendOutward(float speedMultiplier = 1f)
    {
        if (homeTransform != null)
        {
            currentDirection = (transform.position - homeTransform.position).normalized; // away from home
        }
        else
        {
            // fallback: outward is up
            currentDirection = Vector3.up;
        }

        moveSpeed *= speedMultiplier;
        isOutbound = true;
    }

    // Use trigger collisions to detect contact with HomeBase.
    // Recommended setup:
    // - Trash prefab: Collider2D (Is Trigger = true) + Rigidbody2D (Body Type: Kinematic)
    // - HomeBase: Collider2D (Is Trigger = false) + tag set to "HomeBase"
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDamaged) return;
        if (other.CompareTag(homeBaseTag))
        {
            other.gameObject.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
            hasDamaged = true;
            Destroy(gameObject);
        }
    }

    // In case you use non-trigger colliders, you can also handle collision-based contact:
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasDamaged) return;

        if (collision.gameObject.CompareTag(homeBaseTag))
        {
            collision.gameObject.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
            collision.gameObject.SendMessage("health", damageAmount, SendMessageOptions.DontRequireReceiver);

            hasDamaged = true;
            Destroy(gameObject);
        }
    }
}
