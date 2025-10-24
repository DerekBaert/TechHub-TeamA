using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Flips through a spritesheet using code
// Later you may want to use a Animator but it's good to understand what is happening first. 
public class SimpleSpriteAnim_Eg : MonoBehaviour
{

    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;

    public int currentFrame = 0;

    public float currentTimer = 0;

    [Header("Don't Edit this - The time it takes to play one frame")]
    private float frameTime = 0.0666f; // 1/15 or 15 frames per second

    // Start is called before the first frame update
    void Start()
    {
        // error checking
        if(spriteRenderer == null || sprites.Length == 0 || sprites[0] == null) {
            Debug.LogError("SimpleSpriteAnim_Eg needs to be configured!");
        }

        spriteRenderer.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFrameTimer(); 
    }

    // Update the timer and go to the next available frame 
    private void UpdateFrameTimer() {
        // update the timer by the frame time
        currentTimer += Time.deltaTime;

        // Additional Notes: 
        // A += B is the same as A = A + B
        // Time.deltaTime is the time it took the last frame to render, or 1 / framerate. 
        // If you are running 60 FPS your Time.deltaTime will be 1/60

        // Check if our timer is complete, and subtract the frametime from it to reset
        if (currentTimer > frameTime) {
            currentTimer -= frameTime;
            NextFrame();
        }

        // Note: I prefer this to resetting to 0 as it will be more accurate over time
        // and will avoid *drifting*. This is because the time will always be a little over
        // our limit, so we want to include that remainder in the next step. 

    }

    // Go to the next frame in the array
    public void NextFrame() {
        currentFrame++; // Same as currentFrame = currentFrame + 1;
        
        // Check if we hit the last frame. Note the index of the array starts at 0
        // so index == length is off the end and will be null
        if(currentFrame >= sprites.Length) {
            currentFrame = 0; // reset to beginning if we hit the end of the array.
        }
        spriteRenderer.sprite = sprites[currentFrame];
    }
}
