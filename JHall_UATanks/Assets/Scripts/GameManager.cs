using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;     // Creating our Game Manager Singleton

    public int Player1Points = 0;           // Player1 Points for show
    public int AI1Points = 0;               // AI points for show
    public int shellDamage = 20;            // Damage a shell can do
    public int shellTimeForExistence = 3;   // How long a shell can exist
    public GameObject[] AIPrefabs;          // Store our AI prefabs here
    public GameObject[] PlayerPrefabs;      // Store our Player prefabs here

    [HideInInspector]
    public List<TankData> players;          // Holds all players in the game
    [HideInInspector]
    public List<TankData> enemies;          // Holds all Emenies in the game
    [HideInInspector]
    public List<Transform> tanks;           // Hold all transform of every tank in the game
    [HideInInspector]
    public List<Transform> SpawnPoints;     // Hold Player/Ai Spawn locations
    [HideInInspector]
    public List<Transform> PowerupSpawns;   // Hold All power locations
    [HideInInspector]
    public List<Transform> wayPoints;       // Hold AI waypoints
    [HideInInspector]
    public int powerupCount;                // Hold the amount of powerups are in the game


    private int tanksAlive;         // Store the number of tank alive
    private bool ranOnce = false;   // Run a piece of code once


    // Runs before Start()
    void Awake() {
        if(instance == null) {          // If there is no GameManager instance already in our game
            instance = this;            // Make our GameManager instance equal to this
            DontDestroyOnLoad(this);    // Changing scene don't destory
        }
        else {
            Debug.Log("ERROR: There can only be one GameManager.");     // Send error if to many GameMangers in one scene
            Destroy(gameObject);                                        // Destory the other GameManagers
        }
    }

    // Start is called before the first frame update
    void Start() {
        SpawnPlayers(1);    // Spawn a given amount of players
        SpawnAI();          // Spawn all AI tanks
        Spawn();            // Position all players and ai in the spawnLocations
    }

    // Update is called once per frame
    void Update() {
        RespawnIfNeed();

        // Update Scores
        if (players.Count > 0) {
            Player1Points = players[0].points;
        }

        if (enemies.Count > 0) {
            AI1Points = enemies[0].points;
        }

        tanksAlive = players.Count + enemies.Count;         // A Count of All the tank that are alive

        if(tanksAlive <= 1 && !ranOnce) {                       // If there is only 1 Tank left, possibly no Tanks and haven't ran this code yet
            if(players.Count == 1) {                            // One Player
                Debug.Log(players[0].name + " is the Winner!"); // Player wins
            } else if(enemies.Count == 1) {                     // One Enemy
                Debug.Log(enemies[0].name + " is the Winner!"); // Enemy wins
            } else {                                            // Everyone Died
                Debug.Log("Game ended in a Draw...");           // A Draw
            }

            ranOnce = true; // We ran the code above once
        }

    }

    // Function: RESPAWN
    // Respawn our player or AI in one of the spawn Locations
    public void Respawn(Transform ourPostion) {
        // Deactivate gameobject to reset position and not get an inputs
        ourPostion.gameObject.SetActive(false);
        ourPostion.position = SpawnPoints[Random.Range(0, SpawnPoints.Count)].position;

        // Reactivate gameobject in new position
        ourPostion.gameObject.SetActive(true);
    }

    // Function: SPAWN
    // Spawn all Players/AIs in a random location
    public void Spawn() {
        // Hold what location we used to spawn
        List<int> spawnLocations = new List<int>();
        foreach(Transform player in tanks) {
            // Random spawn location
            int spawnNumber = Random.Range(0, SpawnPoints.Count);

            // Get a spawn location that wasn't already used
            while(spawnLocations.Contains(spawnNumber)) {
                spawnNumber = Random.Range(0, SpawnPoints.Count);
            }

            // Add a spawnlocation and spawn our player
            spawnLocations.Add(spawnNumber);
            player.position = SpawnPoints[spawnNumber].position;
        }
    }

    // Function: RESET_HEALTH
    // Reset the health of our tank
    public void ResetHealth(GameObject go) {
        // Reset player's health
        foreach(TankData data in players) {
            if (go.name == data.name) {
                data.health = data.MaxHealth;
            }
        }

        // Reset AI's health
        foreach (TankData data in enemies) {
            if (go.name == data.name) {
                data.health = data.MaxHealth;
            }
        }
    }

    void SpawnAI() {
        GameObject game = GameObject.Find("Game");
        foreach(GameObject ai in AIPrefabs) {
            Instantiate(ai).transform.parent = game.transform;
        }
    }

    void SpawnPlayers(int playerCount) {
        GameObject game = GameObject.Find("Game");
        for(int i = 0; i < playerCount; i++) {
            Instantiate(PlayerPrefabs[Random.Range(0, PlayerPrefabs.Length)]).transform.parent = game.transform;
        }
    }

    void RespawnIfNeed() {
        foreach(TankData player in players) {
            if(player.health <= 0) {
                ResetHealth(player.gameObject);
                Respawn(player.transform);
            }
        }

        foreach(TankData ai in enemies) {
            if(ai.health <= 0) {
                ResetHealth(ai.gameObject);
                Respawn(ai.transform);
            }
        }
    }
}
