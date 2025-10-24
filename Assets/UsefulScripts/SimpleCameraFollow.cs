using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleCameraFollow : MonoBehaviour
{

    public GameObject followThisObject;
    public float cameraMoveSpeed = 10f;
    
    private Vector3 followObjectLocation;
    private Vector3 vectorToTargetPosition;
    private Vector3 normVectorToTarget;
    private float distanceToTarget = 0;
    private Vector3 calculatedMoveVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Cache the location of the object to follow by accessing its Transform Component
        followObjectLocation = followThisObject.transform.position;
        
        // "this" keyword refers to the GameObject this script is currently connected to
        vectorToTargetPosition = followObjectLocation - this.transform.position;

        // Draw this to make sure its right
        Debug.DrawRay(this.transform.position, vectorToTargetPosition, Color.yellow);

        // don't need this right now but good to know how to do. 
        distanceToTarget = vectorToTargetPosition.magnitude; // length of vector

        // normalize the vector to make it length 1. 
        normVectorToTarget = vectorToTargetPosition.normalized;

        // Must check if the distance is less then we are trying to move to avoid jitter
        calculatedMoveVector = normVectorToTarget * cameraMoveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, followObjectLocation) > calculatedMoveVector.magnitude)
        {
            transform.Translate(calculatedMoveVector);
        }
        else
        {
            // If the move we are trying to do is longer then the distance to the target, 
            // just assign the position directly. 
            transform.position = followObjectLocation; 
        }

        
        
        Debug.DrawLine(transform.position, followObjectLocation, Color.green);
        // Alternatively could use
        // Debug.DrawRaw(transform.position, calculatedMoveVector);
    }
}
