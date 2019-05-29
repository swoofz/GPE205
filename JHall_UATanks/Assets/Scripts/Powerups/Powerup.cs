using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Powerup {

    public float speedModifier;             // Change in speed
    public float healthModifier;            // Change in health
    public float maxHealthModifier;         // Change in Max Health
    
    public float duration;                  // Time the power will last for
    public bool isPermanent;                // If power changes stat forever


    // Function: ON_ACTIVATE
    // When activate a powerup update tank data
    public void OnActivate(TankData target) {
        target.forwardSpeed += speedModifier;
        target.MaxHealth += maxHealthModifier;
        target.health += healthModifier;
        if(target.health > target.MaxHealth) {
            target.health = target.MaxHealth;
        }
    }

    // Function: ON_DEACTIVATE
    // Remove powerup effects
    public void OnDeactivate(TankData target) {
        target.forwardSpeed -= speedModifier;
        target.MaxHealth -= maxHealthModifier;
        target.health -= healthModifier;
    }

}
