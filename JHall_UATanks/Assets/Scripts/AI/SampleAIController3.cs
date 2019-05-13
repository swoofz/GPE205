using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAIController3 : MonoBehaviour {

    public enum AttackMode { Chase };
    public AttackMode attackMode;

    public Transform target;
    public float avoidanceTime = 2.0f;

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

    }

    // Update is called once per frame
    void Update() {
        if(attackMode == AttackMode.Chase) {
            if(avoidanceStage != 0) {
                DoAvoidance();
            } else {
                DoChase();
            }
        }
    }

    // DoAcoidance - handles obstacle avoidance
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
}
