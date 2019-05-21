using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatController : MonoBehaviour {

    public PowerupController powCon;
    public Powerup cheatPowerup;

    // Start is called before the first frame update
    void Start() {
        if(powCon == null) {
            powCon = GetComponent<PowerupController>();
        }

    }

    // Update is called once per frame
    void Update() {
        // If H and U are down and this is the first frame that E is pressed
        if (Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.E)) {
            // Add our power up to the tank
            powCon.Add(cheatPowerup);
        }
    }
}
