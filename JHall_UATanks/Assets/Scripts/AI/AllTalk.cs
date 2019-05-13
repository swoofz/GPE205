using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTalk : MonoBehaviour {

    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Rest };
    public AIState aiState = AIState.Chase;
    public float stateEnterTime;
    public float aiSenseRadius;
    public float restingHealRate; //in hp/second

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }



    public void CheckForFlee() {
        // TODO:: Write the CheckForFlee state 
    }

    public void DoRest() {
        // TODO:: Write the Rest state.
    }

    public void ChangeState(AIState newState) {
        // Change our state
        aiState = newState;

        // save the time we changed states
        stateEnterTime = Time.time;
    }

}
