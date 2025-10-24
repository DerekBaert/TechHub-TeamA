using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This player has some basics effects that spawn.
// Topics:
// Playing particle systems and Sounds
// Spawning Instances 
// Timer basics
// Destroying objects gracefully

public class FourthPlayer : MonoBehaviour
{
    // Shared
    public Vector2 inputVector; // 2D Vector, access with inputVector.x/y

    // Kinematic Settings
    public float moveSpeed = 10f; // don't forget the f for floats
    public float rotationSpeed = 4f;

    // Physics Settings
    public float forceAmount = 10f;
    public float torqueAmount = 4f;

    // Eventually we will want to settle on one style and remove the rest. 
    public bool moveUsingKinematic = true;
    public bool moveUsingPhysics = true;
    public bool moveInWorldSpace = true;
    public bool moveInLocalSpace = true;

    public Rigidbody2D rigid;


    // I am going to reference this directly
    // (later on I will show you better ways to handle this! don't worry)
    // [ ] We will put all the art stuff into here so it is separated from the ship control code. 
    // [ ] Move code into effects Component
    public FourthPlayerEffects effects; 

    // Start is called before the first frame update
    void Start(){
        rigid = GetComponent<Rigidbody2D>();
        
        // Nice safe way of doing this. 
        if(effects == null) {
            effects = GetComponentInChildren<FourthPlayerEffects>();
            if(effects == null) { Debug.LogWarning("FourthPlayerEffects on " + gameObject.name + " is null"); }
        }
    }

    // Update is called once per frame
    void Update(){
        if(moveUsingKinematic == moveUsingPhysics){
            Debug.LogWarning("One of moveUsingKinematic or moveUsingPhysics must be true");
        }

        if(moveInLocalSpace == moveInWorldSpace){
            Debug.LogWarning("One of moveInLocalSpace or moveInWorldSpace must be true");
        }

        // Cache Input
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Won't Collide with anything (and stop moving) but can detect collisions. 
        if (moveUsingKinematic){
            if (moveInWorldSpace){
                // Apply movement directly to the Transform. 
                transform.Translate(new Vector3(inputVector.x, 0, 0) * Time.deltaTime * moveSpeed, Space.World); // The 2D Vector will get converted automatically to the 3D Vector the Transform needs.  
                transform.Rotate(new Vector3(0, 0, inputVector.y * rotationSpeed) * Time.deltaTime);
            }else if (moveInLocalSpace) {
                // Apply movement directly to the Transform. 
                transform.Translate(new Vector3(inputVector.x, 0, 0) * Time.deltaTime * moveSpeed); // The 2D Vector will get converted automatically to the 3D Vector the Transform needs.  
                transform.Rotate(new Vector3(0, 0, inputVector.y * rotationSpeed) * Time.deltaTime);

                // Note: Translate defaults to Space.Self or local space
            }
        }

        // ===== Effects =====
        effects.EngineFiring(inputVector.y);
    }

    void FixedUpdate(){
        if (moveUsingPhysics){
            if (moveInWorldSpace) {
                // Add movement using physics. Note we don't need Time.deltaTime
                // Uses World space by default
                rigid.AddForce(new Vector3(0, inputVector.y, 0) * forceAmount);
                rigid.AddTorque(inputVector.x * torqueAmount * -1f);
            } else if (moveInLocalSpace){
                rigid.AddRelativeForce(new Vector3(0, inputVector.y, 0) * forceAmount);
                rigid.AddTorque(inputVector.x * torqueAmount * -1f);
            }
        }
    }   
}
