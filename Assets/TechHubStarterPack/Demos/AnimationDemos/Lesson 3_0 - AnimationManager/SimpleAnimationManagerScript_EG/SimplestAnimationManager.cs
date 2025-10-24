using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimplestAnimationController : MonoBehaviour
{
    // Needs a reference to the AnimationController/Animator
    [Header("Reference to the Animator")]
    public Animator animator;

    [Header("Cached Input - Should be private. I just made them public so you could observe the input. ")]
    public float hInput = 0;
    public float vInput = 0;
    public bool isButtonDownThisFrame = false;
    public bool isButtonHeld = false;

    // Start is called before the first frame update
    void Start()
    {
        // grab a reference, if we don't have one already
        if(animator == null) {
            animator = GetComponent<Animator>();
        }

        if (animator == null) {
            Debug.LogError("Expecting an AnimationController but none found. This should theoretically never be called because of the RequireComponent");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        // The string must match exactly the String in the Animator. 
        animator.SetFloat("hInput", hInput);
        animator.SetFloat("vInput", vInput);

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
}
