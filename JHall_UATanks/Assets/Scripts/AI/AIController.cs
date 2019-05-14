using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMotor))]
public class AIController : MonoBehaviour {

    public Shooter shooter;             // Get Access to the Shooter compenent methods
    public Transform target;            // Target we are going to attack
    public float avoidanceTime = 2.0f;
    public float fleeDistance = 1.0f;
    public float FOV = 45f;
    public float inSights = 10f;
    public float hearDistance = 5f;
    public Transform[] waypoints;

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
    private int timeShot = 0;
    private float fleeTimer = 5f;
    private int currentWaypoint = 0;

    // Start is called before the first frame update
    void Start() {
        FSM = GetComponent<FiniteStateMachine>();
        tf = GetComponent<Transform>();
        tankData = gameObject.GetComponent<TankData>();             // Store Tank data in a variable
        motor = gameObject.GetComponent<TankMotor>();               // Store Tank moter in a variable
        tankData.health = tankData.MaxHealth;                       // Set the current health to max on start
        GameManager.instance.enemies.Add(tankData);                 // Adding AI's Tank Data to our list in the Game Manger to keep track of how many players are in the game
        shellDamge = GameManager.instance.shellDamage;              // Get our shell damage
        currentWaypoint = Random.Range(0, waypoints.Length);
    }

    // Update is called once per frame
    void Update() {

        // Handling Actions
        switch(FSM.persionality) {
            case FiniteStateMachine.Persionality.AllTalk:                           // All Talk Tank
                switch(FSM.aiState) {
                    case FiniteStateMachine.AIState.Chase:                                                              // Chase
                        {// Make a block that can close down in the editor
                            if (avoidanceStage != 0) {
                                DoAvoidance();
                            } else {
                                DoChase();
                            }
                        }
                        break;
                    case FiniteStateMachine.AIState.ChaseAndFire:                                                       // Chase and Fire
                        {// Make a block that can close down in the editor
                            // Chase
                            if (avoidanceStage != 0) {
                                DoAvoidance();
                            } else {
                                DoChase();
                            }

                            // Fire
                            shootTimer -= Time.deltaTime;
                            FSM.tookShots = false;
                            if (shootTimer <= 0) {
                                // AI shoots as soon as it can and when target is in sight
                                if (TargetIsInSight()) {
                                    shootTimer = shooter.Shoot();   // Shoot
                                    timeShot += 1;                  // Increase the times shot
                                    if (timeShot >= 3) {            // Shot 3 times or more
                                        FSM.tookShots = true;       // Send our Finite State machine data need to know
                                        timeShot = 0;               // Reset shots took
                                    }
                                }
                            }
                        }
                        break;
                    case FiniteStateMachine.AIState.Flee:                                                               // Flee
                        {// Make a block that can close down in the editor
                            // If need to avoid
                            if (avoidanceStage != 0) {
                                DoAvoidance();
                            } else {
                                // Otherwise, flee
                                DoFlee();
                            }

                            // Flee for a given time
                            fleeTimer -= Time.deltaTime;
                            FSM.flee = false;
                            if (fleeTimer <= 0) {
                                FSM.flee = true;
                                fleeTimer = 5f;
                            }
                        }
                        break;
                    case FiniteStateMachine.AIState.Patrol:                                                             // Patrol
                        {// Make a block that can close down in the editor
                         // Need Waypoints to move around to the game scene
                            if (avoidanceStage != 0) {
                                DoAvoidance();
                            } else {
                                Patrol(currentWaypoint, 2f);
                            }
                        }
                        break;
                    default:                                                                                            // If all else fails
                        {// Make a block that can close down in the editor
                            Debug.Log("Something went wrong trying to get your aiState");
                        }
                        break;
                }
                break;

            case FiniteStateMachine.Persionality.AttackHunger:                      // Attack Hunger Tank
                // TODO:: Actions for Attack Hunger Tank
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Chase:                                                              // Chase
                        {// Make a block that can close down in the editor

                        }
                        break;
                    default:                                                                                            // If all else fails
                        {// Make a block that can close down in the editor
                            Debug.Log("Something went wrong trying to get your aiState");
                        }
                        break;
                }
                break;

            case FiniteStateMachine.Persionality.ScaredyCat:                        // Scaredy Cat Tank
                // TODO:: Actions for Scaredy Cat Tank
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Flee:                                                               // Flee
                        {// Make a block that can close down in the editor

                        }
                        break;
                    default:                                                                                            // If all else fails
                        {// Make a block that can close down in the editor
                            Debug.Log("Something went wrong trying to get your aiState");
                        }
                        break;
                }
                break;

            case FiniteStateMachine.Persionality.Sniper:                            // Sniper Tank
                // TODO:: Actions for Sniper Tank
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Patrol:                                                             // Patrol
                        {// Make a block that can close down in the editor

                        }
                        break;
                    default:                                                                                            // If all else fails
                        {// Make a block that can close down in the editor
                            Debug.Log("Something went wrong trying to get your aiState");
                        }
                        break;
                }
                break;

            default:                                                                                                    // Can't Find Persionality
                Debug.Log("Something went wrong with getting your Persionality type");
                break;
        }



        /*
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

                // if we have moved long enough, return to an AIstate
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
        // Create a vector to our target then flip to do away
        Vector3 vectorToTarget = target.position - tf.position;
        Vector3 oppsiteVector = -1 * vectorToTarget;
        oppsiteVector.Normalize();                                  // Make magnitude of 1 

        // Go away from target
        oppsiteVector *= fleeDistance;                              // increase the distance to go away from
        Vector3 fleePosition = oppsiteVector + tf.position;         // Get the location that are planing to flee to

        if (CanMove(tankData.forwardSpeed)) {
            motor.RotateTowards(fleePosition, tankData.turnSpeed);  // Rotate to the flee location
            motor.Move(tankData.forwardSpeed);                      // Go to the flee location
        } else {
            avoidanceStage = 1;
        }
    }

    void Patrol(int waypoint, float closeEnough) {
        if (Vector3.SqrMagnitude(waypoints[waypoint].position - tf.position) < ( closeEnough * closeEnough )) {
            currentWaypoint = Random.Range(0, waypoints.Length);
        }

        if (CanMove(tankData.forwardSpeed)) {
            motor.RotateTowards(waypoints[waypoint].position, tankData.turnSpeed);
            motor.Move(tankData.forwardSpeed);
        } else {
            avoidanceStage = 1;
        }
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

    bool TargetIsInSight() {
        // Get the vector to target and find the angle to be able to shoot the target
        Vector3 vectorToTarget = target.position - tf.position;
        float angleToShootTarget = Vector3.Angle(vectorToTarget, tf.forward);

        // target within the angle value to shoot return true
        if(angleToShootTarget <= inSights) {
            return true;
        }

        // Otherwise, return false
        return false;
    }
}
