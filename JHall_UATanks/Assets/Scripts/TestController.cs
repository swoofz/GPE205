﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

    public TankMotor motor; // Tank's motor methods
    public TankData data;   // Hold tank data


    // Start is called before the first frame update
    void Start() {
        if (data == null) {
            data = gameObject.GetComponent<TankData>();
        }
    }

    // Update is called once per frame
    void Update() {
        motor.Move(data.forwardSpeed);
        motor.Rotate(data.turnSpeed);
    }
}