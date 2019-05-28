using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMotor))]
public class AIController : MonoBehaviour {

    [HideInInspector]
    public Transform target;            // Target we are going to attack

    public Shooter shooter;             // Get Access to the Shooter compenent methods
    public float avoidanceTime = 2.0f;  // Time in avoiding stage
    public float fleeDistance = 1.0f;   // How far we will flee
    public float FOV = 45f;             // Field of View    
    public float inSight = 5f;          // Distance that can to be able to see a target
    public float hearDistance = 5f;     // The distance in which you can hear
    public Transform[] waypoints;       // Locations that will go to

    private FiniteStateMachine FSM;     // Finite State Machine for our AI
    private Transform tf;               // To store our tranform
    private TankData tankData;          // Tank's Data
    private TankMotor motor;            // Tank's motor
    private GameObject lastHitBy;       // Get the last person that hit this tank
    private int shellDamge;             // Get the damage a shell does when hits
    private float shootTimer = 0;       // AI shoot after timer is done
    private int avoidanceStage = 0;     // The different stages of avoiding
    private float exitTime;             // Time to exit the avoid stage
    private int timeShot = 0;           // Get the amount of shots fired
    private float fleeTimer = 5f;       // Time will be fleeing
    private int currentWaypoint = 0;    // The current way that we go to
    private float closeEnough = 2f;     // Close enough to a waypoint
    private float inSightsAngle = 10f;  // Within shoot angle

    // Runs before Start
    void Awake() {
        FSM = GetComponent<FiniteStateMachine>();                   // Store our Finite State Machine
        tf = GetComponent<Transform>();                             // Store our Transform for easy access
        tankData = gameObject.GetComponent<TankData>();             // Store Tank data in a variable
        motor = gameObject.GetComponent<TankMotor>();               // Store Tank moter in a variable
        GameManager.instance.enemies.Add(tankData);                 // Adding AI's Tank Data to our list in the Game Manger to keep track of how many players are in the game
        GameManager.instance.tanks.Add(tf);                         // Add our transform to our list to kept track on the different players in the game
    }

    // Start is called before the first frame update
    void Start() {
        tankData.health = tankData.MaxHealth;                                           // Set the current health to max on start
        shellDamge = GameManager.instance.shellDamage;                                  // Get our shell damage
        currentWaypoint = Random.Range(0, GameManager.instance.wayPoints.Count);        // Start off with a random waypoint
    }

