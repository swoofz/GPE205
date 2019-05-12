using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMotor))]
public class SampleAIController : MonoBehaviour {

    public Transform[] waypoints;
    public TankMotor motor;
    public TankData data;
    public float closeEnough = 1.0f;

    private Transform tf;
    private int currentWaypoint = 0;

    // Awake
    void Awake() {
        tf = gameObject.GetComponent<Transform>();
    }


    // Update is called once per frame
    void Update() {
        // First Waypoint (waypoint[0]) RotateTowards
        // if return true rotate
        // else moveforward
        // get to the waypoints then set a new waypoint
        if(motor.RotateTowards(waypoints[currentWaypoint].position, data.turnSpeed)) {
            // Do Nothing
        } else {
            // Move forward
            motor.Move(data.forwardSpeed);
        }

        if(Vector3.Distance(tf.position, waypoints[currentWaypoint].position) < closeEnough) {
            // Advance to the next waypoint, if we are still in range
            if (currentWaypoint < waypoints.Length - 1) {
                currentWaypoint++;
            }
        }



    }
}
