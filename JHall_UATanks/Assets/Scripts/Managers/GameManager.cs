using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;     // Creating our Game Manager Singleton

    public int shellDamage = 20;            // Damage a shell can do
    public int shellTimeForExistence = 3;   // How long a shell can exist
    public GameObject[] AIPrefabs;          // Store our AI prefabs here
    public GameObject[] PlayerPrefabs;      // Store our Player prefabs here


    public List<ScoreData> scores;                  // List of all scores
    public string[] playerNames = new string[2];    // Default names for player 1 and player 2


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
    [HideInInspector]
    public bool gameIsRunning = false;      // Check if the game is running
    [HideInInspector]
    public float waitForLoadTimer;          // Time so things can load in before starting game



    private UIManager ui;               // Get our UI Manager
    private int tanksAlive;             // Store the number of tank alive

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
        playerNames[0] = "Player 1";
        playerNames[1] = "Player 2";
        ui = GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update() {
        // Check whether or not we have started our game
        CheckToStartGameLogic();

        if (gameIsRunning) {
            // Check if player needs a Respawn or needs to be Removed
            RespawnIfNeed();
            RemovePlayers();

            tanksAlive = players.Count;         // A Count of All the tank that are alive

            if (tanksAlive <= 0) {                       // If there is only 1 Tank left, possibly no Tanks and haven't ran this code yet
                // Change Background music and switch to a gameover screen
                AudioManager.instance.ChangeBackgroundAudio("MenuMusic");
                ui.ShowGameOverScreen();
                gameIsRunning = false;
            }

        } else {
            // If not starting game then ending so Remove tanks
            if (tanks.Count > 0 && waitForLoadTimer <= 0) {
                foreach (Transform tank in tanks) {
                    tank.gameObject.SetActive(false);
                }
                RemovePlayers();
            }
        }

    }

    // Function: RESET
    // Reset Game Stats
    public void Reset() {
        // Clear all list
        players.Clear();
        enemies.Clear();
        tanks.Clear();
        SpawnPoints.Clear();
        PowerupSpawns.Clear();
        wayPoints.Clear();
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

    // Function: SPAWN_AI
    // Used to spawn on start
    public void SpawnAI() {
        GameObject game = GameObject.Find("Game");
        // Spawn all AI
        foreach(GameObject ai in AIPrefabs) {
            Instantiate(ai).transform.parent = game.transform;
        }
    }

    // Function: SPAWN_PLAYERS
    // Spawn a spefic number of players on start
    public void SpawnPlayers(int playerCount) {
        GameObject game = GameObject.Find("Game");
        for(int i = 0; i < playerCount; i++) {
            GameObject player = Instantiate(PlayerPrefabs[i]) as GameObject;
            player.name = playerNames[i];
            player.transform.parent = game.transform;

            // Multiplayer Camera and Input Scheme
            if (playerCount > 1) {
                PlayerController controller = player.GetComponent<PlayerController>();
                if(i == 0) {
                    // Player 1 Top Screen
                    controller.cam.rect = new Rect(0, 0.5f, 1, 0.5f);
                } else {
                    // Player 2 Bottom Screen and arrowKey input Scheme
                    controller.cam.rect = new Rect(0, 0, 1, 0.5f);
                    player.GetComponent<InputController>().input = InputController.InputScheme.arrowKeys;
                }
            }
        }
    }

    // Function: RESPAWN_IF_NEED
    // Player / AI health lower or equal to zero respawn is needed
    void RespawnIfNeed() {
        // Check if a player needs a respawn
        foreach(TankData player in players) {
            if (player.health <= 0) {
                player.lives -= 1;              // Remove one life
                // PLay Death Sound
                AudioSource.PlayClipAtPoint(AudioManager.instance.GetClip("Death"), player.transform.position, AudioManager.instance.volume("Death"));

                if (player.lives > 0) {
                    ResetHealth(player.gameObject); // Reset health
                    Respawn(player.transform);      // Then respawn
                } else {
                    player.gameObject.SetActive(false);
                }
            }
        }

        // Check if an ai needs a respawn
        foreach(TankData ai in enemies) {
            if(ai.health <= 0) {
                // Play Death Sound
                AudioSource.PlayClipAtPoint(AudioManager.instance.GetClip("Death"), ai.transform.position, AudioManager.instance.volume("Death"));
                ResetHealth(ai.gameObject); // Reset health
                Respawn(ai.transform);      // Then respawn
            }
        }
    }

    // Function: REMOVE_PLAYERS
    void RemovePlayers() {
        // Create a new list
        List<TankData> removeData = new List<TankData>();

        // Go through existing list and find the tank need to remove
        foreach(TankData tank in players) {
            if(!tank.gameObject.activeSelf) {
                removeData.Add(tank);
            }
        }

        foreach(TankData tank in enemies) {
            if (!tank.gameObject.activeSelf) {
                removeData.Add(tank);
            }
        }

        // Remove the tanks for the lists
        foreach (TankData tank in removeData) {
            tanks.Remove(tank.transform);
            players.Remove(tank);
            enemies.Remove(tank);
            Destroy(tank.gameObject);
        }

        // Clear our new list
        removeData.Clear();
    }

    // Function: CHECK_TO_START_GAME_LOGIC
    // Run game logic check after time is over
    void CheckToStartGameLogic() {
        if (waitForLoadTimer > 0) {
            waitForLoadTimer -= Time.deltaTime;
            if (waitForLoadTimer <= 0 && !gameIsRunning) {
                gameIsRunning = true;
                waitForLoadTimer = 4f;
            }
        }
    }
}
