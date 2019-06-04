using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {


    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < 8; i++) {
            LoadScores("#" + i);
        }
    }

    public static void SaveScore(string name, ScoreData data) {
        // Save id, name, and score
        PlayerPrefs.SetInt(name + "id", data.id);
        PlayerPrefs.SetString(name + "name", data.name); 
        PlayerPrefs.SetFloat(name + "score", data.score);
        PlayerPrefs.Save();
    }

    public void LoadScores(string name) {
        // Create a new ScoreData to get all info on highscores
        ScoreData data = new ScoreData();

        // Get id, name, and score
        data.id = PlayerPrefs.GetInt(name + "id");
        data.name = PlayerPrefs.GetString(name + "name");
        data.score = PlayerPrefs.GetFloat(name + "score");

        // Add to our GameManager scores list
        GameManager.instance.scores.Add(data);
    }
}
