using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Required for the Slider
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {

    // The health script to reference
    public Health referencedHealth;

    // the slider that shows the health
    public Slider healthSliderUI;

	// Use this for initialization
	void Start () {
        healthSliderUI.maxValue = referencedHealth.maxHealth;
        healthSliderUI.value = referencedHealth.currentHealth;

    }
	
	// Update is called once per frame
	void Update () {
        // check the health and update the slider every frame
        healthSliderUI.value = referencedHealth.currentHealth;
	}
}
