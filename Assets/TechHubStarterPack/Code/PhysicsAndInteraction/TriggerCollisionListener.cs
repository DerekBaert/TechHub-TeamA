using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This is a generic script that listens for OnTriggerEvents and triggers unity events
public class TriggerCollisionListener : MonoBehaviour {

    [Header("Collide with objects with this Tag")]
    public bool useTagForCollision = false;
    public string tagToHit = "Player";

    [Header("Goal Event Triggered")]
    public UnityEvent triggeredEvent;

    [Header("Debug Settings")]
    public bool DEBUG_MODE = false;

	public void OnTriggerEnter2D(Collider2D col){
        // get colliding object reference
        // If we are using the tag to filter collisions
        if( useTagForCollision){
            // Check if the object we hit has the correct tag
            if (col.gameObject.CompareTag(tagToHit)) {
                Triggered();

                // DEBUGGING
                if (DEBUG_MODE) { Debug.Log("DEBUG: Goal Triggered by " + gameObject.name + " with a tag " + gameObject.tag);  }
            } else{
                // DEBUGGING
                if (DEBUG_MODE){  Debug.Log("TriggerListener " + gameObject.name + " collided with " + col.gameObject.name + " but its Tag: " + col.gameObject.tag + " did not match the listeners tag : " + tagToHit);  }
            }
        }else{
            // Don't check the tag just trigger it
            Triggered();
        }
    }

    public void Triggered(){

        // call our goal event
        triggeredEvent.Invoke();

        if( DEBUG_MODE) { Debug.Log(gameObject.name + " was triggered!"); } 
    }
}
