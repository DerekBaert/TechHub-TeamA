using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffectsListener : MonoBehaviour
{

    // Play Audio when crashing
    public AudioSource playCrash;

    // play smoke effect. 
    // Could do this using a. particles b. sprite prefabs that get spawned or c. sprite I just enable and disable. 
    // I will do c first as it is simplest. 
    public GameObject impactSprite ;
    public float timerToDisplayImpact = .1f;
    private float currentTimerToDisplayImpact;
    private bool isDisplayingImpact = false;

    private void OnCollisionEnter2D(Collision2D collision) {
       
        // Play audio
        playCrash.Play();
        // Start displaying animation. 
        StartImpactAnimation();
    }

    public void Update() {
        // Check if animation is currently playing. 
        if (isDisplayingImpact) {
            // Update timer by time.deltaTime (time last frame took to render)
            currentTimerToDisplayImpact += Time.deltaTime;
            // Check timer end condition
            if (currentTimerToDisplayImpact > timerToDisplayImpact) {
                StopImpactAnimation();
            }
        }
    }

    private void StartImpactAnimation() {
        // Start animation if it is not already playing. 
        if (!isDisplayingImpact) {
            isDisplayingImpact = true;
            currentTimerToDisplayImpact = 0;
            impactSprite.SetActive(true);
        }
    }

    private void StopImpactAnimation() {
        isDisplayingImpact =  false;
        impactSprite.SetActive(false);
    }

}
