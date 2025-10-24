using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple utility script that just destroys the current gameobject
public class SelfDestruct : MonoBehaviour {

    public bool autoDestructOnStart = false;
    public float timer;

	// Use this for initialization
	void Start () {
        if( autoDestructOnStart){
            Destroy(gameObject, timer);
        }
    }

    // Call this to destroy self from script or events
    public void Destruct(float newTimer){
        Destroy(gameObject, newTimer);
    
    }

}
