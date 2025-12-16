using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CrabTrapWorld : MonoBehaviour
{
    [SerializeField] private float attractionRadius = 5f;
    [SerializeField] private int maxTrashCapacity = 5;
    private int currentCount = 0;

    void Update()
    {
        AttractTrash();
    }

    public void SetCapacity(int cap) => maxTrashCapacity = cap;
    public void SetAttractionRadius(float r) => attractionRadius = r;

    private void AttractTrash()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (var c in cols)
        {
            var h = c.GetComponent<Hander>();
            if (h != null)
            {
                h.SendTowardTarget(transform);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var h = other.GetComponent<Hander>();
        if (h != null)
        {
            Destroy(h.gameObject);
            currentCount++;
            if (currentCount >= maxTrashCapacity)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnDestroy()
    {
        // notify UI that one placed trap was removed so another can be placed
        CrabTrap.NotifyPlacedDestroyed();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}