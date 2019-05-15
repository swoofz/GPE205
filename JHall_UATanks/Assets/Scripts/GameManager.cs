using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;     // Creating our Game Manager Singleton

    public int Player1Points = 0;           // Player1 Points for show
    public int AI1Points = 0;               // AI points for show
    public int shellDamage = 20;            // Damage a shell can do
    public int shellTimeForExistence = 3;   // How long a shell can exist

    [HideInInspector]
    public List<TankData> players;   // Holds all players in the game
    [HideInInspector]
    public List<TankData> enemies;   // Holds all Emenies in the game


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

    }

    // Update is called once per frame
    void Update() {
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

    public TankData GetTargetTankData(Transform target) {
        foreach (TankData player in players) {
            if(player.name == target.name) {
                return player;
            }
        }

        foreach (TankData enemy in enemies) {
            if(enemy.name == target.name) {
                return enemy;
            }
        }


        return null;
    }

}
