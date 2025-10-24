using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script demonstrates the use of the sprite renderer by manually assigning sprites to it. 
public class SimpleSpriteSwitcher : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int currentSprite;

    // Start is called before the first frame update
    void Start()
    {
        // Uppercase Length, it is not a variable, it is a property
        if(sprites.Length < 1 || sprites[0] == null){
            Debug.LogWarning("Warning: Add Sprites to the array (in SimpleSpriteSwitcher");
        }
        else{
            spriteRenderer.sprite = sprites[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            currentSprite++;
            // Mod gives us the remainder. This is a handy trick for iterating arrays that need
            // to loop back to 0 at the end.
            currentSprite = currentSprite % sprites.Length;

            // Assign the Sprite directly to the renderer
            if (sprites[currentSprite] != null){
                spriteRenderer.sprite = sprites[currentSprite];
            }else{
                Debug.LogError("Error: Sprite in array is null at index " + currentSprite);
            }
           
        }
    }
}
