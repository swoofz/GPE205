using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public enum InputScheme { WASD, arrowKeys };
    public InputScheme input = InputScheme.WASD;

    public TankMotor motor;
    public TankData data;

    private float shootTimer = 0;

    // Start is called before the first frame update
    void Start() {
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update() {
        switch (input) {
            case InputScheme.WASD:                      // WASD Input Scheme
                if(Input.GetKey(KeyCode.W)) {           // Move Forward
                    motor.Move(data.forwardSpeed);
                }
                if(Input.GetKey(KeyCode.S)) {           // Move Backwards
                    motor.Move(-data.backwardsSpeed);
                }
                if(Input.GetKey(KeyCode.D)) {
                    motor.Rotate(data.turnSpeed);       // Turn Right
                }
                if (Input.GetKey(KeyCode.A)) {
                    motor.Rotate(-data.turnSpeed);      // Turn Left
                }
                break;
            case InputScheme.arrowKeys:                 // Arrow Keys Input Scheme
                if(Input.GetKey(KeyCode.UpArrow)) {     // Move Forward
                    motor.Move(data.forwardSpeed);
                }
                if (Input.GetKey(KeyCode.DownArrow)) {  // Move Backwards
                    motor.Move(-data.backwardsSpeed);
                }
                if (Input.GetKey(KeyCode.RightArrow)) { // Turn Right
                    motor.Rotate(data.turnSpeed);
                }
                if (Input.GetKey(KeyCode.LeftArrow)) {  // Turn Left
                    motor.Rotate(-data.turnSpeed);
                }
                break;
        }

        shootTimer -= Time.deltaTime;           // Cooldown Timer for shooting
        if (Input.GetKey(KeyCode.Space)) {      // Shoot
            // if can shoot then shoot and reset the cooldown timer
            if (shootTimer <= 0) {              
                motor.Shoot(data.bulletForce, data.shell , data.barrelTip);
                shootTimer = data.fireRate;
            }
        }
    }
}
