using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawner_v2 : MonoBehaviour
{
    // 1. Reference to the Prefab. In this case, we don't yet need a
    // // reference so I can use GameObject
    // Lets add a force to the objects
    // We want to make sure our prefabs contain a Rigidbody2D component
    public Rigidbody2D prefabToSpawn; 
    // now when we add a prefab, it MUST contain a rigidbody. 

    public bool spawnOnStart = true;
    public Vector2 spawnForce = Vector2.down;

    public bool onCooldown = false;
    public float cooldownTimer = .1f;
    private float currentCooldownTimer = 0;

    private bool currentlySpawningMany = false;
    private int amountToSpawnTotal = 0;
    private int currentAmountSpawned = 0;
    public void SpawnMany(int amountToSpawn) {
        amountToSpawnTotal = amountToSpawn;
        currentlySpawningMany = true;
    }

    public void SpawnOne() {
        if (!onCooldown) {
            // Spawn an Instance of the prefab at a given location. 

            // Instantiate(prefabToSpawn);
            // This one spawns at our current transform position, with no rotation. 
            // Can use a Generic tag <T> to specify a type
            // The advantage is that our object reference returned will be a rigidbody. 
            Rigidbody2D newRigidbody = Instantiate<Rigidbody2D>(prefabToSpawn, transform.position, Quaternion.identity);
            newRigidbody.AddForce(spawnForce, ForceMode2D.Impulse);
            // This was returning the reference to type GameObject.
            // Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

            onCooldown = true;
            currentCooldownTimer = 0;
            currentAmountSpawned++; // equivalent to currentAmountSpawned = currentAmountSpawned + 1;
        }
    }

    void Update() {
        if (onCooldown) {
            currentCooldownTimer += Time.deltaTime;
            if (currentCooldownTimer > cooldownTimer) {
                // reset timer
                onCooldown = false;
            }
        }

        if(currentlySpawningMany && currentAmountSpawned < amountToSpawnTotal) {
            SpawnOne();
        }
    }

    void Start() {
        if (spawnOnStart) {
            SpawnOne();
        }
    }
}
