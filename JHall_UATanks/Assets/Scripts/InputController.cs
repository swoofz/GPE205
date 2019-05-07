using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMotor), typeof(Shooter))]
public class InputController : MonoBehaviour {

    public enum InputScheme { WASD, arrowKeys };
    public InputScheme input = InputScheme.WASD;

    private TankMotor motor;
    private TankData data;
    private Shooter shooter;

    private float shootTimer = 0;

    // Start is called before the first frame update
    void Start() {
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
        shooter = gameObject.GetComponent<Shooter>();
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
                shootTimer = shooter.Shoot();
            }
        }
    }
}
