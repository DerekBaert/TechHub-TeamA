using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

    [Header("CurrentState")]
    public bool isActivated = true; // triggered by things, turned to false if destoryOnDeath is false and the item is destroyed
    public int currentHealth = 10; // current health bar
    public int maxHealth = 10; // reset to max health

    [Header("Health Script Settings")]
    public bool destroyOnDeath = false; // do we destroy this when its health runs out vs just deactivate it
    public Transform deathEffectPrefab; // normally i would use a prefab that includes a one shot particle system, audio effect, and a destroy self script

    [Header("Triggered Events")]
    public UnityEvent damagedEvent; // called when you take damage
    public UnityEvent killedEvent; // called when killed

    [Header("Debug Tools")]
    public bool DEBUG_MODE = false;

    // Called by traps when they are hit. Applies damage to the player
    // Pass a negative value to heal instead
    public void TakeDamage(int damageRecieved){
        currentHealth = currentHealth - damageRecieved;
        damagedEvent.Invoke();
       
        // if health is gone
        if ( currentHealth <= 0){
            DeadlyBlow(); // kill the player
        }
    }

    // We hit a trap. Since we are not passing in damage, we just kill the player outright 
    public void DeadlyBlow() {
        if (DEBUG_MODE) {
            Debug.Log(gameObject.name + " was destroyed");
        }

        // Invoke the killed Event
        killedEvent.Invoke();

        DestroySelf();
    }

    // Destroy the gameobject the health is on, optionally trigger a death effect prefab
    private void DestroySelf(){
        if( deathEffectPrefab != null){
            Instantiate(deathEffectPrefab, transform.position, transform.rotation);
        }

        if( destroyOnDeath){
            Destroy(gameObject); // Destroy ourself, the game object
        }else{

            // deactivate the player
            isActivated = false;

            // tries to disable the rigidbody if there is one
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            if (rigid != null) {
               
                rigid.simulated = false;
            }
        }
    }
}
