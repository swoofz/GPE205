using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private TankData tankData;          // Tank's Data
    private TankMotor motor;            // Tank's motor
    private int health;                 // Current Tank health
    private int shellDamge;             // Get the damage a shell does when hits

    // Start is called before the first frame update
    void Start() {
        tankData = gameObject.GetComponent<TankData>();             // Store Tank data in a variable
        motor = gameObject.GetComponent<TankMotor>();               // Store Tank moter in a vaiable
        health = tankData.MaxHealth;                                // Set the current health to max on start
        GameManager.instance.players.Add(gameObject);               // Adding players to our list in the Game Manger to keep track of how many players are in the game
        shellDamge = GameManager.instance.shellDamage;              // Get our shell damage
    }

    // Update is called once per frame
    void Update() {

        // If health get to zero or lower then destory this object
        if(health <= 0) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Shell") {
            // If we get hit by a shell, take some damage
            health = motor.TakeDamage(health, shellDamge);
        }
    }
}
