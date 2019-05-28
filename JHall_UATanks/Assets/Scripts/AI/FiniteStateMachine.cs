using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour {

    // Different Persionality for tanks
    public enum Persionality { AllTalk, AttackHunger, ScaredyCat, PotatoSniper };
    public Persionality persionality = Persionality.AllTalk;

    // State in which our AI is in
    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Patrol };
    //[HideInInspector]
    public AIState aiState;

    [HideInInspector] public bool lowHealth;    // Get whether or not we have low hp
    [HideInInspector] public bool tookShots;    // Get whether or not we took enoung shots
    [HideInInspector] public bool flee;         // Get whether or not we want to flee


    private AIController ai;        // Get our AI Controller
    private Transform tf;           // Get our Transform
    private float hearDistance;     // Figure out how far we can hear
    private float fleeTimer;        // Timer for fleeing
    private float addFleeTime = 2f; // Time to flee

    void Awake() {
        // Inialize Variables to use
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
                        if (lowHealth) {                                // if low hp
                            ChangeState(AIState.Patrol);                // change state to Patorl
                            persionality = Persionality.PotatoSniper;   // then swicth to a potato sniper
                        } else if (DistanceBetween(ai.target) <= (hearDistance * hearDistance) && CanSee()) {
                            // if can cansee and within distance to hear the
                            ChangeState(AIState.ChaseAndFire);
                        } else if (!CanHear()) { // can't hear
                            ChangeState(AIState.Patrol);
                        }
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        if(tookShots) {
                            // if took enough shoots then flee
                            ChangeState(AIState.Flee);
                        } else if (!CanSee() && CanHear()) {
                            // cant see but can hear then just chase
                            ChangeState(AIState.Chase);
                        } else if(ai.target == null) {
                            // no target then go on partol
                            ChangeState(AIState.Patrol);
                        }
                        break;
                    case AIState.Flee:                                                          // Flee
                        if (flee) {
                            // when fleeing
                            if (!CanHear()) {
                                // cant hear go on patrol
                                ChangeState(AIState.Patrol);
                            } else {
                                // otherwise chase
                                ChangeState(AIState.Chase);
                            }
                        }
                        break;
                    case AIState.Patrol:                                                        // Patrol
                        if (!CanSee() && CanHear()) {
                            // Cant see but can hear so chase
                            ChangeState(AIState.Chase);
                        } else if (DistanceBetween(ai.target) <= ( hearDistance * hearDistance ) && CanSee()) {
                            // see and hear chase and fire
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
                            // see chase and fire
                            ChangeState(AIState.ChaseAndFire);
                        } else if (!CanHear()) {
                            // cant hear - patrol
                            ChangeState(AIState.Patrol);
                        }
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        if(!CanSee() && CanHear()) {
                            // cant see and hear - Chase
                            ChangeState(AIState.Chase);
                        } else if(ai.target == null) {
                            // no target - Patrol
                            ChangeState(AIState.Patrol);
                        }
                        break;
                    case AIState.Patrol:                                                        // Patrol
                        if(CanHear()) {
                            // can hear - Chase
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
                            // see and not want to flee - Chase
                            ChangeState(AIState.ChaseAndFire);
                        } else if(!CanHear() && !flee) {
                            // cant hear and dont want to flee - Patrol
                            ChangeState(AIState.Patrol);
                        }
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        // Set flee timer
                        fleeTimer = addFleeTime;
                        if (flee) {
                            ChangeState(AIState.Flee);
                        }
                        break;
                    case AIState.Patrol:                                                        // Patrol
                        if(CanSee()) {
                            // can see - Chase and fire
                            ChangeState(AIState.ChaseAndFire);
                        } else if(CanHear()) {
                            // can hear - Flee
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
                            // Can see - Chase and Fire
                            ChangeState(AIState.ChaseAndFire);
                        } else if (CanHear()) {
                            // Can Hear - Flee
                            ChangeState(AIState.Flee);
                        }
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        if (CanHear()) {
                            // can hear - Flee
                            ChangeState(AIState.Flee);
                        } else if (!CanSee()) {
                            // cant see - Patrol
                            ChangeState(AIState.Patrol);
                        }
                        break;
                    case AIState.Flee:                                                          // Flee
                        if(!CanHear()) {
                            // cant hear - Patrol
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
        // check that we have a target
        if(target == null) {
            return 0;
        }
        // return the distance between target and us squared
        return Vector3.SqrMagnitude(target.position - tf.position);
    }

    // Function: CANSEE
    bool CanSee() {
        // Check all tanks transform
        foreach(Transform tank in GameManager.instance.tanks) {
            // Don't get this tank with this component
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
        // check all tanks transform
        foreach (Transform tank in GameManager.instance.tanks) {

            // don't get the same tank as this one
            if (tank.name != this.name) {
                // Check if you can hear a tank near by
                float volume = ( hearDistance * hearDistance ) - DistanceBetween(tank);

                // if can hear return true and set a target
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
