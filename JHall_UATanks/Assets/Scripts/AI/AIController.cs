using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMotor))]
public class AIController : MonoBehaviour {

    public float timeRotating = 2.5f;   // Time spend rotating one direction
    public Shooter shooter;             // Get Access to the Shooter compenent methods
    public Transform target;

    private FiniteStateMachine FSM;
    private TankData tankData;          // Tank's Data
    private TankMotor motor;            // Tank's motor
    private GameObject lastHitBy;       // Get the last person that hit this tank
    private int shellDamge;             // Get the damage a shell does when hits
    private float shootTimer = 0;       // AI shoot after timer is done
    private float rotateTimer;          // Changer rotation after timer

    // Start is called before the first frame update
    void Start() {
        FSM = GetComponent<FiniteStateMachine>();
        tankData = gameObject.GetComponent<TankData>();             // Store Tank data in a variable
        motor = gameObject.GetComponent<TankMotor>();               // Store Tank moter in a variable
        tankData.health = tankData.MaxHealth;                       // Set the current health to max on start
        GameManager.instance.enemies.Add(tankData);                 // Adding AI's Tank Data to our list in the Game Manger to keep track of how many players are in the game
        shellDamge = GameManager.instance.shellDamage;              // Get our shell damage
        rotateTimer = timeRotating;
    }

    // Update is called once per frame
    void Update() {

        // Handling Actions
        switch(FSM.persionality) {
            case FiniteStateMachine.Persionality.AllTalk:                                                               // All Talk Tank
                switch(FSM.aiState) {
                    case FiniteStateMachine.AIState.Chase:
                        break;
                    default:
                        Debug.Log("Something went wrong trying to get your aiState");
                        break;
                }
                break;
            case FiniteStateMachine.Persionality.AttackHunger:                                                          // Attack Hunger Tank
                // TODO:: Actions for Attack Hunger Tank
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Chase:
                        break;
                    default:
                        Debug.Log("Something went wrong trying to get your aiState");
                        break;
                }
                break;
            case FiniteStateMachine.Persionality.ScaredyCat:                                                            // Scaredy Cat Tank
                // TODO:: Actions for Scaredy Cat Tank
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Flee:
                        break;
                    default:
                        Debug.Log("Something went wrong trying to get your aiState");
                        break;
                }
                break;
            case FiniteStateMachine.Persionality.Sniper:                                                                // Sniper Tank
                // TODO:: Actions for Sniper Tank
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Patrol:
                        break;
                    default:
                        Debug.Log("Something went wrong trying to get your aiState");
                        break;
                }
                break;
            default:
                Debug.Log("Something went wrong with getting your Persionality type");
                break;
        }



        /*
        // Shoot
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0) {
            // AI shoots as soon as it can
            shootTimer = shooter.Shoot();
        }

        // Rotating
        rotateTimer -= Time.deltaTime;
        if(rotateTimer <= 0) {
            // Rotate in the oppsite direction
            tankData.turnSpeed *= -1;
            rotateTimer = timeRotating;
        }
        motor.Rotate(tankData.turnSpeed);


        // Health
        if(tankData.health <= 0) {
            // Dies
            motor.GivePoints(tankData.pointsGivenOnDestory, lastHitBy);     // Give Points
            GameManager.instance.enemies.Remove(tankData);                // Remove from list
            Destroy(gameObject);                                            // Destory this
        }   
        */
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Shell") {
            // Hit by a shell and loses health
            tankData.health = motor.TakeDamage(tankData.health, shellDamge);
            
            // Set lastHitby to the shell owner
            lastHitBy = other.gameObject.GetComponent<ShellController>().tankShooter;
        }
    }
}
