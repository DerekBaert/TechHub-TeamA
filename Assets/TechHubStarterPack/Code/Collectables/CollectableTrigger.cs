using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableTrigger : MonoBehaviour{

    public CollectableType targetType = CollectableType.Coin;
    public int amountRequired;
    public UnityEvent notEnoughEvent;
    public UnityEvent triggeredEvent;

    public bool raiseOnSuccess = false;
    public int amountToRaise = 4;

    public void RaiseThreshold(){
        amountRequired += amountToRaise;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        CollectableCollector cc = collision.GetComponent<CollectableCollector>();
        if (cc != null){
            if (targetType == cc.itemType && cc.collectedItems >= amountRequired){
                triggeredEvent.Invoke();
            }else{
                notEnoughEvent.Invoke();
            }
        }
    }

}
