using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public Powerup powerup;         // Set powerup values

    private Transform tf;       // Store our transfrom
    private string clipName;

    // Start is called before the first frame update
    void Start() {
        tf = GetComponent<Transform>();
        clipName = "pickup";
    }

    void OnTriggerEnter(Collider other) {
        // Variable to store other object's Powerup Controller - if it has one
        PowerupController powCon = other.GetComponent<PowerupController>();

        // If the other object has a Powerup Controller
        if(powCon != null) {
            // Add the powerup
            powCon.Add(powerup);

            // Play Feedback(if it is set)
            if(AudioManager.instance.HaveClip(clipName)) {
                AudioSource.PlayClipAtPoint(AudioManager.instance.GetClip(clipName), tf.position, AudioManager.instance.volume(clipName));
            }

            // Destory this pickup
            Destroy(gameObject);
            GameManager.instance.powerupCount -= 1;
        }    
    }
}
