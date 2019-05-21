using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

    public GameObject pickupPrefab;
    public float spawnDelay;

    private float nextSpawnTime;
    private GameObject spawnPickup;
    private Transform tf;

    // Start is called before the first frame update
    void Start() {
        tf = GetComponent<Transform>();
        nextSpawnTime =  Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update() {
        // IF there is nothing spawns
        if (spawnPickup == null) {
            // Add it is time to spawn
            if (Time.time > nextSpawnTime) {
                // Spawn it and set the next time
                spawnPickup = Instantiate(pickupPrefab, tf.position, Quaternion.identity) as GameObject;
                spawnPickup.transform.parent = tf;
                nextSpawnTime = Time.time + spawnDelay;
            }
        } else {
            // Otherwise, the object still exist, so postpone the spawn
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
}
