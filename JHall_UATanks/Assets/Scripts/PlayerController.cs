using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
public class PlayerController : MonoBehaviour {
    
    private TankData tankData;          // Tank's Data
    private TankMotor motor;            // Tank's motor
    private GameObject lastHitBy;       // Get the last person that hit this tank
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
        // Send Points to be able to the GameManager see in the inspector
        GameManager.instance.Player1Points = tankData.points;


        // If health get to zero or lower then destory this object
        if(health <= 0) {
            // Player dead
            motor.GivePoints(tankData.pointsGivenOnDestory, lastHitBy);     // Give points
            GameManager.instance.players.Remove(gameObject);                // Remove from list
            Destroy(gameObject);                                            // Destory this
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Shell") {
            // If we get hit by a shell, take some damage
            health = motor.TakeDamage(health, shellDamge);

            // Get the Tank that last hit this tank
            lastHitBy = other.gameObject.transform.parent.gameObject.transform.parent.gameObject;
        }
    }
}
