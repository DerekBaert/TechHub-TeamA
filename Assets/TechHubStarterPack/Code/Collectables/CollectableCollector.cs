using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableCollector : MonoBehaviour {
    // The Type of collectable this collector will collect
    public CollectableType itemType;
    // current collected items
    public int collectedItems = 0;

    [Header("Trigger Event When Amount Collected")]
    public bool triggerEventAtCountReached = false;
    public bool resetWhenCollected = true; // reset the counter to zero
    public int goalNumberToReach = 10; // trigger event when this number are collected
    public UnityEvent triggerWhenGoalReached;

    [Header("Additional Stats")]
    // Total Lifetime Count ( not Reset )
    public int totalLifetimeCount = 0;

    // reset coins
    public void Start() {
        collectedItems = 0;
    }

    // *** Player State Functions ***
    public void CollectItem() {
        collectedItems++; // count the item as collected
        totalLifetimeCount++; // Won't get reset if that is a thing

        // If we have the collecter set to trigger an event at a certain count, trigger it here
        if (triggerEventAtCountReached && collectedItems >= goalNumberToReach ){
            // Call the triggered Event
            triggerWhenGoalReached.Invoke();
            // If the resetWhenCollected variable is set to true, reset the score when the set amount is collected.
            if (resetWhenCollected) {
                collectedItems = 0;
            }
        }
    }

    // remove a coin from the players collection
    public void LoseItem() {
        collectedItems--;
    }
}
