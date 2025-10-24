using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookInInputDirection : MonoBehaviour {

    [Header("Rotation Variables")]
    public float currentRotationAngle = 0;
    public float rotationSpeed = 1f;

    [Header("Look Settings")]
    public bool lookDirectionOfInput = true;
    public bool lookDirectionOfHeading = false;

    // References to input vector and the rigidbody
    private Vector2 inputVector;
    private Rigidbody2D _rigid;

    [Header("Debug: Draw Rays")]
    public bool debug_drawDebugRays;

    // Caravans Rotation
    Vector2 currentFacingDirection; // Is this the correct axis?
                                                   // Get Input
    Vector2 inputAiming; // gets input raw
                                       // Velocity Header
    Vector2 currentHeading; // currend heading ( Velocity ) 


    public bool test_lookDirectionOfInput = true;
    public bool test_lookDirectionOfHeading = false;

    // Use this for initialization
    void Start () {
        _rigid = GetComponentInParent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        // Trigonometry is very useful for rotations
        //float radians =  Mathf.Sin(inputVector.y);
        //transform.eulerAngles = new Vector3(0, 0, radians * 180f);

        UpdateInput();

        //UpdateArt();
        UpdateFromCaravans();
        // Other Notes on rotation
        //-------------------------------
        // Eg new vector3(0,0, [0-359]) is set to rotation in degrees
        //transform.eulerAngles =  //       inputVector;
        //-------------------------------
        // Rotation as a Quaternion
        // transform.rotation
        //-------------------------------
        // Rotation by amount per frame
        //transform.Rotate(new Vector3(0, 0,[0 - 359]));
        UpdateDebug();
    }

    public void UpdateInput(){
        currentFacingDirection = transform.up; // Is this the correct axis?
        // Get Input
        inputAiming = inputVector; // gets input raw
        // Velocity Header
        if( _rigid != null)
            currentHeading = _rigid.linearVelocity.normalized; // currend heading ( Velocity ) 
    }

    //********************************
    // Rotates the art to face the direction we want it to
    private void UpdateDebug() {
        
        // get current angle in degrees

        //currentFacingDirection = transform.up; // Is this the correct axis?
        //// Get Input
        //inputAiming = inputVector; // gets input raw
        //// Velocity Header
        //currentHeading = _rigid.velocity.normalized; // currend heading ( Velocity ) 

        if (inputAiming.sqrMagnitude == 0) {
            inputAiming = currentFacingDirection;
        }

        if (currentHeading.sqrMagnitude == 0) {
            currentHeading = currentFacingDirection;
        }

        //Vector3 rightDirection = artTransform.right;

        float V3Signed = Vector3.SignedAngle((Vector3)currentFacingDirection, (Vector3)inputAiming, Vector3.forward);

        // Debug
        if (debug_drawDebugRays) {
            Debug.DrawRay(transform.position, currentFacingDirection * 3f, Color.blue);
            Debug.DrawRay(transform.position, currentHeading * 3f, Color.red);
            Debug.DrawRay(transform.position, inputAiming * 3f, Color.green);
        }


        // [ ] this is coming in backwards, meaning the art has to be rotated 180 . Investigate and figure this out
        float headingDifferenceAngle = Vector2.SignedAngle(currentFacingDirection, currentHeading);
        float inputAimingDifferenceAngle = Vector2.SignedAngle(currentFacingDirection, inputAiming);

        //      Debug.Log("current facing: " + currentFacingDirection + " Heading dif: " + headingDifferenceAngle + " input : " + inputAimingDifferenceAngle);

        if (lookDirectionOfInput) {
            // Rotate the ship art to face the direction we are moving
            currentRotationAngle = Mathf.MoveTowardsAngle(currentRotationAngle, currentRotationAngle + inputAimingDifferenceAngle, Time.deltaTime * rotationSpeed);

        } else if (lookDirectionOfHeading) {
            // Rotate the ship art to face the direction we are moving
            currentRotationAngle = Mathf.MoveTowardsAngle(currentRotationAngle, currentRotationAngle + headingDifferenceAngle, Time.deltaTime * rotationSpeed);

        }

        //transform.rotation = Quaternion.Euler(0, 0, currentRotationAngle);

    }

    // Update Rotation two ways
    public void UpdateFromCaravans(){
        if (test_lookDirectionOfInput) {

            // Get Input
            // Vector2 inputAiming = inputAiming; // gets input raw

            if (inputAiming.sqrMagnitude == 0) {
                inputAiming = currentFacingDirection;
            }

            float inputAimingDifferenceAngle = Vector2.SignedAngle(currentFacingDirection, inputAiming);

            // Rotate the ship art to face the direction we are moving
            currentRotationAngle = Mathf.MoveTowardsAngle(currentRotationAngle, currentRotationAngle + inputAimingDifferenceAngle, Time.deltaTime * rotationSpeed);

            if (debug_drawDebugRays) {
                Debug.DrawRay(transform.position, inputAiming * 3f, Color.green);
            }

        } else if (test_lookDirectionOfHeading) {

            // [ ] this is coming in backwards, meaning the art has to be rotated 180 . Investigate and figure this out
            float headingDifferenceAngle = Vector2.SignedAngle(currentFacingDirection, currentHeading);

            // Rotate the ship art to face the direction we are moving
            currentRotationAngle = Mathf.MoveTowardsAngle(currentRotationAngle, currentRotationAngle + headingDifferenceAngle, Time.deltaTime * rotationSpeed);

        }
        // update the rotation
        transform.rotation = Quaternion.Euler(0, 0, currentRotationAngle);

    }
}
