using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour {

    public int MaxHealth = 100;         // Max Health that a Tank can have
    public float forwardSpeed = 3;      // in meters per second
    public float backwardsSpeed = 3;    // in meters per second
    public float turnSpeed = 180;       // in degress per second
    public int pointsGivenOnDestory;    // points that other tanks get if this tank is destroyed

    // Don't want to be able to set the amount of Points each tank has in inspector but still have
    // access to it in other scripts
    [HideInInspector]
    public int points;                  // Tank points
    //[HideInInspector]
    public float health;
}
