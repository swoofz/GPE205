using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMotor))]
public class AIController : MonoBehaviour {

    private TankData tankData;          // Tank's Data
    private TankMotor motor;            // Tank's motor
    private int health;                 // Current Tank health
    private int shellDamge;             // Get the damage a shell does when hits

    // Start is called before the first frame update
    void Start() {
        tankData = gameObject.GetComponent<TankData>();             // Store Tank data in a variable
        motor = gameObject.GetComponent<TankMotor>();               // Store Tank moter in a vaiable
        health = tankData.MaxHealth;                                // Set the current health to max on start
        GameManager.instance.enemies.Add(gameObject);               // Adding players to our list in the Game Manger to keep track of how many players are in the game
        shellDamge = GameManager.instance.shellDamage;              // Get our shell damage
    }

    // Update is called once per frame
    void Update() {

        if(health <= 0) {
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Shell") {
            health = motor.TakeDamage(health, shellDamge);
        }
    }
}
