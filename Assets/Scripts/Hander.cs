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

    void Update()
    {
        // Always move down
        Vector3 dir = Vector3.down;
        transform.position += dir * moveSpeed * Time.deltaTime;

        // Destroy when below bottom cutoff
        if (transform.position.y <= cutOffBottom)
        {
            Destroy(gameObject);
        }
    }
}
