// [B] - Importing Libraries
// [I] - Can access C# Directly 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatSheet_v0 : MonoBehaviour {
	// Basic Class Cheet Sheet

	// basic variable examples
	bool isSomethingTrue = false;  // true or false, this is a logical operator
	int numberTestVariable = 12345;

	// Public variables are accessible from other Classes and in the editor
	public bool printAdditionalMessage = false;
	public string additionalMessage = "Oh, are you still here?";

	// Private Variables are accessible only within this Class
	private Transform _cachedTransform; // '_' is optional but sometimes conventional for private variables

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

		// 1. Debug.Log prints a string to the console. the "" mean this is a string
		Debug.Log("Hello World");

		string stringVariable = "Good Morning Human, Welcome to the training program";

		// This is another function being called from Start()
		FirstTask();

	}

	// This function gets called from Start, after things have been set up. 
	void FirstTask() {
		// update state
		Debug.Log("This is your first task");
	}
}
