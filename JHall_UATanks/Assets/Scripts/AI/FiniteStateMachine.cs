using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour {

    public enum Persionality { AllTalk, AttackHunger, ScaredyCat, Sniper };
    public Persionality persionality = Persionality.AllTalk;

    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Patrol };
    /*[HideInInspector]*/ public AIState aiState = AIState.Chase;

    [HideInInspector] public bool lowHealth;
    [HideInInspector] public bool tookShots;
    [HideInInspector] public bool flee;


    private AIController ai;
    private Transform target;
    private Transform tf;
    private float hearDistance;

    void Awake() {
        ai = GetComponent<AIController>();
        target = ai.target;
        tf = ai.transform;
        lowHealth = false;
        tookShots = false;
        flee = false;
        hearDistance = ai.hearDistance;
    }

    void Update() {

        // Handling Transitions
        switch (persionality) {
            case Persionality.AllTalk:                                                          // All Talk Tank
                switch(aiState) {
                    case AIState.Chase:                                                         // Chase
                        if (lowHealth) {
                            persionality = Persionality.Sniper;
                        } else if (DistanceBetween() <= (hearDistance * hearDistance) && CanSee()) {
                            ChangeState(AIState.ChaseAndFire);
                        } else if (!CanHear()) {
                            ChangeState(AIState.Patrol);
                        }
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        if(tookShots) {
                            ChangeState(AIState.Flee);
                        } else if (!CanSee() && CanHear()) {
                            ChangeState(AIState.Chase);
                        }
                        break;
                    case AIState.Flee:                                                          // Flee
                        if (flee) {
                            if (!CanHear()) {
                                ChangeState(AIState.Patrol);
                            } else {
                                ChangeState(AIState.Chase);
                            }
                        }
                        break;
                    case AIState.Patrol:                                                        // Patrol
                        if (!CanSee() && CanHear()) {
                            ChangeState(AIState.Chase);
                        } else if (DistanceBetween() <= ( hearDistance * hearDistance ) && CanSee()) {
                            ChangeState(AIState.ChaseAndFire);
                        }
                        break;
                    default:                                                                    // If all else fails
                        Debug.Log("Couldn't find that state");
                        break;
                }
                break;

            case Persionality.AttackHunger:                                                     // Attack Hunger Tank
                // TODO:: Transitions for Attack Hunger Tank
                switch(aiState) {
                    case AIState.Chase:                                                         // Chase
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        break;
                    case AIState.Patrol:                                                        // Patrol
                        break;
                    default:                                                                    // IF all else fails
                        Debug.Log("Couldn't find that state");
                        break;
                }
                break;

            case Persionality.ScaredyCat:                                                       // Scaredy Cat Tank
                // TODO:: Transitions for Scaredy Cat Tank
                switch (aiState) {
                    case AIState.Flee:                                                          // Flee
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        break;
                    case AIState.Patrol:                                                        // Patrol
                        break;
                    default:                                                                    // If all else fails
                        Debug.Log("Couldn't find that state");
                        break;
                }
                break;

            case Persionality.Sniper:                                                           // Sniper Tank
                // TODO:: Transitions for Sniper Tank
                switch (aiState) {
                    case AIState.Patrol:                                                        // Patrol
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        break;
                    case AIState.Flee:                                                          // Flee
                        break;
                    default:                                                                    // If all else fails
                        Debug.Log("Couldn't find that state");
                        break;
                }
                break;

            default:                                                                            // Can't Find Persionality
                Debug.Log("Didn't find the personality that is link to this Tank");
                break;
        }
    }

    void ChangeState(AIState newState) {
        // Change our state
        aiState = newState;
    }

    float DistanceBetween() {
        return Vector3.SqrMagnitude(target.position - tf.position);
    }

    bool CanSee() {
        // Get the direction to the target then find out if that angle looking at the target is...
        //  less than our Field of View
        Vector3 agentToTargetVector = target.position - tf.position;
        float angleToTarget = Vector3.Angle(agentToTargetVector, transform.forward);

        if (angleToTarget <= ai.FOV) {
            RaycastHit hit;

            // Check if we are see the player
            if (Physics.Raycast(tf.position, agentToTargetVector, out hit)) {
                if(hit.collider.CompareTag("Player")) {
                    // Return true if can see the player
                    return true;
                }
            }
        }

        // Otherwise, we can't see the player so return false
        return false;
    }

    bool CanHear() {
        if(target != null) {
            float volume = (hearDistance * hearDistance) - DistanceBetween();

            if(volume > 0) {        // Can hear something if volume is positive
                return true;        //  ...so return true
            }
        }
        
        // Otherwise, can't hear anything so return false
        return false;
    }

}
