using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [Tooltip("If true, trash will be knocked outward immediately on overlap. If false, left click triggers knock.")]
    public bool autoKnockOnOverlap = false;

    [Tooltip("Multiplier applied to trash moveSpeed when knocked outward.")]
    public float knockSpeedMultiplier = 1.2f;

    // Optional tag filter (set on trash prefabs)
    public string trashTag = "TrashObstacle";

    // track overlapping Hander instances
    private readonly HashSet<Hander> _overlapping = new HashSet<Hander>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(trashTag)) return;

        var h = other.GetComponent<Hander>();
        if (h == null) return;

        _overlapping.Add(h);

        if (autoKnockOnOverlap)
        {
            h.SendOutward(knockSpeedMultiplier);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var h = other.GetComponent<Hander>();
        if (h != null) _overlapping.Remove(h);
    }

    private void Update()
    {
        // left mouse click triggers knock on all overlapping trash
        if (Input.GetMouseButtonDown(0) && _overlapping.Count > 0)
        {
            foreach (var h in _overlapping)
            {
                if (h != null) h.SendOutward(knockSpeedMultiplier);
            }
            _overlapping.Clear();
        }
    }
}
