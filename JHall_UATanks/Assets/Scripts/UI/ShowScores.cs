using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShowScores : MonoBehaviour {

    public Font myFont;                 // Font for our text
    public int fontSize = 26;           // Size of our font
    public bool highScores = false;     // Do we want highscores or just player scores

    private Transform tf;       // our transfrom
    private bool ranOnce;       // Run a piece of code once

    void Awake() {
        tf = GetComponent<Transform>();
        ranOnce = false;
    }

    // Update is called once per frame
    void Update() {
        // Game is not runing and haven't ranone
        if(!GameManager.instance.gameIsRunning && !ranOnce) {
            // Do want highscore or just player scores
            if (highScores) {
                // Show High Scores
                ClearScoreBoard();      // Clear board first
                ShowHighScores();       // Create board text
            } else {
                ClearScoreBoard();          // Clear board
                ShowPlayerScore(8, 70);     // Show player 1 score
                ShowPlayerScore(9, 20);     // Show player 2 score
            }
            ranOnce = true; // We ranonce
        }
    }

    // Function: SHOW_PLAYER_SCORE
    // Show a player score on the UI
    void ShowPlayerScore(int index, int yPos) {
        // Get the number of score are in the list
        int amountOfScores = GameManager.instance.scores.Count;

        // if our index is in range of the list
        if (amountOfScores > index) {
            // Get our player score data
            string name = GameManager.instance.scores[index].name;
            float score = GameManager.instance.scores[index].score;
            string text = name + ":";

            // Create our Player GameObject and Text to show
            GameObject player = new GameObject(name, typeof(RectTransform));
            AddTextComponent(player);
            ChangeText(player, text);
            SetTextPosition(player, new Vector3(-180, yPos, 0));
            player.transform.SetParent(tf, false);

            GameObject playerScore = new GameObject(name);
            text = score + "";
            AddTextComponent(playerScore);
            ChangeText(playerScore, text);
            SetTextPosition(playerScore, new Vector3(400, 0, 0));
            playerScore.transform.SetParent(player.transform, false);
        }
    }

    // Function: SHOW_HIGHSCORES
    // Show all the highscore that you have gotten on this pc
    void ShowHighScores() {
        // Get the top Scores in order from highest to lowest
        GameManager.instance.scores.Sort();
        GameManager.instance.scores.Reverse();
        GameManager.instance.scores = GameManager.instance.scores.GetRange(0, 8);

        // Start Position for Text
        int yPos = 70;

        // Show highscores
        for (int i = 0; i < GameManager.instance.scores.Count; i++) {
            string name = GameManager.instance.scores[i].name;
            float score = GameManager.instance.scores[i].score;
            string text = i+1 + ".\t" + name;

            GameObject highScorePlayer = new GameObject(name);
            AddTextComponent(highScorePlayer);
            ChangeText(highScorePlayer, text);
            SetTextPosition(highScorePlayer, new Vector3(-180, yPos, 0));
            highScorePlayer.transform.SetParent(tf, false);

            GameObject playerScore = new GameObject("score");
            AddTextComponent(playerScore);
            ChangeText(playerScore, score + "");
            SetTextPosition(playerScore, new Vector3(400, 0, 0));
            playerScore.transform.SetParent(highScorePlayer.transform, false);

            yPos -= 50;
            // Reset ids and list positions
            GameManager.instance.scores[i].id = i;
            SaveManager.SaveScore("#" + i, GameManager.instance.scores[i]);
        }
    }

    // Function: ADD_TEXT_COMPONENT
    // Add and text component to our object
    void AddTextComponent(GameObject go) {
        Text goText = go.AddComponent<Text>();
        goText.font = myFont;
        goText.fontSize = fontSize;
        goText.color = Color.black;
        goText.alignment = TextAnchor.UpperLeft;
        goText.horizontalOverflow = HorizontalWrapMode.Overflow;
    }

    // Function: CHANGE_TEXT
    // Change the text that is displayed
    void ChangeText(GameObject go, string newText) {
        Text goText = go.GetComponent<Text>();
        goText.text = newText;
    }

    // Function: SET_TEXT_POSITION
    // Customize where your text is displayed
    void SetTextPosition(GameObject textObj, Vector3 pos) {
        RectTransform go = textObj.GetComponent<RectTransform>();
        go.position = pos;
    }
    
    // Function: CLEAR_SCORE_BOARD
    // Clear the score board of previous score to update with new ones
    void ClearScoreBoard() {
        foreach(Transform removeObj in tf) {
            Destroy(removeObj.gameObject);
        }
    }

    // Function: RAN_AGAIN
    // Change our ranOnce value to be able to run the code again
    public void RanAgain() {
        ranOnce = false;
    }
}
