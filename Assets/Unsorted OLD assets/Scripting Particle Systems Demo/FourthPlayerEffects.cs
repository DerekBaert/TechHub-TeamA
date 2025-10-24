using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthPlayerEffects : MonoBehaviour
{


    // ===== Effects =====
    // [ ] Move these into separate class after
    public ParticleSystem engineSmoke;
    // https://docs.unity3d.com/ScriptReference/ParticleSystem.html
    // https://docs.unity3d.com/ScriptReference/ParticleSystem-emission.html
    private ParticleSystem.EmissionModule smokeEmissionModule; // how we access particle system settings. 
    
    public AudioSource engineSound;


    // Start is called before the first frame update
    // Awake is called before Start, so this needs to be Awake so that this script will have the reference when 
    // other scripts call it. 
    void Awake()
    {
        // Grab the components to avoid the errors
        engineSmoke = GetComponent<ParticleSystem>();
        engineSound = GetComponent<AudioSource>();


        // ===== Effects =====
        // Cache the emission system
        smokeEmissionModule = engineSmoke.emission;
        // Start the particlesystem overall
        engineSmoke.Play();
    }

    // Smoke Trail
    // The simplest effect we can do is a smoke trail
    // we will add some audio as well. 
    // We don't have collision detection yet so we can't use that
    // but we will know when the throttle is held down and be able to spawn smoke then. 
    // Also it will need an increment timer. 
    
    // This will fire the engine. We are also passing in the amount of thrust we will need. 
    // I am stating it is normalized so the value will be simply between 0 and 1. 
    // Right now this will get called every frame and filter when it should fire by itself. 

    public void EngineFiring(float normalizedThrustAmount) {

        // Need an interface for this. 
        if (normalizedThrustAmount > .1f) {
        // Simplest 
        // This works! But why?
        // https://docs.unity3d.com/ScriptReference/ParticleSystem.Play.html

        // 1. If the Particle System has stopped, then the system starts from time 0
        //  If the Particle System is already playing, the system continues to play and this function has no effect.
        // Meaning this is not actually needed either
        //  if (!engineSmoke.isPlaying) {
        //  }
        // this really just starts the system up from frame zero. 
        // Adding stop STOPPED the whole particle system, not just the emitting. 

        // see accessing Module Properties
            //https://docs.unity3d.com/ScriptReference/ParticleSystem.html 
            smokeEmissionModule.enabled = true;

            engineSound.Play();
            Debug.Log("Playing effects");
        } else {
            smokeEmissionModule.enabled = false;
        }


    }
    //  - Note: We don't need Update because this will be called from the Players Update loop. 
}
// Why doesn't this work? 
/*
 * https://stackoverflow.com/questions/18292087/accessing-and-changing-structs-as-property-vs-as-field
Section "1.7 Structs" of the C# language specification 5.0 says:

With classes, it is possible for two variables to reference the same object and thus possible for operations on one variable to affect the object referenced by the other variable. With structs, the variables each have their own copy of the data, and it is not possible for operations on one to affect the other.

That explains that you will receive a copy of the struct and not be able to modify the original struct. However, it doesn't describe why it isn't allowed.

Section "11.3.3" of the specifcation:

When a property or indexer of a struct is the target of an assignment, the instance expression associated with the property or indexer access must be classified as a variable. If the instance expression is classified as a value, a compile-time error occurs. This is described in further detail in §7.17.1.

So the returned "thing" from the get accessor is a value and not a variable. That explains the wording in the error message.
*/

/*
 * Error Log: 
 * Got this: NullReferenceException: Do not create your own module instances, get them from a ParticleSystem instance
 * which threw me off for a while, but it was just because the reference to the particle system was null. 
   // ===== Effects =====
        // Cache the emission system
 //       smokeEmissionModule = engineSmoke.emission;
        // Start the particlesystem overall
        engineSmoke.Play();
// This will cause it 
 * 
 */