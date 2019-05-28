using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSpawnPoints : MonoBehaviour {

    // Define what type of spawns we want to store
    public enum SpawnerType { Powerups, Players, Waypoints };
    public SpawnerType points;

    void Awake() {
        switch(points) {
            case SpawnerType.Powerups:
                // Send all Spawn points to our GameManager, to be able to get spawn location
                foreach (Transform child in transform) {
                    GameManager.instance.PowerupSpawns.Add(child);
                }
                break;
            case SpawnerType.Players:
                // Send all Spawn points to our GameManager, to be able to get spawn location
                foreach (Transform child in transform) {
                    GameManager.instance.SpawnPoints.Add(child);
                }
                break;
            case SpawnerType.Waypoints:
                // Send all Spawn points to our GameManager, to be able to get spawn location
                foreach (Transform child in transform) {
                    GameManager.instance.wayPoints.Add(child);
                }
                break;
            default:
                Debug.LogWarning("Something went wrong when trying to find what type of spawn point this is.");
                break;
        }        
    }

}

