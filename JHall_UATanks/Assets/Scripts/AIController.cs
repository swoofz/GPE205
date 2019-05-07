﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMotor))]
public class AIController : MonoBehaviour {

    private TankData tankData;          // Tank's Data
    private TankMotor motor;            // Tank's motor
    private GameObject lastHitBy;       // Get the last person that hit this tank
    private int health;                 // Current Tank health
    private int shellDamge;             // Get the damage a shell does when hits
    private float shootTimer = 0;       // AI shoot after timer is done

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
        // Send Points to be able to the GameManager see in the inspector
        GameManager.instance.AI1Points = tankData.points;

        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0) {
            // AI as soon as it can
            motor.Shoot(tankData.bulletForce, tankData.shell, tankData.barrelTip);
            shootTimer = tankData.fireRate;
        }


        if(health <= 0) {
            // Dies
            motor.GivePoints(tankData.pointsGivenOnDestory, lastHitBy);     // Give Points
            GameManager.instance.enemies.Remove(gameObject);                // Remove from list
            Destroy(gameObject);                                            // Destory this
        }   

    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Shell") {
            // Hit by a shell and loses health
            health = motor.TakeDamage(health, shellDamge);
            // Other gameObject = Shell
            // Shell parent = Bullets
            // Bullets parent = AI Tank/Player Tank ex. Player1 (Top most gameObject layer we want)
            //        =          Shell               Bullets                     ex. Player1
            lastHitBy = other.gameObject.transform.parent.gameObject.transform.parent.gameObject;
        }
    }
}
