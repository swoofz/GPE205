using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
public class TankMotor : MonoBehaviour {


    private CharacterController characterController;    // This variable holds our Character Controller component
    private Transform tf;                               // Variable to store a transform

    // AWAKE
    void Awake() {
        tf = gameObject.GetComponent<Transform>();  // Store our GameObject's Transform in a variable
    }

    // START is called before the first frame update
    void Start() {
        // Store the CharacterController in our variable
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Function: MOVE
    // Move tank forward or backwards
    public void Move(float speed) {
        // Create a vector to store our speed data
        // Set our vector to look the same direction as the Gameobject it is on
        // And apply our speed value to it
        Vector3 speedVector = tf.forward * speed;

        // Call SimpleMove() and send it our vector
        characterController.SimpleMove(speedVector);
    }

    // Function: ROTATE
    // Rotates our tank right or left
    public void Rotate(float speed) {
        // Create a variable to rotate right or left at a given speed basic on time (per second)
        Vector3 rotateVector = Vector3.up * speed * Time.deltaTime;

        // Rotate our tank in local space
        tf.Rotate(rotateVector, Space.Self);
    }

    // Function: TAKEDAMAGE
    // Lose health when hit
    public float TakeDamage(float health, int damage) {
        // Take away health basic on how much damage was done
        health -= damage;

        // Return our current health
        return health;
    }

    //Function: GIVEPOINTS
    // Gives points to the tank the got the killing blow
    public void GivePoints(int points,  GameObject tankDiedTo) {
        // Adds points the tank that got the killing blow points
        tankDiedTo.GetComponent<TankData>().points += points;
    }

    // Function: ROTATETOWARDS
    // Rotate towards our tagart if not already face it
    public bool RotateTowards(Vector3 target, float rotateSpeed) {
        Vector3 vectorToTarget;

        // The vecotr to our target is the DIFFERENCE
        //      between the target position and our position.
        vectorToTarget = target - tf.position;

        // Freeze the rotation on the x and y axis ... Get ride of tilt on flat surface
        Vector3 freezeRotation = new Vector3(vectorToTarget.x, 0, vectorToTarget.z);

        // Find the Quaternion that looks down that vector
        Quaternion targetRotation = Quaternion.LookRotation(freezeRotation);

        // If not facing the target we rotate
        if (tf.rotation != targetRotation) {
            // Turn toward target at a constant speed
            tf.rotation = Quaternion.RotateTowards(tf.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            return true;
        }

        // Otherwise we don't need to rotate
        return false;
    }
}
