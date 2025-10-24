// Dylans Unity Cheat Sheet V2 - March 9th

using UnityEngine; // Defaults
using System.Collections; // Defaults
using System.Collections.Generic; // This Is For Lists

// Basic Class
public class CheatSheet_v2 : MonoBehaviour {

    // Variable types
    public bool isMoving = false;
	public bool isKeyDown = false;  // Logical Operator
	public int currentTurnNumber = 0; // Basic Integers
	public float floatPointValue = 1.314f; // don't forget the f!
	public int[] arrayOfInts; // an array can hold many of an item
	public List<int> listOfInts = new List<int>(); // Generic List (like ArrayList in Java). 
	public List<string> whatHappenedThisTurnLog = new List<string>();

	public bool printAdditionalMessage = true;
	public string message = "So Far So Good!";

    // It can be useful to have these cached. 
    public GameObject cachedGameObject;
    public Transform cachedTransform;

	// Physics
	public Rigidbody2D rigid;

	// Effects - 
	public AudioSource sfx; // Audio
	public SpriteRenderer art; // 2D art
	public ParticleSystem particles; // Particles. 	

	// Item To Spawn
	public GameObject itemPrefab; 

	void SampleFunction(){
		
		// Debugging
		Debug.Log(message);
		Debug.DrawLine(Vector3.zero, Vector3.up, Color.red); // Draw a line from one V3 position to another V3 position (Only in the scene view)
        Debug.DrawRay(new Vector3(0, 0, 0), new Vector3(0, 1, 0), Color.blue); // Draws a line from position zero in the direction (and distance) V3(0,1,0) (up one unit). 
        // Note the input for these two are equivalent but the outcome is very different. 


		
        // Basic Movement
        Vector3 moveVector = new Vector3(0, 1f, 0); // define the vector to move on. 
        transform.Translate(moveVector); // moves without physics. 1 unit (~1m) towards the Y axis
        transform.Rotate(moveVector); // rotates the given transform 1* around the Y axis
    }
    // ====== Built In Functions (MonoBehaviour) ======
    void Awake() { } // Called automatically First at startup (Before Start)
    void Start() { // After Awake at startup
        // Get a component of type GameObject (or anything) off the current object. 
        // Use the generics interface <Type> to access ONLY a specific type of script
        cachedGameObject = GetComponent<GameObject>();
        art = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>(); // Gets the rigidbody reference. 
        particles = GetComponent<ParticleSystem>();

        sfx = GetComponentInChildren<AudioSource>(); // Gets teh AudioSource from a Child GameObject (in the scene, not the class hierarchy type child). 
    }

    // Built In Functions (MonoBehaviour) - Updates
    // Update is called every frame (at the rendered frame rate, usually 30-60 fps)
    void Update(){
        if( isMoving ){
            Vector3 moveVector = new Vector3(1, 0, 0);
            transform.Translate(moveVector * Time.deltaTime); // Moves each frame, normalized to the frame rate (1 Unit per second)
            transform.Rotate(moveVector); // rotates each frame, normalized to the frame rate (1 Unit per second)
        }

        // Input Functions
        // See Edit > Project Settings > Input - for the full list of Input mappings or to add your own. 
        bool a = Input.GetKeyDown(KeyCode.A); // Maps directly to the keyboard and skips the Input Manager. 
        bool b = Input.GetButton("Jump"); // returns True if the button is currently being held down 
        bool c = Input.GetButtonDown("Jump"); // Default of Spacebar , returns true or false if the button was pressed this frame. 
        float horizontal = Input.GetAxis("Horizontal"); //  // Returns a value between (-1, 1)
        float vertical = Input.GetAxis("Vertical"); //"Vertical" is up and down.
        Vector2 input = new Vector2(horizontal, vertical); // stored as a V2 which can be useful
    
        // eg if the buttonis down, call the method. 
        if( b ) {
            Debug.Log("Button Pressed");
            ButtonPressed();
        }

    } // Called automatically Every Frame

    // To use a rigidbody (physics) use Fixed Update
    // Called automatically every physics frame (about 3-4x faster then Update). All Physics related things need to be in here. 
    private void FixedUpdate() {
        rigid.AddForce(new Vector2(1f, 0)); // add a force to a given vector, doesn't need to be normalized with Time.deltaTime
        rigid.AddTorque(1f); // add rotational force (torque) //note these are for 2d, if you use 3d rigidbody you need to use a Vector3 instead
    } 

	// Keyboard Button Pressed
	private void ButtonPressed(){
		// Effects
		sfx.Play(); // Sounds
		particles.Emit(5); // emit 5 particles
		particles.Play(); // play the particle system loop
		art.color = Color.red; // make the sprite red

		// Spawning a GameObject from a Prefab (assigned in inspector) at a given position and rotation. 
		GameObject go = Instantiate(itemPrefab, transform.position, transform.rotation);
        // Can also instantiate in some other type with 
        // Transform t = Instantiate<Transform>(itemPrefab, transform.position, transform.rotation);
        go.name = "New Game Object"; // assign name
		Destroy(go, 3f); // Destroy new gamePbject in 3 seconds. 

	}

	// CollisionEvents()
    // Called when this rigidbody2D (must have a rigidbody2d with isTrigger = false) first collides with another rigidbody
	private void OnCollisionEnter2D(Collision2D collision){
        // retrieve a script form the collided object
        Transform t = collision.gameObject.GetComponent<Transform>();// access the Tranform on the thing we hit. 
    }
    // Also
    private void OnCollisionStay2D(Collision2D collision) {} // called every frame that the collision continues
    private void OnCollisionExit2D(Collision2D collision) {} // Called when the collision ends. 

    // This will be triggered if the rigidbody is set to be a trigger and hits something, it works similarly but slightly differently. 
    private void OnTriggerEnter2D(Collider2D collision){
        Transform t = collision.gameObject.GetComponent<Transform>();// access the Tranform on the thing we hit. 
        // or filter by Tag
        if( collision.CompareTag("SomeTag")){
            // hit something with a given Tag
        }
    }
}
