using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This controls the player
public class PlayerController : MonoBehaviour {

    [Header("*** Input Variables ***")]
    public bool buttonDown = false;
    // set up a vector 2 to hold the input from the player
    public Vector2 inputVector = Vector2.zero;

    [Header("*** Player State ***")]
    public float movementForce = 500f;
    public float boostForce = 1000f;

    // Other Classes on the GameObject
    // The Rigidbody is needed to move items within the physics system
    public Rigidbody2D playerRigidbody;

    [Header(" *** Player State Information ***")]

    // is the player activated or not? 
    public bool autoStart = true;
    public bool isActivated = true;

    // Use this for initialization - 
    // Start is called automatically when the object starts for the first time
    void Start () {
        // grab the physics object so we can move it
         playerRigidbody = GetComponent<Rigidbody2D>();
        if( !isActivated ){
            isActivated = autoStart; // enable the player
        }
         
    }

    // FixedUpdate is called once per Physics step (this happens more then once per frame, usually ~150-200 steps per second)
    // We're using Physics so put it in Fixed Update rather then Update
    void FixedUpdate () {
        if(isActivated){
            // Gets the Jump Button
            buttonDown = Input.GetButton("Jump");
            // you can also use .GetButtonDown("Jump") to only only register true on the single frame it was hit, useful for UI's

            // Gets the input values from the arrow keys (by default)
            inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            // this will return us a value for x and y between -1 and 1
            // you can also do them seperately and handle the x axis and y axis each on thier own float


            // Physics - player
            

            if(buttonDown){
                playerRigidbody.AddForce(inputVector * boostForce);
            }else{
                playerRigidbody.AddForce(inputVector * movementForce);
            }
        }
    }

    // Basic Accessor methods
    // Allows other scripts to get information about this script without making variables public. 
    public bool isActive() {
        return isActivated;
    }
}
