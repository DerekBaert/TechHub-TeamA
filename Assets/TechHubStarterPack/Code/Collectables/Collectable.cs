using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Added this to use the UnityEvents 

// Types of things that can be collected
public enum CollectableType{
    Coin,
    Gold,
    Shrimp
}

public class Collectable : MonoBehaviour {

    [Header(" --- Settings --- ")]
    [Header("Collectable Type")]
    public CollectableType itemType;
    [Header("Disable When Collected")]
    [Tooltip("Is this collectable item disabled when it is collected, or can it be collected multiple times")]
    public bool disableWhenCollected = true;

    [Header("Trigger a Unity Event when collected")]
    public UnityEvent coinCollectedEvent; // for adding additional effects

    [Header("Effects")]
    public AudioSource collectedSound;

    [Header("Debug Settings")]
    public bool DEBUG_MODE = false;

	//public void OnCollisionEnter2D(Collision2D col){
	public void OnTriggerEnter2D(Collider2D col){
        // get the player controller reference
        CollectableCollector collector = col.GetComponent<CollectableCollector>();
        // if we collided with a gameobject that has a CollectableCollector component
        if ( collector != null && collector.itemType == itemType ){
            // Debugging
            if(DEBUG_MODE) {  Debug.Log("DEBUG: Collectable Collided with " + col.gameObject.name); }

            // Add one coin to the player counter
            collector.CollectItem();

            // Play Collection Sound
            if(collectedSound != null){
                collectedSound.Play();
            }

            // disable the coin
            if(disableWhenCollected){
                gameObject.SetActive( false );
            }

            // call our goal event, ignore this if you don't have any events
            coinCollectedEvent.Invoke();
        }
	}
}
