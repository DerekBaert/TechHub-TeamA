using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawner_wArray : MonoBehaviour
{
    // 1. Reference to the Prefab. In this case, we don't yet need a reference so I can use GameObject

    public GameObject[] arrayOfObjects = new GameObject[10]; 

    public GameObject prefabToSpawn;

    public bool spawnOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn an Instance of the prefab at a given location. 
        if (spawnOnStart) {
            // Instantiate(prefabToSpawn);
            // This one spawns at our current transform position, with no rotation. 
            /*
            int i = 0;
            while(i < arrayOfObjects.Length) {
                GameObject go = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
                arrayOfObjects[i] = go;
                Destroy(go, Random.Range(1f, 5f));
                i++; // i = i + 1;
            }
            */
            for(int j = 0; j < arrayOfObjects.Length; j++) {
                GameObject go = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
                go.name = go.name + ": (" + j + ")";

                arrayOfObjects[j] = go;
                Destroy(go, Random.Range(1f, 5f));
            }


  //          Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            
     //       arrayOfObjects[0] = go;

//  rb.AddForce(new Vector3(1, 2, 3));
        }
    }
}
