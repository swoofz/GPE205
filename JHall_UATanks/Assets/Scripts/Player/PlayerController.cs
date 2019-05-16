using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController), typeof(TankMotor))]
public class PlayerController : MonoBehaviour {

    public Shooter shooter;             // Our Shooter component in order to shoot
    
    private TankData tankData;          // Tank's Data
    private TankMotor motor;            // Tank's motor
    private InputController input;      // Player's inputs to do actions
    private GameObject lastHitBy;       // Be able to get the last person that hit this tank
    private int shellDamge;             // Get the damage a shell does when hits
    private float shootTimer = 0f;      // Time to be able to shoot


    // Start is called before the first frame update
    void Start() {
        tankData = gameObject.GetComponent<TankData>();             // Store Tank data in a variable
        motor = gameObject.GetComponent<TankMotor>();               // Store Tank moter in a variable
        input = GetComponent<InputController>();                    // Store our InputController in a variable
        tankData.health = tankData.MaxHealth;                       // Set the current health to max on start
        GameManager.instance.players.Add(tankData);                 // Adding player's Tank Data to our list in the Game Manger to keep track of how many players are in the game
        GameManager.instance.tanks.Add(gameObject.transform);
        shellDamge = GameManager.instance.shellDamage;              // Get our shell damage
    }

    // Update is called once per frame
    void Update() {
        // Get two a max of two input to move around with
        if(input.move1  == InputController.MoveActions.Forward || input.move2 == InputController.MoveActions.Forward) {
            motor.Move(tankData.forwardSpeed);                  // Move Forward
        }

        if (input.move1 == InputController.MoveActions.Backward || input.move2 == InputController.MoveActions.Backward) {
            motor.Move(-tankData.backwardsSpeed);               // Move Backwards
        }

        if (input.move1 == InputController.MoveActions.Right || input.move2 == InputController.MoveActions.Right) {
            motor.Rotate(tankData.turnSpeed);                   // Turn Right
        }

        if (input.move1 == InputController.MoveActions.Left || input.move2 == InputController.MoveActions.Left) {
            motor.Rotate(-tankData.turnSpeed);                  // Turn Left
        }

        shootTimer -= Time.deltaTime;
        if (input.action == InputController.Action.Shoot) {
            if (shootTimer <= 0) {
                // Be able to shoot when timer is up
                shootTimer = shooter.Shoot();                   // Shoot
            }
        }


        // If health get to zero or lower then destory this object
        if (tankData.health <= 0) {
            // Player dead
            motor.GivePoints(tankData.pointsGivenOnDestory, lastHitBy);     // Give points
            GameManager.instance.players.Remove(tankData);                // Remove from list
            GameManager.instance.tanks.Remove(gameObject.transform);
            Destroy(gameObject);                                            // Destory this
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Shell") {
            // If we get hit by a shell, take some damage
            tankData.health = motor.TakeDamage(tankData.health, shellDamge);

            // Set lastHitby to the shell owner
            lastHitBy = other.gameObject.GetComponent<ShellController>().tankShooter;
        }
    }
}
