using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMotor))]
public class SampleAIController2 : MonoBehaviour {

    public enum AttackMode { Chase, Flee };
    public AttackMode attackMode;
    public Transform target;
    public float fleeDistance = 1.0f;

    private Transform tf;
    private TankMotor motor;
    private TankData data;

    void Awake() {
        tf = GetComponent<Transform>();
        motor = GetComponent<TankMotor>();
        data = GetComponent<TankData>();
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        switch(attackMode) {
            case AttackMode.Chase:
                // Rotate towards the target
                motor.RotateTowards(target.position, data.turnSpeed);
                // Move Forward
                motor.Move(data.forwardSpeed);
                break;
            case AttackMode.Flee:
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
                break;
            default:
                Debug.Log("Something went wrong.");
                break;
        }
    }
}
