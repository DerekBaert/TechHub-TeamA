using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

    [Header("Sprite Reference")]
    public SpriteRenderer sprite;

    [Header("Color")]
    public Color currentColor;

    [Header("Color Values")]
    public Color startColor;
    public Color endColor;

    [Header("Speed in Seconds")]
    public float lerpSpeed = 1f;

    [Header("Controls")]
    public bool triggerOnStart = true;

    // ******* Sine Wave **********
    [Header("Wave Settings")]
    public bool sineWave = false;
    [Header("Height of the wave")]
    [Tooltip("1 is default, larger is taller")]
    public float amplitude = 1f;
    [Header("Frequency of the Wave")]
    [Tooltip("1 is default, Lower is faster")]
    public float frequency = 1f; // angular frequency
    [Header("Sine Offset pushes the whole wave up or down")]
    [Tooltip("0 is default, this is an offset of the position")]
    public float sineOffset = 1f;
    [Header("This is an offset along the direction of the wave")]
    [Tooltip("Most notable compared to another of hte same wave with a different offset")]
    public float phase = 1f;
    public float currentSineOutput = 0;

    // lerp state settings. 
    [Header("Is The Timer Running")]
    public bool isChanging = false;
    private float currentTimer = 0f; // 0 is start color, 1 is the end

    [Header("Debug")]
    public bool DEBUG_MODE = false;

    // manually trigger a color change
    public void TriggerChange(){
        isChanging = true;

    }


	// Use this for initialization
	void Start (){
        if(triggerOnStart){
            isChanging = true;
        }

    }
	
	// Update is called once per frame
	void Update () {
        // if it is currently activated
        if ( isChanging && !sineWave ){

            // *** Time.deltaTime ***
            // Time.deltaTime is the time the last frame took. 
            // Adding lerp speed without it would change it in one frame
            // lerpSpeed * Time.deltaTime would do it in 1 second instead. 
            // It also normalizes the logic from the fluctuations of the frame rate
          
            currentTimer += lerpSpeed * Time.deltaTime ;

            if (frequency == 0) {
                frequency = 0.00001f;
            }
             

            // Change the current color
            currentColor = Color.Lerp( startColor, endColor, currentTimer / frequency);
            



        } else if( isChanging && sineWave ){
            // ******* Sine Wave **********
            // if ( 1 / period != frequency ){
            //  Recalculate frequency & omega
            //  frequency = 1/ (period)
            //  angular = (2 * Mathf.PI) * frequency
            // }

            currentTimer += Time.deltaTime;

            //float amplitude = 1;

            // sounds like a band 
            if( frequency == 0){
                frequency = 0.00001f;
            }
            float omegaProduct = currentTimer / frequency;

            // This will give you a sine wave from (-1,+1) when amp =1 and sineOffset = 0
            // 0.5 sineOffset and 0.5 amplitude will give you a wave from 0...1
            currentSineOutput = (amplitude * Mathf.Sin(omegaProduct + phase)) + sineOffset;

            //float offset

            //if(currentValue > Mathf.PI * 2){
            //    currentValue = 0;
            //}
            if( DEBUG_MODE ){
                if( currentSineOutput > 0){
                    Debug.DrawLine(transform.position, transform.position + new Vector3(0, currentSineOutput, 0), Color.green);
                }else{
                    Debug.DrawLine(transform.position, transform.position + new Vector3(0, currentSineOutput, 0), Color.red);
                }
                
            }

            currentColor = Color.Lerp( startColor, endColor, currentSineOutput);
        }

        // Update the sprite color
        if (sprite != null) {
            sprite.color = currentColor;
        }
    }
}
