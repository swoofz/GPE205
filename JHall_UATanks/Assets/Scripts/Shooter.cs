using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public GameObject shell;            // Bullets used to fire
    public Transform barrelTip;         // The location of the end of the barrel
    public float fireRate = 1.5f;       // how often can shoot
    public float force = 10f;           // Force added to the shell to move it in the direction fired

    private Transform tf;               // Store our transform

    // Start is called before the first frame update
    void Start() {
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    public float Shoot() {
        GameObject bulletPlaceHolder;            // Create a Storage place for bullet gameobject

        if (tf.parent.transform.Find("Bullets") == null) {                              // If there is no child bullet GameObject
            bulletPlaceHolder = new GameObject();                                       // Create new empty GameObject
            bulletPlaceHolder.name = "Bullets";                                         // Call it Bullets
            bulletPlaceHolder.transform.parent = tf.parent;                             // Set it as a child to this GameObject
        } else {                                                                        // Else
            bulletPlaceHolder = tf.parent.transform.Find("Bullets").gameObject;         // Find the child GameObject Bullets and store it
        }

        // Mulptle the force that will be add by 100
        // Kepts force value lower when trying to find a good force for the bullet to travel
        float newForce = force * 100;

        // Create a GameObject that is spawning a shell at the gunTip position
        GameObject bullet = Instantiate(shell, barrelTip.position, tf.rotation) as GameObject;
        bullet.transform.parent = bulletPlaceHolder.transform;                                  // Set new bullets instantiate as a child of the Bullets GameObject

        // Add force to the bullet to move it forward
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * newForce);

        // Sending Tank that shot the bullet
        bullet.GetComponent<ShellController>().tankShooter = gameObject;

        return fireRate;
    }

    // Return this GameObject to find out who shot this bullet

}
