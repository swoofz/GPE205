using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour {

    public GameObject shell;            // Bullets used to fire
    public Transform barrelTip;         // The location of the end of the barrel
    public int MaxHealth = 100;         // Max Health that a Tank can have
    public float forwardSpeed = 3;      // in meters per second
    public float backwardsSpeed = 3;    // in meters per second
    public float turnSpeed = 180;       // in degress per second
    public float fireRate = 1.5f;       // Time till next shot
    public float bulletForce = 10f;     // Force added to the shell to move it in the direction fired
    public int pointsGivenOnDestory;    // points that other tanks get if this tank is destroyed

    // Don't want to be able to set the amount of Points each tank has in inspector but still have
    // access to it in other scripts
    [HideInInspector]
    public int points;                  // Tank points

}
