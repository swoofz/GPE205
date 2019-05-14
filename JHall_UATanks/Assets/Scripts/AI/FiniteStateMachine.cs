using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour {

    public enum Persionality { AllTalk, AttackHunger, ScaredyCat, Sniper };
    public Persionality persionality = Persionality.AllTalk;

    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Patrol };
    [HideInInspector] public AIState aiState = AIState.Chase;

    private AIController ai;
    private Transform target;
    private Transform tf;
    private bool lowHealth;
    private bool tookShots;
    private float aiSenseRadius;
    private float fleeTimer;

    void Awake() {
        ai = GetComponent<AIController>();
        target = ai.target;
        tf = ai.transform;
        lowHealth = false;
        tookShots = false;
    }

    void Update() {

        // Handling Transitions
        switch (persionality) {
            case Persionality.AllTalk:                                                          // All Talk Tank
                aiSenseRadius = 5;
                switch(aiState) {
                    case AIState.Chase:
                        if (lowHealth) {
                            persionality = Persionality.Sniper;
                        } else if (Vector3.SqrMagnitude(target.position - tf.position) <= (aiSenseRadius * aiSenseRadius)) {
                            ChangeState(AIState.ChaseAndFire);
                        }
                        break;
                    case AIState.ChaseAndFire:
                        if(tookShots) {
                            ChangeState(AIState.Flee);
                        } else if (Vector3.SqrMagnitude(target.position - tf.position) > ( aiSenseRadius * aiSenseRadius )) {
                            ChangeState(AIState.Chase);
                        }
                        break;
                    case AIState.Flee:
                        if (fleeTimer <= 0) {
                            if (target == null) {
                                ChangeState(AIState.Patrol);
                            } else {
                                ChangeState(AIState.Chase);
                            }
                        }
                        break;
                    case AIState.Patrol:
                        if (Vector3.SqrMagnitude(target.position - tf.position) > ( aiSenseRadius * aiSenseRadius )) {
                            ChangeState(AIState.Chase);
                        } else if (Vector3.SqrMagnitude(target.position - tf.position) <= ( aiSenseRadius * aiSenseRadius )) {
                            ChangeState(AIState.ChaseAndFire);
                        }
                        break;
                    default:
                        Debug.Log("Couldn't find that state");
                        break;
                }

                break;
            case Persionality.AttackHunger:                                                     // Attack Hunger Tank
                // TODO:: Transitions for Attack Hunger Tank
                switch(aiState) {
                    case AIState.Chase:
                        break;
                    case AIState.ChaseAndFire:
                        break;
                    case AIState.Patrol:
                        break;
                    default:
                        Debug.Log("Couldn't find that state");
                        break;
                }
                break;
            case Persionality.ScaredyCat:                                                       // Scaredy Cat Tank
                // TODO:: Transitions for Scaredy Cat Tank
                switch (aiState) {
                    case AIState.Flee:
                        break;
                    case AIState.ChaseAndFire:
                        break;
                    case AIState.Patrol:
                        break;
                    default:
                        Debug.Log("Couldn't find that state");
                        break;
                }
                break;
            case Persionality.Sniper:                                                           // Sniper Tank
                // TODO:: Transitions for Sniper Tank
                switch (aiState) {
                    case AIState.Patrol:
                        break;
                    case AIState.ChaseAndFire:
                        break;
                    case AIState.Flee:
                        break;
                    default:
                        Debug.Log("Couldn't find that state");
                        break;
                }
                break;
            default:
                Debug.Log("Didn't find the personality that is link to this Tank");
                break;
        }
    }

    void ChangeState(AIState newState) {
        // Change our state
        aiState = newState;
    }

}
