using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleRespawner : MonoBehaviour {
    [Header("Item To Spawn")]
    public GameObject prefab;

    [Header("Options")]
    public bool spawnAtStart = true;
    public bool respawnIfDestroyed = true;
    public bool limitedLives = false;
    public int totalLives = 99;

    [Header("Spawned Item Reference")]
    public Transform spawnedItem;

    [Header("Spawn Event")]
    public UnityEvent spawnEvent;
    public UnityEvent doneSpawning;

	// Use this for initialization
	void Start () {
        if( spawnAtStart ){
            SpawnLogic();
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(respawnIfDestroyed){
            if(spawnedItem == null){
                SpawnLogic();
            }
        }
    }

    // Check current lives to see if we should spawn
    private void SpawnLogic(){

        // If lives are limited
        if (limitedLives){

            if (totalLives >= 0) {

                totalLives--;

                SpawnDirectly();

            } else{
                // out of lives
                doneSpawning.Invoke();
            }
        }else{

            // Ignore lives just spawn
            SpawnDirectly();
        }
    }

    // Create a new instance of the given prefab. Don't do any checking just do it
    private void SpawnDirectly(){
        // Create the new object instance and store the transform
        spawnedItem = Instantiate(prefab, transform.position, transform.rotation).transform;

        // Call all scripts listening for the event
        spawnEvent.Invoke();
    }
}
