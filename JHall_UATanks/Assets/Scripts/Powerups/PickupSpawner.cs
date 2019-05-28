using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

    public GameObject[] pickupPrefab;   // Pick up items
    public float spawnDelay;            // Time take it take to respawn the item
    public int maxPowerups = 3;         // Max amount of power in the map at one time

    private float nextSpawnTime;        // Next time to spawn the item
    private GameObject spawnPickup;     // Get the pickup we want to spawn
    private Transform powerupSpawn;     // Get the position we want to spawn pickup

    // Start is called before the first frame update
    void Start() {
        nextSpawnTime =  Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update() {
        // IF there is room to spawn another powerup
        if (GameManager.instance.powerupCount < maxPowerups) {
            // Add it is time to spawn
            if (Time.time > nextSpawnTime) {
                GetRandomSpawnPoint();
                // Spawn it and set the next time if have a place to spawn
                if (powerupSpawn != null) {
                    spawnPickup = Instantiate(pickupPrefab[Random.Range(0, pickupPrefab.Length)], powerupSpawn.position, Quaternion.identity) as GameObject;
                    spawnPickup.transform.parent = powerupSpawn;
                    GameManager.instance.powerupCount += 1;
                }
                nextSpawnTime = Time.time + spawnDelay;
            }
        } else {
            // Otherwise, the object still exist, so postpone the spawn
            nextSpawnTime = Time.time + spawnDelay;
        }
    }

    void GetRandomSpawnPoint() {
        int size = GameManager.instance.PowerupSpawns.Count;                        // Get how many powerup spawns there are
        powerupSpawn = GameManager.instance.PowerupSpawns[Random.Range(0, size)];   // Get a random powerup spawn and set it
        
        // if already has a power up object as a child set to null
        if(powerupSpawn.childCount != 0) {
            // no overlaps
            powerupSpawn = null;
        }
    }
}
