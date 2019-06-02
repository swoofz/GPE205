using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public int rows;                // Map Width
    public int cols;                // Map Height
    public int mapSeed;             // Random Seed
    public bool isMapOfTheDay;      // Want map of the day?
    public bool randomMap;          // Want random map?

    private float roomWidth = 50.0f;    // Room width size
    private float roomHeight = 50.0f;   // Room height size
    private Room[,] grid;               // Place to store our map grid

    public GameObject[] gridPrefabs;    // Room prefabs


    public void SetRandomSeed() {
        if (isMapOfTheDay) {
            // Set our seed
            UnityEngine.Random.InitState(DateToInt(DateTime.Now.Date));
        }

        if (randomMap) {
            UnityEngine.Random.InitState(DateToInt(DateTime.Now) * mapSeed);
        }
    }

    public void ResetRandomSeed() {
        // Reset the Random for every other things that is using random
        UnityEngine.Random.InitState(System.Environment.TickCount);
    }

    public void GenerateGrid() {
        // Clear out the grid - "which column" is our X, "which row" is our Y
        grid = new Room[cols,rows];

        // For each row...
        for(int i = 0; i < rows; i++) {
            // For each column in that row
            for(int j = 0; j < cols; j++) {
                // Figure out the location
                float xPos = roomWidth * j;
                float zPos = roomHeight * i;
                Vector3 newPosition = new Vector3(xPos, 0.0f, zPos);

                // Create a new Grid at the appropriate location
                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                // Set it parent
                tempRoomObj.transform.parent = transform;

                // Give it a meaningful name
                tempRoomObj.name = "Room_" + j + "," + i;

                // Get the Room Object
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                // Open the doors
                // If we are on the bottom row, open north door
                if(i == 0) {
                    tempRoom.doorNorth.SetActive(false);
                } else if (i == rows -1) {
                    // otherwise, if on the top row, open south door
                    tempRoom.doorSouth.SetActive(false);
                } else {
                    // otherwise, we are in the middle open both doors
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }

                // if we are on the first column, open the east door
                if(j == 0) {
                    tempRoom.doorEast.SetActive(false);
                } else if (j == cols -1) {
                    // otherwise, if we are on the last column row, open the west door
                    tempRoom.doorWest.SetActive(false);
                } else {
                    // otherwise, we are in the middle, so open both doors
                    tempRoom.doorWest.SetActive(false);
                    tempRoom.doorEast.SetActive(false);
                }

                // Save it to the grid
                grid[j, i] = tempRoom;
            }
        }
    }

    // Funtion: RANDOM_ROOM_PREFAB
    // Return a random room
    public GameObject RandomRoomPrefab() {
        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }

    // Function: DATE_TO_INT
    // Convert Datae to an int
    public int DateToInt(DateTime dateToUse) {
        // Add our date up and return it
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }

    // Function: LOAD_NEXT_RANDOM_MAP
    // Load another Random Map to Show that the map is changing.
    public void LoadNextRandomMap() {
        // Clear out the current rooms
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        // Spawn all players in a new spot
        GameManager.instance.Spawn();
    }

    public void ClearMap() {
        // Clear out the current rooms
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
}
