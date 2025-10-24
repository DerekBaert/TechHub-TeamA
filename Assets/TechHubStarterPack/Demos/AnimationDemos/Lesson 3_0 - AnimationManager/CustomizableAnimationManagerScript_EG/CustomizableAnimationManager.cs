using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script passes data from C# to the Animator. 
// The Customizable AM includes modular options so you can choose what you want to use. 
// Use the Bools to turn modules on and off, they will throw warnings or errors if they
// are not handled in the Animator (only if you call the methods)
[RequireComponent(typeof(Animator))]
public class CustomizableAnimationManager : MonoBehaviour {
    // Needs a reference to the AnimationController/Animator
    [Header("Reference to the Animator")]
    public Animator animator;

    [Header("Rigidbody - Must be set to use Rigidbody mode")]
    public Rigidbody2D rb;
    private Vector2 velocity2D = Vector2.zero;
    private float torque2D = 0f;

    [Header("Custom Input Mode - Call the method to set the variable")]
    private bool customInputForAnim = false;

    [Header("Cached Input - Should be private. I just made them public so you could observe the input. ")]
    public float hInput = 0;
    public float vInput = 0;
    public bool isButtonDownThisFrame = false;
    public bool isButtonHeld = false;

    [Header("Select Animator Controls - You'll have to add the names exactly")]
    public bool useDirectionalInput = false;
    public bool useButtonInput = false;

    // Use the rigidbody to take input directly from the physics calculations. 
    public bool useRigidbodyVelocity = false;
    
    // Create a custom method that can be used for arbitrary inputs. 
    public bool useCustomEventMethodCall = false;

    // ?? I can't think of anything else I would use right now, these are the main ones. 

    // Start is called before the first frame update
    void Start() {
        // grab a reference, if we don't have one already
        if (animator == null) {
            animator = GetComponent<Animator>();
        }

        if (animator == null) {
            Debug.LogError("Expecting an AnimationController but none found. This should theoretically never be called because of the RequireComponent");
        }
    }

    // Update is called once per frame
    void Update() {
        if (useDirectionalInput) {
            UseDirectionalInput();
        }

        if (useButtonInput) {
            UseButtonInput();
        }

        if (useRigidbodyVelocity) {
            UseRigidbodyInput();
        }

        if (useCustomEventMethodCall) {
            UseCustomInput();
        }  
    }

    public void SetCustomInput(bool state) {
        customInputForAnim = state;
    }


    private void UseDirectionalInput() {
        // Get the input
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        // The string must match exactly the String in the Animator. 
        animator.SetFloat("hInput", hInput);
        animator.SetFloat("vInput", vInput);
    }

    private void UseButtonInput() {
        // Button Input - One Press (not as useful with Bools, but good for Triggers)
        if (Input.GetButtonDown("Jump")) {
            animator.SetBool("isButtonDownThisFrame", true); // remember to change this back to false otherwise. 
            animator.SetTrigger("isButtonDown_Trigger"); // Doesn't need the bool as it only turns true. 
        } else {
            animator.SetBool("isButtonDownThisFrame", false);
        }

        if (Input.GetButton("Jump")) {
            animator.SetBool("isButtonHeld", true);
        } else {
            animator.SetBool("isButtonHeld", false);
        }
    }

    private void UseRigidbodyInput() {
        velocity2D = rb.linearVelocity;
        torque2D = rb.totalTorque;

        animator.SetFloat("xVelocity", velocity2D.x);
        animator.SetFloat("yVelocity", velocity2D.y);
        animator.SetFloat("torque", torque2D);

    }

    private void UseCustomInput() {
        animator.SetBool("CustomInput", customInputForAnim);
        if (customInputForAnim) {
            animator.SetTrigger("CustomInputTrigger");
        }
    }
}
