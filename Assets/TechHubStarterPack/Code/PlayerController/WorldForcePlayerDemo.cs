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
    public float slowDownFactor = 0.5f; // Factor to reduce speed
    public float slowDownDuration = 2f; // Duration of slowdown

    // Physics References
    // Note: I used Private here as we don't want people assigning this value in the editor, 
    //      We will grab the reference with code in the Start Method
    private Rigidbody2D rigid;

    // Add these new fields
    [SerializeField]
    private bool isAlive = true;

    public bool IsAlive => isAlive; // Public property to check player state

    // Add this new method to handle collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TrashObstacle") && isAlive)
        {
            StartCoroutine(SlowDown());
        }
    }

    private IEnumerator SlowDown()
    {
        float originalForceAmount = forceAmount; // Store original speed
        forceAmount *= slowDownFactor; // Reduce speed

        Debug.Log("Player slowed down!");

        yield return new WaitForSeconds(slowDownDuration); // Wait for 2 seconds

        forceAmount = originalForceAmount; // Restore original speed
        Debug.Log("Player speed restored!");
    }
    
    // Start is called before the first frame update
    void Start() 
    {
        // This is get a reference to the local Rigidbody
        rigid = GetComponent<Rigidbody2D>();

        // Prevent falling:
        rigid.gravityScale = 0f; // option A: disable gravity
        // rigid.constraints = RigidbodyConstraints2D.FreezePositionY; // option B: freeze Y position instead
    }

    // Update is called once per frame
    void Update() 
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    // Physics Update: Provide Forces in this loop instead. 
    void FixedUpdate() 
    {
        // Movement
        // Get horizontal input (e.g., A/D keys or Left/Right arrow keys)
        float horizontalInput = xInput;

        // Set horizontal velocity, keep vertical locked at 0 so the player won't fall
        rigid.linearVelocity = new Vector2(horizontalInput * forceAmount, 0f);
    }
}
