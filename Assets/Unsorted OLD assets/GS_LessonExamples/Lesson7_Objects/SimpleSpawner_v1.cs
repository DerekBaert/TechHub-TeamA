using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawner_v1 : MonoBehaviour
{
    // 1. Reference to the Prefab. In this case, we don't yet need a reference so I can use GameObject

    public GameObject prefabToSpawn;

    public bool spawnOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn an Instance of the prefab at a given location. 
        if (spawnOnStart) {
            // Instantiate(prefabToSpawn);
            // This one spawns at our current transform position, with no rotation. 
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        
        }
    }
}
