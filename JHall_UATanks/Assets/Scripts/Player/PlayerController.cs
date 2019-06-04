﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(InputController), typeof(TankMotor))]
public class PlayerController : MonoBehaviour {

    public Shooter shooter;             // Our Shooter component in order to shoot
    public Camera cam;                  // Our Player View Camera
    public Text scoreText;
    public Text livesText;
    public ScoreData sData;


    private TankData tankData;          // Tank's Data
    private TankMotor motor;            // Tank's motor
    private InputController input;      // Player's inputs to do actions
    private GameObject lastHitBy;       // Be able to get the last person that hit this tank
    private int shellDamge;             // Get the damage a shell does when hits
    private float shootTimer = 0f;      // Time to be able to shoot

    // Runs before Start
    void Awake() {
        tankData = gameObject.GetComponent<TankData>();             // Store Tank data in a variable
        motor = gameObject.GetComponent<TankMotor>();               // Store Tank moter in a variable
        input = GetComponent<InputController>();                    // Store our InputController in a variable
        GameManager.instance.tanks.Add(gameObject.transform);       // Add our tranform to a list in the game Manager
    }

    // Start is called before the first frame update
    void Start() {
        tankData.health = tankData.MaxHealth;                       // Set the current health to max on start
        GameManager.instance.players.Add(tankData);                 // Adding player's Tank Data to our list in the Game Manger to keep track of how many players are in the game
        shellDamge = GameManager.instance.shellDamage;              // Get our shell damage
        sData.id = GameManager.instance.scores.Count;
        sData.name = gameObject.name;
        GameManager.instance.scores.Add(sData);
    }

    // Update is called once per frame
    void Update() {
        // Update Score
        UpdateScore();
        livesText.text = "Lives: " + tankData.lives;


        // Get two a max of two input to move around with
        if (input.move1  == InputController.MoveActions.Forward || input.move2 == InputController.MoveActions.Forward) {
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
            // Player died
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Shell") {
            // If we get hit by a shell, take some damage
            tankData.health = motor.TakeDamage(tankData.health, shellDamge);

            // Set lastHitby to the shell owner
            lastHitBy = other.gameObject.GetComponent<ShellController>().tankShooter;
            motor.GivePoints(tankData.pointsGivenOnDestory, lastHitBy);                 // Give points
        }
    }

    void UpdateScore() {
        sData.score = tankData.points;
        scoreText.text = "Score: " + sData.score;

        foreach(ScoreData score in GameManager.instance.scores) {
            if(score.id == sData.id) {
                score.score = sData.score;
            }
        }
    }
}
