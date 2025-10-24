using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trap : MonoBehaviour {

    [Header("Trap Damage")]
    public int damage = 5;

    [Header("Hit Event")]
	public UnityEvent trapReachedEvent;

    [Header("Audio Effects")]
    public AudioSource trapHitSound;

    [Header("Trap Settings")]
    public bool disableOnHit = false;

    [Header("Debug Settings")]
    public bool DEBUG_MODE = false;

    // If the Collider attached to this script has a Collider with the Trigger checkbox checked, 
    // call OnTriggerEnter2D with the other colliders reference
	public void OnTriggerEnter2D(Collider2D col){

        // get the player controller reference from the collider. 
        // We are assuming the PlayerController script is on the same GameObject as the collider
        Health collidedHealth = col.GetComponent<Health>();

        // if we collided with a player
        if (collidedHealth != null){

            // Send a HitTrap method call to the health that collided with it
            collidedHealth.TakeDamage(damage);

            // Check trap settings 
            if( disableOnHit ){
                gameObject.SetActive(false);
            }

            // If we have a trap hit sound, play it
            if(trapHitSound != null){
                trapHitSound.Play();
            }

            // call our goal event
            trapReachedEvent.Invoke();

            // Debugging
            if (DEBUG_MODE) {
                Debug.Log("DEBUG: Trap Collided with " + col.gameObject.name);
            }
        }
	}
}
