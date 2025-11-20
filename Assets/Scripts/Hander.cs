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
    private float cutOffTop = 30f;

    [Header("HomeBase impact")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private string homeBaseTag = "HomeBase";

    private Transform homeTransform;
    private bool hasDamaged = false;

    void Start()
    {
        var home = GameObject.FindGameObjectWithTag(homeBaseTag);
        if (home != null) homeTransform = home.transform;
        else Debug.LogWarning($"Hander: No object with tag '{homeBaseTag}' found. Falling back to downward movement.");
    }

    void Update()
    {
        // Move toward HomeBase if found, otherwise move straight down
        if (homeTransform != null)
        {
            Vector3 dir = (homeTransform.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }

        // Failsafe: destroy if it gets too far below the scene
        if (transform.position.y <= cutOffBottom)
        {
            Destroy(gameObject);
        }
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
            // Try to notify HomeBase to take damage. Use SendMessage as a tolerant fallback.
            other.gameObject.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
            other.gameObject.SendMessage("health", damageAmount, SendMessageOptions.DontRequireReceiver);

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
