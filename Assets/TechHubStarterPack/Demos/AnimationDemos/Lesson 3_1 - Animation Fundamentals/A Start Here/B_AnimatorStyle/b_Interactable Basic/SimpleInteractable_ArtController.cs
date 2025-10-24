using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimpleInteractable_ArtController : MonoBehaviour
{
    public Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump")) {
            anim.SetBool("ActionPressed", true);
        } else {
            anim.SetBool("ActionPressed", false); // don't' forget to shut it off too!
        }
    }
}
