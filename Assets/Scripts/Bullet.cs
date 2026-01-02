using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLife = 2f;
    public float speed = 5f;

    void Start()
    {
        // Destroy after life expires
        Destroy(gameObject, bulletLife);
    }

    void Update()
    {
        // Simply move forward in the direction the bullet is facing
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}