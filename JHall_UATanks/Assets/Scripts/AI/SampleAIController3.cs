using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAIController3 : MonoBehaviour {

    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Rest };
    public AIState aiState = AIState.Chase;

    public float stateEnterTime;
    public float aiSenseRadius;
    public float restingHealRate; //in hp/second
    public Transform target;
    public float avoidanceTime = 2.0f;
    public float fleeDistance = 1.0f;

    private float lastShootTime;
    private Transform tf;
    private TankData data;
    private TankMotor motor;
    private int avoidanceStage = 0;
    private float exitTime;

    void Awake() {
        tf = GetComponent<Transform>();
        data = GetComponent<TankData>();
        motor = GetComponent<TankMotor>();
    }

    // Start is called before the first frame update
    void Start() {
        data.health = data.MaxHealth;
    }

    // Update is called once per frame
    void Update() {
        if(aiState == AIState.Chase) {
            // Perform behavior
            if(avoidanceStage != 0) {
                DoAvoidance();
            } else {
                DoChase();
            }

            // Check for Transitions    
            if(data.health < data.MaxHealth * 0.5f) {
                ChangeState(AIState.CheckForFlee);
            } else if (Vector3.Distance(target.position, tf.position) <= aiSenseRadius) {
                ChangeState(AIState.ChaseAndFire);
            }
        } else if (aiState == AIState.ChaseAndFire) {
            // Perform behacior
            if(avoidanceStage != 0) {
                DoAvoidance();
            } else {
                DoChase();

                // Limit our firing rate, so we can only shoot if enough time has passed
                if(Time.time > lastShootTime + 2) {
                    // Shoot
                    Debug.Log("Shoot");
                    lastShootTime = Time.time;
                }
            }

            // Check for Transitions
            if(data.health < data.MaxHealth * 0.5f) {
                ChangeState(AIState.CheckForFlee);
            } else if (Vector3.Distance(target.position, tf.position) > aiSenseRadius) {
                ChangeState(AIState.Chase);
            }
        } else if( aiState == AIState.Flee) {
            // Perform behavior
            if(avoidanceStage != 0) {
                DoAvoidance();
            } else {
                DoFlee();
            }

            // Check for Transitions
            if(Time.time >= stateEnterTime + 10) {
                ChangeState(AIState.CheckForFlee);
            }
        } else if (aiState == AIState.CheckForFlee) {
            // Perform behavior
            CheckForFlee();

            // Check for Transitions
            if(Vector3.Distance(target.position, tf.position) <= aiSenseRadius) {
                ChangeState(AIState.Flee);
            } else {
                ChangeState(AIState.Rest);
            }
        } else if (aiState == AIState.Rest) {
            // Perform behavior
            DoRest();

            // Check for Transitions
            if(Vector3.Distance(target.position, tf.position) <= aiSenseRadius) {
                ChangeState(AIState.Flee);
            } else if (data.health >= data.MaxHealth) {
                ChangeState(AIState.Chase);
            }
        }


    }

    // DoAvoidance - handles obstacle avoidance
    void DoAvoidance() { 
        if(avoidanceStage == 1) {
            // Rotate Left
            motor.Rotate(-1 * data.turnSpeed);

            // If can move forward, move to stage 2
            if(CanMove(data.forwardSpeed)) {
                avoidanceStage = 2;

                // Set the number of seconds we will stay in stage 2
                exitTime = avoidanceTime;
            }

            // Otherwise, we'll do this again next turn!
        } else if(avoidanceStage == 2) {
            // if we can move forward do so
            if(CanMove(data.forwardSpeed)) {
                // Subtract from our timer and move
                exitTime -= Time.deltaTime;
                motor.Move(data.forwardSpeed);

                // if we have moved long enough, return to chase mode
                if(exitTime <= 0) {
                    avoidanceStage = 0;
                }
            } else {
                // Otherwise, we can't move forward, so back to stage 1
                avoidanceStage = 1;
            }
        } 
    }

    // DoFlee - 
    void DoFlee() {
        // The vector from ai to target is target position minus our position
        Vector3 vectorToTarget = target.position - tf.position;

        // We can flip this vector by -1 to get a vector AWAY from our target
        Vector3 vectorAwayFromTarget = vectorToTarget * -1;

        // Now, we can normalize that vector to give it a magnitude of 1
        vectorAwayFromTarget.Normalize();

        // A normalize vector can be multiplied by a length to make a vector of that length
        vectorAwayFromTarget *= fleeDistance;

        // We can find the position in space we want to move to by adding our vector away from our ai to our AI's position
        //      This gives us a point that is "that away vector" from our current position
        Vector3 fleePosition = vectorAwayFromTarget + tf.position;
        motor.RotateTowards(fleePosition, data.turnSpeed);
        motor.Move(data.forwardSpeed);
    }

    // DoChase - 
    void DoChase() {
        motor.RotateTowards(target.position, data.turnSpeed);
        // Check if we can move "data.moveSpeed" units away.
        //      We chose this direction, because that is how far we move in 1 second,
        //      This means, we are looking for collisions "one second in the future."
        if(CanMove(data.forwardSpeed)) {
            motor.Move(data.forwardSpeed);
        } else {
            // Enter obstacle avoidance stage 1
            avoidanceStage = 1;
        }
    }

    // CanMove - checks if I can move "speed" units forward. If so, return true. If not, return false.
    bool CanMove(float speed) {
        // Cast a ray forward in the direction we sent
        // If our raycast hit something
        RaycastHit hit;
        if (Physics.Raycast(tf.position, tf.forward, out hit, speed)) {
            // ...and if what we hit is not the player...
            if(!hit.collider.CompareTag("Player")) {
                // ... then we can't move
                return false;
            }
        }

        // otherwise, return true
        return true;
    }

    public void CheckForFlee() {
        // TODO:: Write the CheckForFlee state 
    }

    public void DoRest() {
        // Increase our health. Remember that our incresse is "per second"!
        data.health += restingHealRate * Time.deltaTime;

        // But never go over our max Health
        data.health = Mathf.Min(data.health, data.MaxHealth);
    }

    public void ChangeState(AIState newState) {
        // Change our state
        aiState = newState;

        // save the time we changed states
        stateEnterTime = Time.time;
    }
}
