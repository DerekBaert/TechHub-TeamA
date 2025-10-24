using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTriggerListener : MonoBehaviour
{
    // 2D Collider Docs
    // https://docs.unity3d.com/ScriptReference/Collider2D.html

    // Collision Object returned from Collision (passed as parameter)
    // https://docs.unity3d.com/ScriptReference/Collision2D.html

    // Collision Event
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html


    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(this.gameObject.name + " OnCollisionEnter2D event called from collision with " + col.gameObject.name);
    }

    void OnTriggerExit2D(Collider2D collision) {
        
    }

    void OnTriggerStay2D(Collider2D collision) {
        
    }
}
