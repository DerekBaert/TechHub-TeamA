using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// manages story events
// This class is the parent of the UI for displaying events. 
// You call it with a StoryEvent, which is an object that holds the portrait and text content to display
// The Manager class is responsible for opening and closing the ui and updaing the fields. 
public class UIStoryEventManager : MonoBehaviour {

    [Header("--- Story Manager Controls ---")]
    [Header("Displays event dialogue for n seconds")]
    // do we close the UI automatically? 
    public bool closeAfterTime = true;
    public float eventDisplayTime = 2f;
    private float triggeredTime = 999990f;

    [Header("--- UI References ---")]
    [Header("UI Panel")]
    public GameObject panel;

    [Header("Story UI Control References")]
    public Text titleUI;
    public Text contentUI;
    public Image portraitImage;

    [Header("Optional Dialogue Selection Buttons")]
   // public List<string> buttonText;
    //public Button buttonPrefab;
    //private List<Button> buttons;

    [Header("Print Debug Messages")]
    public bool DEBUG_MODE = false;

    // Close the currently open UI
    public void CloseUI(){

        // disable the panel

        panel.SetActive(false);


        // Clear the buttons
        //if (buttons != null) {
        //    for (int i = buttons.Count; i >= 0; i--) {
        //        Destroy(buttons[i]);
        //    }
        //}
    }

    public void Start() {
        CloseUI(); // Close the UI at start
    }

    // Open the UI and display the StoryEvent
    public void OpenUI(StoryEvent eventToShow){

        // *** DEBUG ***
        CheckIfUIIsSet(); // Debugging and testing to ensure things are properly set
        CheckIfEventIsValid(eventToShow); // checks the event itself to ensure it is valid

        // Enabled the Panel that holds the other UI elements
        panel.SetActive(true);
        // Sets each UI element to display the content. 
        titleUI.text = eventToShow.title;
        contentUI.text = eventToShow.textContent;
        portraitImage.sprite = eventToShow.eventPicture;

        // save timer info
        triggeredTime = Time.time;
    }

    // In Progress
    //public void OpenUIWithOptions(ButtonSelector_StoryEvent selectionEvent){
    //    // *** DEBUG ***
    //    CheckIfUIIsSet(); // Debugging and testing to ensure things are properly set
    //    //CheckIfEventIsValid(eventToShow); // checks the event itself to ensure it is valid

    //    if(selectionEvent == null){
    //        Debug.LogWarning("The event is null");
    //    }
        
    //    if(buttonPrefab == null){
    //        Debug.LogWarning("No Button Prefab Set in the event manager");
    //    }

    //    for(int i = 0; i < selectionEvent.buttonOptions.Count; i++){

    //        // Create a new button
    //        Button newButton = Instantiate(buttonPrefab, panel.transform.position, panel.transform.rotation, panel.transform);

    //        buttons.Add(newButton); // add the button to the button list
    //        // tells the button to close the UI when it is clicked or selected

    //        Text buttonText = newButton.GetComponent<Text>(); // Get the text element from the button
    //        buttonText.text = selectionEvent.buttonOptions[i]; // Assign the text.
    //    }
    //    // Enabled the Panel that holds the other UI elements
    //    panel.SetActive(true);
    //    // Sets each UI element to display the content. 
    //    titleUI.text = selectionEvent.title;
    //    contentUI.text = selectionEvent.textContent;
    //    portraitImage.sprite = selectionEvent.eventPicture;

    //    // save timer info
    //    triggeredTime = Time.time;
    //}

    //public void OnButtonClicked(){

    //}

    // *********** All Debugging and error checking **************
    // Checks the state of the UI before we call anything
    private void CheckIfUIIsSet(){
        // Check each UI element reference for a null title, then if it exists, set the title to the story event title
        if (titleUI == null) {
            Debug.LogWarning("OOPS: StoryEventManager (" + gameObject.name + " has no Title UI reference " + gameObject.name);
        }

        // Check each UI element reference for a null contentUI, then if it exists, set the title to the story event title
        if (contentUI == null) {
            Debug.LogWarning("OOPS: StoryEventManager (" + gameObject.name + " has no Content UI reference " + gameObject.name);
        }

        // Check each UI element reference for a null portraitImage, then if it exists, set the portraitImage to the story event portrait
        if (portraitImage == null ) {
            Debug.LogWarning("OOPS: StoryEventManager (" + gameObject.name + " has no Image selected UI reference " + gameObject.name);
        }

        if (panel == null) {
            Debug.LogWarning("OOPS: StoryEventManager(" + gameObject.name + " Panel Reference is missing");
        }
    }

    public void CheckIfEventIsValid(StoryEvent eventToShow){
        
        if (eventToShow == null) {
            Debug.LogWarning("OOPS: You have a null story entry " + gameObject.name);
            return; // skip the rest of this
        }

        if(eventToShow.eventPicture == null){
            Debug.LogWarning("Event has no picture set: " + eventToShow.name);
        }

        // print debugs
       if (DEBUG_MODE)
            Debug.Log("Open UI for event : " + eventToShow.name);
    }

    // Use start to check all the references
    public void Update() {
        if ( closeAfterTime ){
            // Time.time is the current time, Triggered time is the time the UI was opened, event display time is the length of time to display the UI
            if( Time.time > triggeredTime + eventDisplayTime){
                CloseUI();
            }
        }
    }
}
