using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SimplestArtController : MonoBehaviour
{
    // Store Input Here (Independantly from other scripts)
    public Vector2 moveInput;

    // Going to get this from code so make it private so people can't fiddle with it. 
    private SpriteRenderer spriteRenderer;

    // These are the source sprites that will be assigned by the code. 
    // Later we will use the Animator but it is a bit complicated for right now. 
    public Sprite idleSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite topSprite;
    public Sprite bottomSprite;


    // Start is called before the first frame update
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            Debug.LogWarning("No SpriteRenderer Found");
        }
    }

    // Update is called once per frame
    void Update(){
        // Short way to store both axes straight into a Vector2.
        // Basically the same as what we did before with floats though. 
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // First check if we are moving. 
        if (moveInput.magnitude > 0.2f) {
                // If the vector is larger then our threshold value (0.2f) then walk
            // This is checking the absolute value (value ignoring negatives) of X to the 
            // Absolute Value of Y. If it is true, it means the X axis is bigger then the Y
            // Meaning we should use a right or left sprite
            // If Y is bigger we need an Up or Down Sprite. 
            if(Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y)) {
                // If X is positive, walk right. 
                if(moveInput.x > 0) {
                    spriteRenderer.sprite = rightSprite;
                } else {
                    spriteRenderer.sprite = leftSprite;
                }
            } else {
                // Vertical axis is dominant, walking up or down. 
                // Y is Up in Unity
                if(moveInput.y > 0) {
                    spriteRenderer.sprite = topSprite;
                } else {
                    spriteRenderer.sprite = bottomSprite;
                }
            }
        } else {
            // Not really moving so use idle sprite. 
            spriteRenderer.sprite = idleSprite;
        }
    }
}
