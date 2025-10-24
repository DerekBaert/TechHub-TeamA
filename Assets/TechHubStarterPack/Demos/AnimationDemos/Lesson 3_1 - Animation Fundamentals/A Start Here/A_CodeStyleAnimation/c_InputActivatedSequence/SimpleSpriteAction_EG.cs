using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: I would never build a game like this, but its useful for understanding what the Animator
// is actually doing, and for understanding why it is so complex. It will make using Animator easier to
// see how it is done manually first. 

// Creating a custom Enumerator to keep track of current state we are in. 
public enum currentState {
    Idle, Action
}

// This will force a SpriteRenderer to be present. 
[RequireComponent(typeof(SpriteRenderer))]
public class SimpleSpriteAction_EG : MonoBehaviour
{
    // Our sprites
    public Sprite[] idleSprites;
    public Sprite[] actionSprites;

    // to render, made this more locked down for this eg. I'll add the reference in Awake
    private SpriteRenderer spriteRenderer;

    [Header("Current State the animation is in")]
    private currentState currentAnimationState; // current state we are in 

    [Header("Settings to play with")]
    public bool interruptable = false; // finish the current sheet before switching if false

    private bool currentInputPressed = false;

    // Timer variables. 
    public int currentFrame = 0;
    public float currentTimer = 0;
    public float frameTime = 0.0666f; // 1/15 or 15 frames per second

    // Awake happens before start and is only run once. 
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Grabs the sprite renderer from the local GameObject
    }

    // Start is called before the first frame update and will be run again when it is enable/disabled. 
    void Start() {
        // error checking
        if (spriteRenderer == null) {
            Debug.LogError("Should never happen because we are forcing it to exist with Requires.. then grabbing it in code. ");
        }

        if (idleSprites.Length == 0 || idleSprites[0] == null) {
            Debug.LogError("Initialize Idle Sprites array");
        }

        if (actionSprites.Length == 0 || actionSprites[0] == null) {
            Debug.LogError("Initialize actionSprites array");
        }

        // start our renderer at idle 0
        spriteRenderer.sprite = idleSprites[0];
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetButton("Jump")) {
            currentInputPressed = true;
        } else {
            currentInputPressed = false; // don't forget to set it back if using GetButton,
        }

        UpdateFrameTimer();
    }

    // Update the timer and go to the next available frame 
    private void UpdateFrameTimer() {
        // update the timer by the frame time
        currentTimer += Time.deltaTime;

        // Check if our timer is complete, and subtract the frametime from it to reset
        if (currentTimer > frameTime) {
            currentTimer -= frameTime;
            NextFrame();
        }

    }

    // Go to the next frame in the array
    public void NextFrame() {
        currentFrame++; // currentFrame++ is the same as currentFrame = currentFrame + 1;

        // Check this first, if the frames are not interruptable, we should check at the end of the animation instead. 
        if (interruptable) {
            // check state to see if we transition. If we are in idle and the button is pressed, transition to Action
            if (currentInputPressed && currentAnimationState == currentState.Idle) {
                currentFrame = 0;
                currentAnimationState = currentState.Action;
                spriteRenderer.sprite = actionSprites[currentFrame]; // start the action sequence
            }

            // Transition from Action to Idle 
            if (!currentInputPressed && currentAnimationState == currentState.Action) {
                currentFrame = 0;
                currentAnimationState = currentState.Idle;
                spriteRenderer.sprite = idleSprites[currentFrame]; // start the action sequence
            }

        }

        // Having two states makes this much more complicated. Imagine having 3+ states to transition between. 

        // If interruptable, we still have to check that we reached the end of the current sprites
        // If not interruptable, this is the only time we can transition to the other set of sprites. 

        // Check if we reached the end of the Idle State, and if we should change to Action
        if (currentAnimationState == currentState.Idle && currentFrame >= idleSprites.Length) {
            currentFrame = 0; // finish & reset
            if (currentInputPressed) {
                currentAnimationState = currentState.Action; // transition to action if button is pressed
            }

        } else if(currentAnimationState == currentState.Action && currentFrame >= actionSprites.Length) {
            currentFrame = 0;
            if (!currentInputPressed) {
                currentAnimationState = currentState.Idle; // transition to Idle if button is Not pressed
            }
        }

        // We've checked all the end/reset conditions, now to just update the current sprite. 
        // This will depend which state we are in. 
        if (currentAnimationState == currentState.Idle) {
            spriteRenderer.sprite = idleSprites[currentFrame];
        } else if( currentAnimationState == currentState.Action){
            spriteRenderer.sprite = actionSprites[currentFrame];
        } else {
            // Should never reach this state, but I like to have the else above because it clarifies what is required. 
            // 
        }
        
    }
}
