using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtController_Demo : MonoBehaviour
{
    /*
    1. Basic walk
    2. Dancing - Array of sprites, iterate through array to dance. 


    */


    // Reference to the Sprite Renderer
    public SpriteRenderer spriteRenderer;

    // Reference to the Sprites 
    public Sprite idleSprite;
    public Sprite walkSprite;
    public Sprite jumpSprite;
    public Sprite deadSprite;

    // Input
    public Vector2 input;
    public bool isJumpButtonPressed;

    public Sprite[] danceSequence;
    public bool isDanceSequenceHappening = false;
    public float danceTimer = 5f;
    private float current_danceTimer = 0;
    private int currentDanceFrame = 0;


    public void Update() {
        // Input 
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isJumpButtonPressed = Input.GetButtonDown("Jump");

        // Update our art according to input. 
        // 
        // === X Axis - Walk L/R and idle ===
        if( input.x <= 0.1f && input.x >= -0.1f) {
            // Idle state
            spriteRenderer.sprite = idleSprite; 

        } else if( input.x > 0.1f ){
            // walk right
            spriteRenderer.sprite = walkSprite;
            spriteRenderer.flipX = true;

        } else if( input.x < -0.1f ){
            // walk left
            spriteRenderer.sprite = walkSprite;
            spriteRenderer.flipX = false;

        } else {

            Debug.LogError("Should not be here");
        }

        // === y axis - jump dance and breakdance moves ====
        if (input.y <= 0.1f && input.y >= -0.1f) {
            // Idle state
        //    spriteRenderer.sprite = idleSprite;

        } else if (input.y > 0.1f) {
            // walk right
            spriteRenderer.sprite = jumpSprite;

        } else if (input.y < -0.1f) {
            // walk left
            spriteRenderer.sprite = deadSprite;

        } else {

            Debug.LogError("Should not be here");
        }

        if (isJumpButtonPressed) {
            StartDanceSequence();
        }

        if (isDanceSequenceHappening) {
            spriteRenderer.sprite = danceSequence[currentDanceFrame];

            // Update to the next sprite. 
            currentDanceFrame++;
            currentDanceFrame = currentDanceFrame % danceSequence.Length;
        }


    }

    public void StartDanceSequence() {
        /*
       public Sprite[] danceSequence;
        public bool isDanceSequenceHappening = false;
        public float danceTimer = 5f;
        private float current_danceTimer = 0;
        */
        isDanceSequenceHappening = true;
        current_danceTimer = 0;
        spriteRenderer.sprite = danceSequence[0];
    }

}

