using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
public class TankMotor : MonoBehaviour {


    private CharacterController characterController;    // This variable holds our Character Controller component
    private Transform tf;                               // Variable to store a transform


    void Awake() {
        tf = gameObject.GetComponent<Transform>();  // Store our GameObject's Transform in a variable
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

    // Function: Shoot
    // Shoot a Shell the Direction your tank is facing
    public void Shoot(float force, GameObject shell, Transform gunTip) {
        GameObject bulletPlaceHolder;            // Create a Storage place for bullet gameobject

        if (tf.Find("Bullets") == null) {                           // If there is no child bullet GameObject
            bulletPlaceHolder = new GameObject();                   // Create new empty GameObject
            bulletPlaceHolder.name = "Bullets";                     // Call it Bullets
            bulletPlaceHolder.transform.parent = tf;                // Set it as a child to this GameObject
        } else {                                                    // Else
            bulletPlaceHolder = tf.Find("Bullets").gameObject;      // Find the child GameObject Bullets and store it
        }

        // Mulptle the force that will be add by 100
        // Kepts force value lower when trying to find a good force for the bullet to travel
        force *= 100;

        // Create a GameObject that is spawning a shell at the gunTip position
        GameObject bullet = Instantiate(shell, gunTip.position, tf.rotation) as GameObject;
        bullet.transform.parent = bulletPlaceHolder.transform;                                  // Set new bullets instantiate as a child of the Bullets GameObject

        // Add force to the bullet to move it forward
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * force);
    }

    // Function: TakeDamage
    // Lose health when hit
    public int TakeDamage(int health, int damage) {
        // Take away health basic on how much damage was done
        health -= damage;

        // Return our current health
        return health;
    }

    //Function: Givepoints
    // Gives points to the tank the got the killing blow
    public void GivePoints(int points,  GameObject tankDiedTo) {
        // Adds points the tank that got the killing blow points
        tankDiedTo.GetComponent<TankData>().points += points;
    }
}
