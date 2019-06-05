using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject gui;              // Get our GUI
    public GameObject startMenu;        // Get our Start Menu
    public GameObject optionsMenu;      // Get our Option Meun
    public GameObject GameOverMenu;     // Get our GameOverMenu
    public MapGenerator mapGenerator;   // Get our Map Generator

    private bool isPlayer1 = true;      // if this is player 1
    private bool toggleOne = false;     // if we already toggled
    private int players = 1;            // Number of player in the game
    private Text myText;                // store a text value

    // Function: IS_PLAYER_ONE
    // check if this is player 1
    public void IsPlayerOne(bool player) {
        isPlayer1 = player;
    }

    // Function: GET_INPUT_FIELD_TEXT
    // Get our players name
    public void GetInputFieldText(InputField inputField) {
        if (isPlayer1) {
            GameManager.instance.playerNames[0] = inputField.text;
        } else {
            GameManager.instance.playerNames[1] = inputField.text;
        }
    }

    // Function: START_GAME
    // Start up our game
    public void StartGame() {
        // Reset everything to make sure every is new
        GameManager.instance.Reset();
        DeactiveMenus();
        gui.SetActive(false);                           // Deactivate our GUI
        mapGenerator.SetRandomSeed();                   // Set random seed for our map
        mapGenerator.GenerateGrid();                    // Generate our map
        mapGenerator.ResetRandomSeed();                 // Reset our random seed 

        // Spawn all player and AIs
        GameManager.instance.SpawnPlayers(players); 
        GameManager.instance.SpawnAI();
        GameManager.instance.Spawn();
        GameManager.instance.waitForLoadTimer = 4f;
    }

    // Function: OPEN_MENU
    // Open a given menu
    public void OpenMenu(GameObject menu) {
        if (menu.activeSelf) {
            menu.SetActive(false);
        } else { 
            // Deactivate any menu active
            DeactiveMenus();
            menu.SetActive(true);
        }
    }

    // Function: GOTO_MAIN_MENU
    // Go back to main menu
    public void GoToMainMenu() {
        gui.SetActive(true);
    }

    // Function: TOGGLE
    // Only one toggle can be on between two toggles
    public void Toggle(Toggle target) {
        if(!toggleOne) {
            toggleOne = true;
            target.isOn = !target.isOn;
            return;
        }

        toggleOne = false;
    }

    // Function: IS_RANDOM_MAP
    // Change our what type of map generation we want
    public void IsRandomMap(Toggle toggle) {
        if (toggle.isOn) {
            mapGenerator.randomMap = true;
            mapGenerator.isMapOfTheDay = false;
        } else {
            mapGenerator.isMapOfTheDay = true;
            mapGenerator.randomMap = false;

        }
    }

    // Function: IS_MULTIPLAYER
    // Check if we want multiplayer
    public void IsMultiplayer(Toggle toggle) {
        if(toggle.isOn) {
            // True
            players = 2;
        } else {
            // False
            players = 1;
        }
    }

    // Function: GET_TEXT_TO_CHANGE
    // Set our text to use to change later
    public void GetTextToChange(Text text) {
        myText = text;
    }

    // Function: UPDATE_TEXT
    // Use our text that got earlier to use while updating our slide value
    public void UpdateText(Slider slider) {
        myText.text = "" + slider.value;
    }

    // Function: SAVE_SOUND_VOLUME
    // Save our save setting
    public void SaveSoundVolume() {
        AudioManager.instance.GetComponent<AudioSource>().volume = AudioManager.instance.volume("MenuMusic");
        PlayerPrefs.SetFloat("menuMusic", AudioManager.instance.MusicVolume.value);
        PlayerPrefs.SetFloat("FXsound", AudioManager.instance.FX.value);
    }

    // Function: SHOW_GAMEOVER_SCREEN
    // Clear game and show game over screen
    public void ShowGameOverScreen() {
        mapGenerator.ClearMap();
        gui.SetActive(true);
        GameOverMenu.SetActive(true);
    }
    
    // Function: RESTART_GAME
    // Restart the game using the same value as just played with
    public void RestartGame() {
        GameManager.instance.Reset();
        StartGame();
    }

    // Function: QUIT_GAME
    // Close application
    public void QuitGame() { 
        Application.Quit();
    }

    // Function: PLAY_BUTTON_SOUND
    public void PlayButtonSound() {
        AudioSource.PlayClipAtPoint(AudioManager.instance.GetClip("Buttons"), Vector3.zero, AudioManager.instance.volume("Buttons"));
    }

    // Function: DEACTIVE_MENUS
    // Deactive all menus expect main
    void DeactiveMenus() {
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        GameOverMenu.SetActive(false);
    }

}
