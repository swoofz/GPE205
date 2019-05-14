using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMotor))]
public class AIController : MonoBehaviour {

    public float timeRotating = 2.5f;   // Time spend rotating one direction
    public Shooter shooter;             // Get Access to the Shooter compenent methods
    public Transform target;            // Target we are going to attack
    public float avoidanceTime = 2.0f;
    public float fleeDistance = 1.0f;

    private FiniteStateMachine FSM;
    private Transform tf;
    private TankData tankData;          // Tank's Data
    private TankMotor motor;            // Tank's motor
    private GameObject lastHitBy;       // Get the last person that hit this tank
    private int shellDamge;             // Get the damage a shell does when hits
    private float shootTimer = 0;       // AI shoot after timer is done
    private float rotateTimer;          // Changer rotation after timer
    private int avoidanceStage = 0;
    private float exitTime;
    private int timeShot;
    private float fleeTimer = 5f;

    // Start is called before the first frame update
    void Start() {
        FSM = GetComponent<FiniteStateMachine>();
        tf = GetComponent<Transform>();
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
                    case FiniteStateMachine.AIState.Chase:                                                              // Chase
                        if(avoidanceStage != 0) {
                            DoAvoidance();
                        } else {
                            DoChase();
                        }
                        break;
                    case FiniteStateMachine.AIState.ChaseAndFire:                                                       // Chase and Fire
                        // Chase
                        if(avoidanceStage != 0) {
                            DoAvoidance();
                        } else {
                            DoChase();
                        }

                        // Fire
                        shootTimer -= Time.deltaTime;
                        FSM.tookShots = false;
                        if (shootTimer <= 0) {
                            // AI shoots as soon as it can
                            shootTimer = shooter.Shoot();
                            timeShot += 1;
                            if(timeShot >= 3) {
                                FSM.tookShots = true;
                                timeShot = 0;
                            }
                        }
                        break;
                    case FiniteStateMachine.AIState.Flee:                                                               // Flee
                        DoFlee();
                        fleeTimer -= Time.deltaTime;
                        FSM.flee = false;
                        if(fleeTimer <= 0) {
                            FSM.flee = true;
                            fleeTimer = 5f;
                        }
                        break;
                    case FiniteStateMachine.AIState.Patrol:                                                             // Patrol
                        break;
                    default:                                                                                            // If all else fails
                        Debug.Log("Something went wrong trying to get your aiState");
                        break;
                }
                break;

            case FiniteStateMachine.Persionality.AttackHunger:                                                          // Attack Hunger Tank
                // TODO:: Actions for Attack Hunger Tank
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Chase:                                                              // Chase
                        break;
                    default:                                                                                            // If all else fails
                        Debug.Log("Something went wrong trying to get your aiState");
                        break;
                }
                break;

            case FiniteStateMachine.Persionality.ScaredyCat:                                                            // Scaredy Cat Tank
                // TODO:: Actions for Scaredy Cat Tank
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Flee:                                                               // Flee
                        break;
                    default:                                                                                            // If all else fails
                        Debug.Log("Something went wrong trying to get your aiState");
                        break;
                }
                break;

            case FiniteStateMachine.Persionality.Sniper:                                                                // Sniper Tank
                // TODO:: Actions for Sniper Tank
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Patrol:                                                             // Patrol
                        break;
                    default:                                                                                            // If all else fails
                        Debug.Log("Something went wrong trying to get your aiState");
                        break;
                }
                break;

            default:                                                                                                    // Can't Find Persionality
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

    // Function: DOAVOIDANCE
    void DoAvoidance() {
        if(avoidanceStage == 1) {
            // TODO:: Find Shortest path around object instead of always rotating left
            // Rotate Left
            motor.Rotate(-tankData.turnSpeed);

            // If can move forward, go to stage 2
            if(CanMove(tankData.forwardSpeed)) {
                avoidanceStage = 2;

                // Number of second we want to stay in stage 2
                exitTime = avoidanceTime;
            }
        } else if(avoidanceStage == 2) {

            // if we can move forward do so
            if(CanMove(tankData.forwardSpeed)) {
                // Start time countdown and move forward
                exitTime -= Time.deltaTime;
                motor.Move(tankData.forwardSpeed);

                // if we have moved long enough, return to chase mode
                if(exitTime <= 0) {
                    avoidanceStage = 0;
                }
            } else {
                // Can't move go back to stage 1
                avoidanceStage = 1;
            }
        } 
    }

    // Function: DOCHASE
    void DoChase() {
        motor.RotateTowards(target.position, tankData.turnSpeed);       // Rotate to target's position

        // if can move forward then do so
        if(CanMove(tankData.forwardSpeed)) {
            motor.Move(tankData.forwardSpeed);
        } else {
            // Avoid obstacle that is in front of us
            avoidanceStage = 1;
        }
    }

    void DoFlee() {
        // TODO:: Write Flee functionality
    }

    void Patrol() {
        // TODO:: Write Patrol Functionality
    }

    bool CanMove(float speed) {
        // Send a Raycast forward
        //  if hits something that is not a player then we can't move
        RaycastHit hit;
        if(Physics.Raycast(tf.position, tf.forward, out hit, speed)) {
            if(!hit.collider.CompareTag("Player")) {
                return false;
            }
        }

        // Otherwise, we can move
        return true;
    }
}
