using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    // Runs before Start
    void Awake() {
        // Send all Spawn points to our GameManager, to be able to get spawn location
        foreach (Transform child in transform) {
            GameManager.instance.SpawnPoints.Add(child);
        }
    }
}
