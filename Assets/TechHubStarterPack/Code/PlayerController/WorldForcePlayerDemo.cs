using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldForcePlayerDemo : MonoBehaviour
{
    [SerializeField] // This will allow us to see our private variables, without being able to access them. 
    private float xInput = 0;
    [SerializeField]
    private float yInput = 0;

    // Physics Settings
    public float forceAmount = 10f;

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
    void Update() {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    // Physics Update: Provide Forces in this loop instead. 
    void FixedUpdate() {
        // Movement
        // Add Force to the object in the world direction & orientation. 
        // This is useful if you want to have a simple control scheme where the input directly 
        //   relates to the movement in "World Space". This is useful for top down games,
        //   and can be manipulated into other genres (beat em up, fighting, etc)
        
        // In this example, the movement input can be combined into a single Vector
        Vector2 worldForce = new Vector2(xInput, yInput);

        // The AddForce method will apply a force in a World Orientation, which means it will ignore the
        // rigidbody's own facing value. Usually when using this, I will lock the rotation constraint, and
        // handle the art seperately later (to have a sprite face different directions etc)
        rigid.AddForce(worldForce * forceAmount);
    }
}
