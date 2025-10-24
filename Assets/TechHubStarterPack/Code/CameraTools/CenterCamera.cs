using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCamera : MonoBehaviour
{
    [Header("Should the camera follow the target")]
    public bool followTarget = false;
    public Transform target;
    public float lerpSpeed = 1f;
    public Vector3 offset = new Vector3(0, 0, -10); // -10 is default camera offset in z ( it can't be flush with the z plane or you won't see anything

    [Header("Listen to respawer and add new item as target")]
    public bool listenToRespawner = true;
    public SimpleRespawner spawner;

    private void Awake() {
        if(spawner != null){
            spawner.spawnEvent.AddListener(GetSpawnTarget);
            Debug.Log("Event called");
        }
    }

    private void GetSpawnTarget(){
        Debug.Log("Event called2 " + spawner.spawnedItem);
        target = spawner.spawnedItem;
    }

    // Update is called once per frame
    void Update()
    {
        if(followTarget && target != null){

            transform.position = Vector3.Lerp(transform.position, target.position + offset, lerpSpeed * Time.deltaTime) ;
        }
    }
}
