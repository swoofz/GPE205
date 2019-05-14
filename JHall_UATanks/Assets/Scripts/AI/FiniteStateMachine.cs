using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour {

    public enum Persionality { AllTalk, AttackHunger, ScaredyCat, Sniper };
    public Persionality persionality = Persionality.AllTalk;

    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Patrol };
    [HideInInspector] public AIState aiState = AIState.Chase;

    [HideInInspector] public bool lowHealth;
    [HideInInspector] public bool tookShots;
    [HideInInspector] public bool flee;


    private AIController ai;
    private Transform target;
    private Transform tf;
    private float aiSenseRadius;

    void Awake() {
        ai = GetComponent<AIController>();
        target = ai.target;
        tf = ai.transform;
        lowHealth = false;
        tookShots = false;
        flee = false;
    }

    void Update() {

        // Handling Transitions
        switch (persionality) {
            case Persionality.AllTalk:                                                          // All Talk Tank
                aiSenseRadius = 5;
                switch(aiState) {
                    case AIState.Chase:                                                         // Chase
                        if (lowHealth) {
                            persionality = Persionality.Sniper;
                        } else if (DistanceBetween() <= (aiSenseRadius * aiSenseRadius)) {
                            // TODO:: Only fire if in LOS
                            ChangeState(AIState.ChaseAndFire);
                        }
                        break;
                    case AIState.ChaseAndFire:                                                  // Chase and Fire
                        if(tookShots) {
                            ChangeState(AIState.Flee);
                        } else if (DistanceBetween() > ( aiSenseRadius * aiSenseRadius )) {
                            ChangeState(AIState.Chase);
                        }
                        break;
                    case AIState.Flee:                                                          // Flee
                        if (flee) {
                            if (target == null) {
                                ChangeState(AIState.Patrol);
                            } else {
                                ChangeState(AIState.Chase);
                            }
                        }
                        break;
                    case AIState.Patrol:                                                        // Patrol
                        if (DistanceBetween() > ( aiSenseRadius * aiSenseRadius )) {
                            ChangeState(AIState.Chase);
                        } else if (DistanceBetween() <= ( aiSenseRadius * aiSenseRadius )) {
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
        // TODO:: Write if can see our target
        return false;
    }

    bool CanHear() {
        // TODO:: Write if can hear something
        return false;
    }

}
