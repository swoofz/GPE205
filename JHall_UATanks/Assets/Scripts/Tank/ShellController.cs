using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour {

    [HideInInspector]
    public GameObject tankShooter;      // Get the owner of this shell

    private int damage;                 // Shell damage when hit a tank
    private float timeForExistence;     // Time for the shell to be in the scene

    // Start is called before the first frame update
    void Start() {
        damage = GameManager.instance.shellDamage;                      // Store damge value from the GameManager
        timeForExistence = GameManager.instance.shellTimeForExistence;  // Store Time for a shell existence from the GameManger
    }   

    // Update is called once per frame
    void Update() {

        // Count down timer for shell to be active
        timeForExistence -= Time.deltaTime;
        if(timeForExistence <= 0) {
            // if timer runs out destory shell
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        // If collides with anything other than powerups destory this GameObject
        if (other.tag != "Powerups") {
            Destroy(gameObject);
        }
    }
}
