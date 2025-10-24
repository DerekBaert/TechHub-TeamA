// [B] - Importing Libraries
// [I] - Can access C# Directly 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Add this to use the UI
//using UnityEngine.UI;


// This is the slightly improved cheatsheet v1. Friday March 9th - Dylan 
public class CheatSheet_v1 : MonoBehaviour {
	// Basic Class Cheet Sheet

	// Public variables are accessible from other Classes and in the editor
	// In Unity that also means its exposed in the Editor
	public bool isKeyDown = false;  
	public int currentTurn = 0;
	public int[] arrayOfInts; // an array can hold many of an item
	public List<int> listOfInts = new List<int>(); // List is like an array but with more features
	public List<string> whatHappenedThisTurn = new List<string>();

	public bool printAdditionalMessage = true;
	public string additionalMessage = "So Far So Good!";

	// Private Variables are accessible only within this Class
	// Variables can be of Componets
	private Transform _cachedTransform; // '_' is optional but sometimes conventional for private variables

	// These are UI COmponents that we can attach to in the Editor Window
//	public Text turnSummaryDisplay; 

	// Use this for initialization
	// Start is a Unity function (
	// [I] - It is a part of the MonoBehaviour Class that this script extends from

	// Functions: Sequences of tasks that can be triggered with a Function Call. 
	// This stuff automatically happens at the beginning when the script turns on
	void Awake(){
		// First things first
		// Initialization tasks
	}

	// Runs When the script starts, but after Awake()
	void Start() {
		// This is another function being called from Start()

		InitializeSetup();
	}

	// Update gets called every turn automatically
	// Add your own code to check frame by frame
	void Update(){
		// wait for input

		// Get Key Down is triggerer on the frame that the button was pressed on 
		if( Input.GetKeyDown(KeyCode.A)){
			// Listen for the A Key to be pressed
			NextTurn(); // Trigger the next turn
		}
	}

	// This function gets called from Start, after things have been set up. 
	void InitializeSetup() {
		// update state
		string message = "This is your first task";
		// Its much cleaner syntax to do in two steps and makes debugging simpler
		Debug.Log(message);
		// Especially when we want to reuse it
		// Spelling mistakes in strings that cause them to mismatch can be a difficult and frustrating bug to track down
	//	turnSummaryDisplay.text = message;

	}

	void NextTurn(){
		// * * Update state * *
		// Increment the turn
		currentTurn = currentTurn + 1;
		whatHappenedThisTurn.Add("This Turn was turn number " + currentTurn.ToString());
		// Next Story beat or gameplay update is called on the next frame that the key is pressed
	}
}
