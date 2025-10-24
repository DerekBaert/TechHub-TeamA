using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://docs.unity3d.com/Manual/script-AnimationWindowEvent.html
public class AnimEventTester_CallMethods : MonoBehaviour
{
    public float num = 0;


    public void CallMe() {
        Debug.Log("CallMe Was Called");
    }

    /*
     * Use an Animation Event to call a function at a specific point in time. 
     * This function can be in any script attached to the GameObject but must 
     * only accept a single parameter of type float, int, string, an object 
     * reference, or an AnimationEvent object.

    // From Docs
    */
    public void NeedAParam(float inputFromAnim) {
        Debug.Log("NeedAParam: " +  inputFromAnim);
        // Printed from the animaton clip
    }
}
