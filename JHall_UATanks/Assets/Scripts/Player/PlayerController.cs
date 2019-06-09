using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(InputController), typeof(TankMotor))]
public class PlayerController : MonoBehaviour {

    public Shooter shooter;             // Our Shooter component in order to shoot
    public Camera cam;                  // Our Player View Camera
    public Text scoreText;              // Get our score text
    public GameObject livesUI;          // Get where our lives are and update them
    public Slider worldHealthBar;       // Player health show to the world
    public Slider cameraHealthBar;      // Player health show to the camera

    public ScoreData sData;             // Create ScoreData basic on this component

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
        sData.id = GameManager.instance.scores.Count;               // Set our ScoreData id to be able to get it later
        sData.name = gameObject.name;                               // Set our ScoreData name
        GameManager.instance.scores.Add(sData);                     // Add to our score list
    }

    // Update is called once per frame
    void Update() {
        UpdateScore();  // Update Score
        UpdateLives();  // Update Lives on the UI
        UpdateHealth(); // Update Slide health bar


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
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Shell") {
            // If we get hit by a shell, take some damage
            tankData.health = motor.TakeDamage(tankData.health, shellDamge);
            // Play take damage clip
            AudioSource.PlayClipAtPoint(AudioManager.instance.GetClip("GotShot"), transform.position, AudioManager.instance.volume("GotShot"));

            if (tankData.health <= 0) {
                // Set lastHitby to the shell owner
                lastHitBy = other.gameObject.GetComponent<ShellController>().tankShooter;
                motor.GivePoints(tankData.pointsGivenOnDestory, lastHitBy);                 // Give points
            }
        }
    }


    // Function: UPDATE_SCORE
    // Change our point value
    void UpdateScore() {
        // Change scores value and text
        sData.score = tankData.points;
        scoreText.text = "Score: " + sData.score;

        // Change the value of this scoredata in GameManger list
        foreach(ScoreData score in GameManager.instance.scores) {
            if(score.id == sData.id) {
                score.score = sData.score;
            }
        }
    }

    // Function: UPDATE_LIVES
    // Update the lives on the UI
    void UpdateLives() {
        int count = 0;                                      // Get a start value
        foreach(Transform life in livesUI.transform) {      // Go into the transform that stores our lives
            if(tankData.lives > count) {                    // Found out if we have this life
                life.gameObject.SetActive(true);            // Makes sure our life is active to see
                count += 1;                                 // increase our count
            } else {
                life.gameObject.SetActive(false);           // Otherwise, don't show life
            }
        }
    }

    // Function: UPDATE_HEALTH
    // UI healtth show to player and other players
    void UpdateHealth() {
        worldHealthBar.maxValue = tankData.MaxHealth;
        cameraHealthBar.maxValue = tankData.MaxHealth;
        worldHealthBar.value = tankData.health;
        cameraHealthBar.value = tankData.health;
    }
}
