using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public Powerup powerup;
    public AudioClip feedback;

    private Transform tf;

    // Start is called before the first frame update
    void Start() {
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        // Variable to store other object's Powerup Controller - if it has one
        PowerupController powCon = other.GetComponent<PowerupController>();

        // If the other object has a Powerup Controller
        if(powCon != null) {
            // Add the powerup
            powCon.Add(powerup);

            // Play Feedback(if it is set)
            if(feedback != null) {
                AudioSource.PlayClipAtPoint(feedback, tf.position, 1.0f);
            }

            // Destory this pickup
            Destroy(gameObject);
        }    
    }
}
