using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
public class TankMotor : MonoBehaviour {

    // This variable holds our Character Controller component
    private CharacterController characterController;
    private Transform tf;


    void Awake() {
        tf = gameObject.GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start() {
        // Store the CharacterController in our variable
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Function: Move
    // Move tank forward or backwards
    public void Move(float speed) {
        // Create a vector to store our speed data
        // Set our vector to look the same direction as the Gameobject it is on
        // And apply our speed value to it
        Vector3 speedVector = tf.forward * speed;

        // Call SimpleMove() and send it our vector
        characterController.SimpleMove(speedVector);
    }

    // Function: Rotate
    // Rotates our tank right or left
    public void Rotate(float speed) {
        // Create a variable to rotate right or left at a given speed basic on time (per second)
        Vector3 rotateVector = Vector3.up * speed * Time.deltaTime;

        // Rotate our tank in local space
        tf.Rotate(rotateVector, Space.Self);
    }
}
