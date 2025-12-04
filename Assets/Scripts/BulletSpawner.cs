using JetBrains.Annotations;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    enum SpawnerType { Straight, Spin, Backtfourth }
    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 1f; 
    public float speed = 1f;
    public Transform pos1, pos2; //This Is for the BackTForth Spawner
    public Transform StartPos;
  

    Vector3 nextpos;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float FiringRate = 1f;

    public GameObject spawnedBullet;
    private float timer = 0f;

    void Start()
    {
         nextpos = StartPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; 
        if (spawnerType == SpawnerType.Spin) transform.eulerAngles = new Vector3 (0f, 0f, transform.eulerAngles.z+1f);
        if (timer >= FiringRate)
        {
            timer = 0f;
            Fire();

        }
        
        timer += Time.deltaTime;
        if (spawnerType == SpawnerType.Backtfourth);
        if (timer >= FiringRate)
           if (transform.position== pos1.position)
            {
                nextpos = pos2.position;
            }
        if (transform.position == pos2.position)
        {
            nextpos = pos1.position;
        }
       
        {
            timer = 0f;
            Fire();

        }
    }
    private void Fire() {

        if (bullet)
        {
            spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            spawnedBullet.GetComponent<Bullet>().speed = speed;
            spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            spawnedBullet.transform.rotation = transform.rotation;
        }
    }
    }
    