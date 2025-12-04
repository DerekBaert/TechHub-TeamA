using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{


    public float bulletLife = 1f; // How long it lives
    public float rocation = 0f;  // Way its going ya you get it...
    public float speed = 1f; // How fast it goes ZOOOOOOOOOOOOOOOOM
    
    private Vector2 spawnPoint; //where it spawns i think...
    private  float timer = 0f; // How long it last
    void Start()
    {
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
    }
    // Update is called once per frame
    void Update()
    {
        if (timer > bulletLife) Destroy(gameObject);
        timer += Time.deltaTime;
        transform.position = Movement(timer);
    }

    public Vector2 Movement(float t)
    {
        float x = timer * speed * transform.right.x;
        float y = timer * speed * transform.right.y;
        return new Vector2(x + spawnPoint.x, y + spawnPoint.y);

 }

}

        
    