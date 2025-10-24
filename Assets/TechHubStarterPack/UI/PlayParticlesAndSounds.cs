using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Demonstrates how to play particle systems and audio systems from code. 
// Audio
public class PlayParticlesAndSounds : MonoBehaviour
{
    // Particle Systems
    public ParticleSystem[] particleSystems;

    // Audio Source
    // public AudioSource audio; can't name it this, used as a system variable. 
    public AudioSource audioSource; 


    // Check to see if we have references to any particles systems or
    // AudioSources and try to automatically find them up if we don't. 
    // throw a warning if they can't be found. 
    void Start()
    {
        // --- Audio Systems --- 
        // In no audio system is referenced, try to find one in the hierarchy
        // Also check if we have a clip to play. 
        // ( note: I like to check in case I want to reference a different one for some reason). 
        if (audioSource == null || audioSource.clip == null)
        {
            // This will check self, then child objects in the hierarchy to find 
            // the first AudioComponent it encounters. 
            audioSource = GetComponentInChildren<AudioSource>();
        }
        // If it is still not found, throw a warning 
        if (audioSource == null || audioSource.clip == null)
        {
            Debug.LogWarning("No AudioSource found on " + this.gameObject.name);
        }

            // --- Particle Systems ---
            // If no particle systems are referenced, search the hierarchy. 
        if (particleSystems == null || particleSystems.Length < 1)
        {
            // Note the s, this is searching for ALL ParticleSystems on this objects
            // or its children. Returns an array of all objects found. 
            particleSystems = GetComponentsInChildren<ParticleSystem>();
            Debug.Log("Oops had to get this from get component");
        }
        // Last check in case it couldn't find anything. 
        if (particleSystems == null || particleSystems.Length < 1)
        {
            // this refers to the current object instance, gameObject grabs the GameObject it is on, name is the name of the GameObject. 
            // All together will print the name of the GameObject that is throwing this warning. 
            Debug.LogWarning("No Particle Systems Found on " + this.gameObject.name); 
        }


    }

    // Further Reading: https://docs.unity3d.com/ScriptReference/Component.html

    // Play the audio and particle systems. 
    public void Play(){
        // Play the audio
        audioSource.Play();
    

        for (int i = 0; i < particleSystems.Length; i++) {
            particleSystems[i].Play();
        }
    }

    // Stop playing the audio and particle systems. 
    public void Stop(){
        // Play the audio
        audioSource.Stop();

        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
        }
    }
}
