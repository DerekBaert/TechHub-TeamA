using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    enum SpawnerType { Straight, Spin, BackAndForth }
    
    [Header("Bullet Attributes")]
    public GameObject bulletPrefab;
    public float bulletLife = 2f; 
    public float bulletSpeed = 5f;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float firingRate = 0.5f;
    [SerializeField] private float spinSpeed = 100f;

    [Header("Back and Forth Settings")]
    public Transform pos1;
    public Transform pos2;
    public float moveSpeed = 2f;

    private float timer = 0f;
    private bool movingToPos2 = true;

    void Update()
    {
        timer += Time.deltaTime;

        // 1. Handle Spawner Rotation (Spin)
        if (spawnerType == SpawnerType.Spin)
        {
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        }

        // 2. Handle Spawner Movement (Back and Forth)
        if (spawnerType == SpawnerType.BackAndForth && pos1 != null && pos2 != null)
        {
            Transform target = movingToPos2 ? pos2 : pos1;
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < 0.01f)
            {
                movingToPos2 = !movingToPos2;
            }
        }

        // 3. Handle Firing
        if (timer >= firingRate)
        {
            Fire();
            timer = 0;
        }
    }

    void Fire()
    {
        if (bulletPrefab)
        {
            GameObject b = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Bullet bulletScript = b.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.speed = bulletSpeed;
                bulletScript.bulletLife = bulletLife;
            }
        }
    }
}