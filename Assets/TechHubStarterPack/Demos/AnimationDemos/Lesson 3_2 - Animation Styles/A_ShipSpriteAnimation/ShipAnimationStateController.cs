using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimationStateController : MonoBehaviour
{

    public Vector2 cachedInput;

    public Animator animController;

    // Start is called before the first frame update
    void Start()
    {
        // check for references we need. 
        if(animController== null) {
            animController = GetComponent<Animator>();
            Debug.Log("gameObject.name + \" ShipAnimationStateController attampting to grab a reference to the AnimationController Component");
     
            // Make sure it found it. 
            if(animController != null) {
                Debug.LogWarning(gameObject.name + " ShipAnimationStateController needs a reference to the AnimationController Component");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Remember in Unity input isn't consumed so we can just do this (for now).
        cachedInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // This will match the parameter names in the Animation Controller
        // Make sure you spell it correctly and test that the values are changing
        animController.SetFloat("TurnAxis", cachedInput.x);
        animController.SetFloat("ForwardAxis", cachedInput.y);
    }
}
