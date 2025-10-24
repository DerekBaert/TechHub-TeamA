using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * In this Demo we will look at using the AddRelativeForce method and Add Torque method to 
 * make a player character that will move relative to its own orientation, rather then the world orientation. 
 * This is typical of games such as Asteroids, or racing/driving/flying games. 
 */ 
public class RelativeForcePlayerDemo : MonoBehaviour
{
    [SerializeField] // This will allow us to see our private variables, without being able to access them. 
    private float xInput = 0;
    [SerializeField]
    private float yInput = 0;

    // Physics Settings
    public float forceAmount = 10f;
    public float torqueAmount = 4f;

    // Physics References
    // Note: I used Private here as we don't want people assigning this value in the editor, 
    //      We will grab the reference with code in the Start Method
    private Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start() {
        // This is get a reference to the local Rigidbody
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    // Physics Update: Provide Forces in this loop instead. 
    void FixedUpdate() {
        // Movement
        // Add Force to the object in the direction it is facing. Ignore Strafe movement for now
        //              (but adding a value in the X spot would strafe)
        // Create the vector that the AddRelativeForce Method needs
        Vector2 relativeForce = new Vector2 (0, yInput);
        // The Add Relative Force uses the relative orientation of the physics object, so +Y means Forward
        rigid.AddRelativeForce(relativeForce * forceAmount);

        // Rotation
        // Add Torque to rotate the object
        // In 2D, the only axis we can rotate on is Z (think of rotating AROUND that axis)
        // Note the xInput goes from (-1,+1)
        // the -1f multiplier is because by default we will rotate the wrong way. 
        rigid.AddTorque(torqueAmount * xInput * -1f); 
    }
}
