using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum InputMode {
    UseInputForAnimation, UseVelocityForAnimation, UseBothBlended
}

public class ArtController_4DirDemo : MonoBehaviour {
     // This will allow us to see our private variables, without being able to access them. 
    private Vector2 moveInput;
    private Vector2 cachedVelocity;

    private Rigidbody2D rigid;
    private Animator anim; // Is Expecting and animator with floats for input direction. 

    public InputMode inputToUseForAnimations = InputMode.UseInputForAnimation;

    private void Start() {
        rigid = GetComponentInParent<Rigidbody2D>();
        if (rigid == null) {
            Debug.LogWarning("Rigidbody Not Found in Parent GameObject");
        }

        anim = GetComponent<Animator>();
        if (anim == null) {
            Debug.LogWarning("Animator Not Found in current GameObject");
        }
    }

    // Update is called once per frame
    void Update() {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        cachedVelocity = rigid.linearVelocity;

        UpdateAnimation();
    }
        
    private void UpdateAnimation() {
        if (inputToUseForAnimations == InputMode.UseInputForAnimation) {
            anim.SetFloat("xMoveInput", moveInput.x);
            anim.SetFloat("yMoveInput", moveInput.y);
            Debug.DrawRay(transform.position, moveInput * 5f, Color.green);

            if (moveInput.magnitude > 0.1f) {
                anim.SetBool("isMoving", true);
            } else {
                anim.SetBool("isMoving", false);
            }

        } else if (inputToUseForAnimations != InputMode.UseVelocityForAnimation) {

            Vector2 normalizedVelocity = cachedVelocity.normalized;
            anim.SetFloat("xMoveInput", cachedVelocity.x);
            anim.SetFloat("yMoveInput", cachedVelocity.y);
            Debug.DrawRay(transform.position, normalizedVelocity * 5f, Color.blue);

            if (normalizedVelocity.magnitude > 0.1f) {
                anim.SetBool("isMoving", true);
            } else {
                anim.SetBool("isMoving", false);
            }

        } else if (inputToUseForAnimations != InputMode.UseBothBlended) {
            Vector2 normalizedVelocity = cachedVelocity.normalized;
            Vector2 simpleBlended = (normalizedVelocity + moveInput) / 2f;

            Debug.DrawRay(transform.position, moveInput * 5f, Color.green);
            Debug.DrawRay(transform.position, normalizedVelocity * 5f, Color.blue);
            Debug.DrawRay(transform.position, simpleBlended * 5f, Color.red);

            anim.SetFloat("xMoveInput", simpleBlended.x);
            anim.SetFloat("yMoveInput", simpleBlended.y);

            if (simpleBlended.magnitude > 0.1f) {
                anim.SetBool("isMoving", true);
            } else {
                anim.SetBool("isMoving", false);
            }

        } else {
            Debug.LogError("Shouldn't be able to get here, something has gone wrong.");
        }
    }
}
