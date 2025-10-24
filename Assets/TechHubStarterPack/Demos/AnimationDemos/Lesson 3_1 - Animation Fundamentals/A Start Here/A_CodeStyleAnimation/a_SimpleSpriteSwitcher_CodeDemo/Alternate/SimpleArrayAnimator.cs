using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleArrayAnimator : MonoBehaviour
{
    // Events that get called
    public UnityEvent OnEndedEvent = new UnityEvent();

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    public int currentSprite;
    // Per Frame
    private float currentTimer = 0;
    public float frameTimer = 1f;

    public bool isPlaying = true;
    public bool repeat = true;

    public bool destroyWhenDone = false;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(sprites.Length == 0 || sprites[0] == null) {
            Debug.LogWarning("Must Assign a Sprite to the array!");
        }
    }

    private void Update() {
        if(isPlaying) {
            // increment timer if we are playing
            currentTimer += Time.deltaTime;


            // check if our timer is up and ready to go to next frame. 
            if (currentTimer > frameTimer) {
                currentSprite++;
                currentTimer = 0;

                // Handle different options for dealing with the end of the array (repeat or stop)
                // MUST do this before trying to assign the sprite. 
                if (currentSprite >= sprites.Length) {

                    currentSprite = 0;

                    if (!repeat) {
                        isPlaying = false;
                        PlayingEnded();
                    }


                }

                if (isPlaying) {
                    // update sprite
                    spriteRenderer.sprite = sprites[currentSprite];
                }

            }
        }
    }

    public void StartPlaying() {
        isPlaying = true;
    }

    public void RestartPlaying() {
        currentTimer = 0;
        currentSprite = 0;
        isPlaying = true;
    }
    public void PausePlaying() {
        isPlaying = false;
    }

    public void PlayingEnded() {
        // anything that happening when we stop playing
        if (destroyWhenDone) {
            Destroy(gameObject);
        }

        OnEndedEvent.Invoke();
    }
}
