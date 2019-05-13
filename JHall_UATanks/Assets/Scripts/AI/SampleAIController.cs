using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMotor))]
public class SampleAIController : MonoBehaviour {

    public enum LoopType { Stop, Loop, PingPong };
    public LoopType loopType;

    public Transform[] waypoints;
    public TankMotor motor;
    public TankData data;
    public float closeEnough = 1.0f;

    private Transform tf;
    private int currentWaypoint = 0;
    private bool isPatrolForward = true;

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

        // Optimization: Taking out square root
        //      Note: Vector3.SqrMagnitude (waypoints[currentWatpoint].position - tf.position) < (closeEnough * closeEnough)
        if(Vector3.Distance(tf.position, waypoints[currentWaypoint].position) < closeEnough) {
            // Run through way points and stop when done
            if (loopType == LoopType.Stop) {
                // Advance to the next waypoint, if we are still in range
                if (currentWaypoint < waypoints.Length - 1) {
                    currentWaypoint++;
                }
            }

            // Circle throught the waypoints
            else if (loopType == LoopType.Loop) {
                // Advance to the next waypoint, if we are still in range
                if(currentWaypoint < waypoints.Length -1) {
                    currentWaypoint++;
                } else {
                    // Otherwise go back to waypoint 0
                    currentWaypoint = 0;
                }
            }

            // Run through to the end then come back from the end to the begining
            else if (loopType == LoopType.PingPong) {
                if(isPatrolForward) {
                    // Advance to the next waypoint, if we are still in range
                    if(currentWaypoint < waypoints.Length - 1) {
                        currentWaypoint++;
                    } else {
                        // Otherwise reverse direction and decrement our current waypoint
                        isPatrolForward = false;
                        currentWaypoint--;
                    }
                } else {
                    // Advance to the next waypoint, if we are still in range
                    if(currentWaypoint > 0) {
                        currentWaypoint--;
                    } else {
                        // Otherwise reverse direction and increment our current waypoint
                        isPatrolForward = true;
                        currentWaypoint++;
                    }
                }
            }

        }



    }
}
