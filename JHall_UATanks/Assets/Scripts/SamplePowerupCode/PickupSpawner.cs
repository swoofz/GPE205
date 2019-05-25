﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

    public GameObject[] pickupPrefab;
    public float spawnDelay;
    public int maxPowerups = 3;

    private float nextSpawnTime;
    private GameObject spawnPickup;
    private Transform tf;
    //private Transform randomPowerupSpawn;

    // Start is called before the first frame update
    void Start() {
        foreach (Transform child in transform) {
            GameManager.instance.PowerupSpawns.Add(child);
        }
        nextSpawnTime =  Time.time + spawnDelay;
        //tf = GameManager.instance.PowerupSpawns[0];
    }

    // Update is called once per frame
    void Update() {
        // IF there is nothing spawns
        if (GameManager.instance.powerupCount < maxPowerups) {
            // Add it is time to spawn
            if (Time.time > nextSpawnTime) {
                GetRandomSpawnPoint();
                // Spawn it and set the next time
                if (tf != null) {
                    spawnPickup = Instantiate(pickupPrefab[Random.Range(0, pickupPrefab.Length)], tf.position, Quaternion.identity) as GameObject;
                    spawnPickup.transform.parent = tf;
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
        int size = GameManager.instance.PowerupSpawns.Count;
        tf = GameManager.instance.PowerupSpawns[Random.Range(0, size)];
        if(tf.childCount != 0) {
            tf = null;
        }
    }
}
