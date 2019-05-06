using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;     // Creating our Game Manager Singleton

    public int shellDamage = 20;
    public int shellTimeForExistence = 3;

    [HideInInspector]
    public List<GameObject> players;   // Holds all players in the game
    [HideInInspector]
    public List<GameObject> Emenies;   // Holds all Emenies in the game

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

    }
}
