using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTimer_wEvent : MonoBehaviour {
    public UnityEvent OnTimerCompletedEvent = new UnityEvent();

    public float maxDelayTime = 1f;
    private float currentTimer = 0f;

    public bool hasStarted = false;

    public bool repeatOnCompletion = false;

    // Update is called once per frame
    void Update() {
        if (hasStarted) {
            // add the current frame time to our current timer value. 
            currentTimer += Time.deltaTime;

            // If our timer is complete. 
            if (currentTimer > maxDelayTime) {
                // Completed our timer; 
                // Call our Event: 
                OnTimerCompletedEvent.Invoke();

                // Handle what comes next, depending if it loops or not. 
                if (repeatOnCompletion) {
                    currentTimer -= maxDelayTime; // This helps keep it accurate when the value is going over our limit. 
                } else {
                    // Disable and reset
                    hasStarted = false;
                    currentTimer = 0f;
                }
            }
        }
    }

    // Provide some helpful methods since we are probably activating this from other Events: 
    // Starts the timer. Does nothing if it's already running. 
    public void StartTimer() {
        hasStarted = true;
    }

    // Stops and resets state. 
    public void StopTimer() {
        hasStarted = false;
        currentTimer = 0f;
    }

    // Stops timer but leaves current time intact
    public void PauseTimer() {
        hasStarted = false;
    }

    public void SetMaxTimerValue(float value) {
        maxDelayTime = value;
    }

    public void SetCurrentTimerTime(float value) {
        currentTimer = value;
    }

    public void ToggleRepeatingMode(bool value) { 
        repeatOnCompletion = value; 
    }
}
