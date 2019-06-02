﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject gui;
    public GameObject startMenu;
    public GameObject optionsMenu;
    public GameObject GameOverMenu;
    public MapGenerator mapGenerator;

    private bool isPlayer1 = true;
    private bool toggleOne = false;
    private int players = 1;
    private Text myText;


    public void IsPlayerOne(bool player) {
        isPlayer1 = player;
    }

    public void GetInputFieldText(InputField inputField) {
        if (isPlayer1) {
            GameManager.instance.playerNames[0] = inputField.text;
        } else {
            GameManager.instance.playerNames[1] = inputField.text;
        }
    }

    public void StartGame() {
        DeactiveMenus();
        gui.SetActive(false);
        mapGenerator.SetRandomSeed();
        mapGenerator.GenerateGrid();
        mapGenerator.ResetRandomSeed();
        GameManager.instance.SpawnPlayers(players);
        GameManager.instance.SpawnAI();
        GameManager.instance.Spawn();
        GameManager.instance.waitForLoadTimer = 4f;
    }

    public void OpenMenu(GameObject menu) {
        if (menu.activeSelf) {
            menu.SetActive(false);
        } else { 
            DeactiveMenus();
            menu.SetActive(true);
        }
    }

    public void GoToMainMenu() {
        gui.SetActive(true);
    }

    public void Toggle(Toggle target) {
        if(!toggleOne) {
            toggleOne = true;
            target.isOn = !target.isOn;
            return;
        }

        toggleOne = false;
    }

    public void IsRandomMap(Toggle toggle) {
        if (toggle.isOn) {
            mapGenerator.randomMap = true;
            mapGenerator.isMapOfTheDay = false;
        } else {
            mapGenerator.isMapOfTheDay = true;
            mapGenerator.randomMap = false;

        }
    }

    public void IsMultiplayer(Toggle toggle) {
        if(toggle.isOn) {
            // True
            players = 2;
        } else {
            // False
            players = 1;
        }
    }

    public void GetTextToChange(Text text) {
        //TODO:: Change Value to Actual Value
        myText = text;
    }

    public void UpdateText(Slider slider) {
        myText.text = "" + slider.value;
    }

    public void SaveSoundVolume() {
        // TODO:: Write UI Save volume
    }

    public void ShowGameOverScreen() {
        gui.SetActive(true);
        GameOverMenu.SetActive(true);
    }
    
    public void RestartGame() {
        mapGenerator.ClearMap();
        GameManager.instance.Reset();
        StartGame();
    }

    public void QuitGame() {
        // Save then quit
        // TODO:: Save code
        Application.Quit();
    }

    void DeactiveMenus() {
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        GameOverMenu.SetActive(false);
    }

}