    // Update is called once per frame
    void Update() {

        // Health
        if (tankData.health <= 0) {
            // Dies
            motor.GivePoints(tankData.pointsGivenOnDestory, lastHitBy);     // Give Points
        }

        // Handling Actions
        switch (FSM.persionality) {
            case FiniteStateMachine.Persionality.AllTalk:                           // All Talk Tank
                // Set Tank to be worried about health
                if(tankData.health <= 30) {
                    FSM.lowHealth = true;
                } else {
                    FSM.lowHealth = false;
                }

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
                                Patrol(currentWaypoint, closeEnough);
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
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Chase:                                                              // Chase
                        {// Make a block that can close down in the editor
                            if(avoidanceStage != 0) {
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

                            // target is in our line of fire
                            if (TargetIsInSight()) {
                                // Shoot
                                shootTimer -= Time.deltaTime;
                                if (shootTimer <= 0) {
                                    shootTimer = shooter.Shoot();
                                }
                            }
                        }
                        break;
                    case FiniteStateMachine.AIState.Patrol:                                                             // Patrol
                        {// Make a block that can close down in the editor
                            if (avoidanceStage != 0) {
                                DoAvoidance();
                            } else {
                                Patrol(currentWaypoint, closeEnough);
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

            case FiniteStateMachine.Persionality.ScaredyCat:                        // Scaredy Cat Tank
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Flee:                                                               // Flee
                        {// Make a block that can close down in the editor
                            if(avoidanceStage != 0) {
                                DoAvoidance();
                            } else {
                                DoFlee();
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


                            if (TargetIsInSight()) {
                                shootTimer -= Time.deltaTime;
                                if (shootTimer <= 0) {
                                    shootTimer = shooter.Shoot();
                                    timeShot += 1;
                                    if (timeShot >= 1) {
                                        FSM.flee = true;
                                    }
                                }
                            }
                        }
                        break;
                    case FiniteStateMachine.AIState.Patrol:                                                             // Patrol
                        {// Make a block that can close down in the editor
                            if(avoidanceStage != 0) {
                                DoAvoidance();
                            } else {
                                Patrol(currentWaypoint, closeEnough);
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

            case FiniteStateMachine.Persionality.PotatoSniper:                            // Sniper Tank
                switch (FSM.aiState) {
                    case FiniteStateMachine.AIState.Patrol:                                                             // Patrol
                        {// Make a block that can close down in the editor
                            if(avoidanceStage != 0) {
                                DoAvoidance();
                            } else {
                                Patrol(currentWaypoint, closeEnough);
                            }
                        }
                        break;
                    case FiniteStateMachine.AIState.ChaseAndFire:                                                       // Chase and Fire
                        {// Make a block that can close down in the editor
                            // Sit and shoot at the target
                            // Try leading shoot
                            shootTimer -= Time.deltaTime;
                            ShootInDirectionGoing();

                        }
                        break;
                    case FiniteStateMachine.AIState.Flee:                                                               // Flee
                        {// Make a block that can close down in the editor
                            if(avoidanceStage != 0) {
                                DoAvoidance();
                            } else {
                                DoFlee();
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

            default:                                                                                                    // Can't Find Persionality
                Debug.Log("Something went wrong with getting your Persionality type");
                break;
        }

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
        if (avoidanceStage == 1) {
            GoAround();

            // If can move forward, go to stage 2
            if (CanMove(tankData.forwardSpeed)) {
                avoidanceStage = 2;
                // Number of second we want to stay in stage 2
                exitTime = avoidanceTime;
            }

        } else if (avoidanceStage == 2) {
            // if we can move forward do so
            if (CanMove(tankData.forwardSpeed)) {
                // Start time countdown and move forward
                exitTime -= Time.deltaTime;
                motor.Move(tankData.forwardSpeed);

                // if we have moved long enough, return to an AIstate
                if (exitTime <= 0) {
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
        if (target != null) {
            motor.RotateTowards(target.position, tankData.turnSpeed);       // Rotate to target's position

            // if can move forward then do so
            if (CanMove(tankData.forwardSpeed)) {
                motor.Move(tankData.forwardSpeed);
            } else {
                // Avoid obstacle that is in front of us
                avoidanceStage = 1;
            }
        }
    }

    // Function: DOFLEE
    void DoFlee() {
        if (target != null) {
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
    }

    // Function: PATROL
    void Patrol(int waypoint, float closeEnoughToWaypoint) {
        // if close enough to a waypoint then change waypoint
        if (Vector3.SqrMagnitude(GameManager.instance.wayPoints[waypoint].position - tf.position) < ( closeEnoughToWaypoint * closeEnoughToWaypoint )) {
            currentWaypoint = Random.Range(0, GameManager.instance.wayPoints.Count);
        }

        // If can move then more otherwise do avoidance
        if (CanMove(tankData.forwardSpeed)) {
            motor.RotateTowards(GameManager.instance.wayPoints[waypoint].position, tankData.turnSpeed);
            motor.Move(tankData.forwardSpeed);
        } else {
            avoidanceStage = 1;
        }
    }

    // Function: CANMOVE
    bool CanMove(float distanceInFront) {
        // Send a Raycast forward
        //  if hits something that is not a player then we can't move
        RaycastHit hit;

        // Find out if we come in to contact (collision) with an object other than player return false
        //      ... Center, Diagonal to the right, and diagonal to the left
        if(Physics.Raycast(tf.position, tf.forward, out hit, distanceInFront)) { // Middle
            if(!hit.collider.CompareTag("Player")) {
                return false;
            }
        }
        
        if (Physics.Raycast(tf.position, tf.forward - tf.right, out hit, distanceInFront)) { // left diagonal
            if (!hit.collider.CompareTag("Player")) {
                return false;
            }
        }

        if (Physics.Raycast(tf.position, tf.forward + tf.right, out hit, distanceInFront)) { // right diagonal
            if (!hit.collider.CompareTag("Player")) {
                return false;
            }
        }
        

        // Otherwise, we can move
        return true;
    }

    // Function: GOAROUND
    void GoAround() {
        RaycastHit hit;
        bool avoid = false;                         // find out of need to avoid anything
        float maxDistance = tankData.forwardSpeed;  // Max distance to see if hit anything
        float direction = 0;                        // Get out direction we want to turn
        float goRight = 0, goLeft = 0;              // Find out what side has more room to go on

        if (Physics.Raycast(tf.position, tf.forward, out hit, maxDistance)) {    // Middle
            if(!hit.collider.CompareTag("Player")) {
                avoid = true;
                goRight += Vector3.SqrMagnitude(hit.collider.transform.position - tf.position);
                goLeft += Vector3.SqrMagnitude(hit.collider.transform.position - tf.position);
            }
        }

        if (Physics.Raycast(tf.position, tf.forward + tf.right, out hit, maxDistance)) {    // Diagonal Right
            if (!hit.collider.CompareTag("Player")) {
                avoid = true;
                goRight += Vector3.SqrMagnitude(hit.collider.transform.position - tf.position);
            }
        }

        if (Physics.Raycast(tf.position, tf.forward - tf.right, out hit, maxDistance)) {    // Diagonal Left
            if (!hit.collider.CompareTag("Player")) {
                avoid = true;
                goLeft += Vector3.SqrMagnitude(hit.collider.transform.position - tf.position);
            }
        }

        if (Physics.Raycast(tf.position, tf.right, out hit, maxDistance)) {                 // Right
            if (!hit.collider.CompareTag("Player")) {
                avoid = true;
                goRight += Vector3.SqrMagnitude(hit.collider.transform.position - tf.position);
            }
        }

        if (Physics.Raycast(tf.position, -tf.right, out hit, maxDistance)) {                // Left
            if (!hit.collider.CompareTag("Player")) {
                avoid = true;
                goLeft += Vector3.SqrMagnitude(hit.collider.transform.position - tf.position);
            }
        }

        // if need to avoid
        if (avoid) {
            // find out the direction need to go
            if(goRight > goLeft) {
                direction = -1;                 // go left
            } else if (goRight <= goLeft) {
                direction = 1;                  // go right
            }

        }

        motor.Rotate(tankData.turnSpeed * direction);
        motor.Move(tankData.forwardSpeed);
        
    }

    // Function: TARGETISINSIGHT
    bool TargetIsInSight() {
        if (target != null) {
            // Get the vector to target and find the angle to be able to shoot the target
            Vector3 vectorToTarget = target.position - tf.position;
            float angleToShootTarget = Vector3.Angle(vectorToTarget, tf.forward);

            // target within the angle value to shoot return true
            if (angleToShootTarget <= inSightsAngle) {
                return true;
            }
        }

        // Otherwise, return false
        return false;
    }

    // Function: SHOOTINDIRECTIONGOING
    void ShootInDirectionGoing() {
        // Find where the target is going to be and shoot in the location it will hit have the bullet makes it that far
        float bulletTravelTimeToTarget = ( target.position - tf.position ).magnitude / (shooter.force * 1.25f);
        Vector3 velocity = target.GetComponent<CharacterController>().velocity;
        Vector3 futurePos = target.position  * bulletTravelTimeToTarget;

        // Rotate to the future position
        motor.RotateTowards(futurePos, tankData.turnSpeed);

        // then shoot
        if (shootTimer <= 0) {
            shootTimer = shooter.Shoot();
        }
    }
}
