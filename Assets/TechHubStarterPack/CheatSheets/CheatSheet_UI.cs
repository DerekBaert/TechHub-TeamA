// Dylans Unity Cheat Sheet V2 - March 9th

using UnityEngine; // Defaults
using System.Collections; // Defaults
using System.Collections.Generic; // This Is For Lists
using UnityEngine.UI; // Add this to use the UI elements

// Basic Class
public class CheatSheet_UI : MonoBehaviour {

    public Text textToSet; 
	public Button clickableButton;
    public Slider slider; // Can either listen to the slider, or set the slider. 

	void SampleMethod(){
		
		Input.GetButtonDown("Jump");
		Input.GetAxis("Horizontal");

		// Basic Movement
		Vector3 moveVector = new Vector3(0, 1f, 0);
		transform.Translate(moveVector);
		transform.Rotate(moveVector);
	}

	// Built In Functions (MonoBehaviour)
	void Update(){
        if( Input.GetButtonDown("Jump")){
            UpdateUI(); 
        }

    } // Every Frame
	void Awake(){} // First at startup
	void Start(){ // After Awake at startup
		
		// Register button Listener - You can also do this through the drag and drop interface on the Button itself (in the inspector). 
		clickableButton.onClick.AddListener(ButtonPressed);

        slider.value = 1; // sets the current value of the slider. 
        slider.maxValue = 2; // sets the max value of the slider. 
        slider.minValue = 0; // sets the min value of the slider. 
    } 

	// Update UI
	public void UpdateUI(){
        textToSet.text = "Hello World"; // Assign new text to the Text element. 
	}
	// UI Button Pressed
	private void ButtonPressed(){

        Debug.Log("ButtonPressed");
        
	}

    // UI Button Pressed
    private void ButtonPressed2() {

        Debug.Log("ButtonPressed from the Event drag and drop interface");


    }
    // This is referenced from the Slider component itself, this is only if you want to be able 
    // to drag the slider in the scene, you can also just use it for output by directly setting the value (as in Start). 
    public void OnSliderChanged(float value){
        Debug.Log("Slider changed to : " + value);
    }

}
