using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour {

    public enum Persionality { AllTalk, AttackHunger, ScaredyCat, PotatoSniper };
    public Persionality persionality = Persionality.AllTalk;

    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Patrol };
    //[HideInInspector]
    public AIState aiState;

    [HideInInspector] public bool lowHealth;
    [HideInInspector] public bool tookShots;
    [HideInInspector] public bool flee;


    private AIController ai;
    private Transform tf;
    private float hearDistance;
    private float fleeTimer;
    private float addFleeTime = 2f;

    void Awake() {
        ai = GetComponent<AIController>();
        tf = ai.transform;
        lowHealth = false;
        tookShots = false;
        flee = false;
        hearDistance = ai.hearDistance;
        aiState = AIState.Patrol;
    }

    void Update() {

        // Handling Transitions
        switch (persionality) {
            case Persionality.AllTalk:                                                          // All Talk Tank
                switch(aiState) {
                    case AIState.Chase:                                                         // Chase
                        if (lowHealth) {
                            ChangeState(AIState.Patrol);
                            persionality = Persionality.PotatoSniper;
                        } else if (DistanceBetween(ai.target) <= (hearDistance * hearDistance) && CanSee()) {
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
                        } else if(ai.target == null) {
                            ChangeState(AIState.Patrol);
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
                        } else if (DistanceBetween(ai.target) <= ( hearDistance * hearDistance ) && CanSee()) {
                            ChangeState(AIState.ChaseAndFire);
                        }
                        break;
                    default:                                                                    // If all else fails
                        Debug.Log("Couldn't find that state");
                        break;
                }
                break;

            case Persionality.AttackHunger:                                                     // Attack Hunger Tank
                switch(aiState) {
                    case AIState.Chase:                                                         // Chase
                        if(CanSee()) {
                            ChangeState(AIState.ChaseAndFire);
                        } else if (!CanHear()) {
                            ChangeState(AIState.Patrol);
                        }
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        if(!CanSee() && CanHear()) {
                            ChangeState(AIState.Chase);
                        } else if(ai.target == null) {
                            ChangeState(AIState.Patrol);
                        }
                        break;
                    case AIState.Patrol:                                                        // Patrol
                        if(CanHear()) {
                            ChangeState(AIState.Chase);
                        }
                        break;
                    default:                                                                    // IF all else fails
                        Debug.Log("Couldn't find that state");
                        break;
                }
                break;

            case Persionality.ScaredyCat:                                                       // Scaredy Cat Tank
                switch (aiState) {
                    case AIState.Flee:                                                          // Flee
                        // If want to flee then flee first then do what ever want to do
                        if (flee) {
                            fleeTimer -= Time.deltaTime;
                            if (fleeTimer <= 0) {
                                flee = false;
                            }
                        }


                        if (CanSee() && !flee) {
                            ChangeState(AIState.ChaseAndFire);
                        } else if(!CanHear() && !flee) {
                            ChangeState(AIState.Patrol);
                        }
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        fleeTimer = addFleeTime;
                        if (flee) {
                            ChangeState(AIState.Flee);
                        }
                        break;
                    case AIState.Patrol:                                                        // Patrol
                        if(CanSee()) {
                            ChangeState(AIState.ChaseAndFire);
                        } else if(CanHear()) {
                            ChangeState(AIState.Flee);
                        }
                        break;
                    default:                                                                    // If all else fails
                        Debug.Log("Couldn't find that state");
                        break;
                }
                break;

            case Persionality.PotatoSniper:                                                           // Sniper Tank
                switch (aiState) {
                    case AIState.Patrol:                                                        // Patrol
                        if(CanSee()) {
                            ChangeState(AIState.ChaseAndFire);
                        } else if (CanHear()) {
                            ChangeState(AIState.Flee);
                        }
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        if (CanHear()) {
                            ChangeState(AIState.Flee);
                        } else if (!CanSee()) {
                            ChangeState(AIState.Patrol);
                        }
                        break;
                    case AIState.Flee:                                                          // Flee
                        if(!CanHear()) {
                            ChangeState(AIState.Patrol);
                        }
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

    // Function: CHANGESTATE
    void ChangeState(AIState newState) {
        // Change our state
        aiState = newState;
    }

    // Function: DISTANCEBETWEEN
    float DistanceBetween(Transform target) {
        if(target == null) {
            return 0;
        }
        return Vector3.SqrMagnitude(target.position - tf.position);
    }

    // Function: CANSEE
    bool CanSee() {
        foreach(Transform tank in GameManager.instance.tanks) {
            if (tank.name != this.name) {
                ai.target = tank;
                // Makes sure the target is close enough to for us to see
                if (DistanceBetween(ai.target) <= ai.inSight * ai.inSight) {
                    // Get the direction to the target then find out if that angle looking at the target is...
                    //  less than our Field of View
                    Vector3 agentToTargetVector = ai.target.position - tf.position;
                    float angleToTarget = Vector3.Angle(agentToTargetVector, tf.forward);

                    if (angleToTarget <= ai.FOV) {
                        RaycastHit hit;

                        // Check if we are able to see the player
                        if (Physics.Raycast(tf.position, agentToTargetVector, out hit)) {
                            if (hit.collider.CompareTag("Player")) {
                                // Return true if can see the player
                                ai.target = tank;
                                return true;
                            }
                        }
                    }
                }
            }
        }

        // Otherwise, we can't see the player so return false
        ai.target = null;
        return false;
    }

    // Function: CANHEAR
    bool CanHear() {
        foreach (Transform tank in GameManager.instance.tanks) {

            if (tank.name != this.name) {
                float volume = ( hearDistance * hearDistance ) - DistanceBetween(tank);

                if (volume > 0) {
                    ai.target = tank;
                    return true;
                }
            }
        }

        if (!CanSee()) {
            // Make sure cant see before setting the target to null
            ai.target = null;
        }

        // Otherwise, can't hear anything so return false
        return false;
    }

}
