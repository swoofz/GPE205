using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShowScores : MonoBehaviour {

    public Font myFont;
    public int fontSize = 26;
    public bool highScores = false;

    private Transform tf;
    private bool ranOnce;

    void Awake() {
        tf = GetComponent<Transform>();
        ranOnce = false;
    }

    // Update is called once per frame
    void Update() {
        if(!GameManager.instance.gameIsRunning && !ranOnce) {
            if (highScores) {
                // Show High Scores
                ClearScoreBoard();
                ShowHighScores();
            } else {
                ClearScoreBoard();
                ShowPlayerScore(8, 70);
                ShowPlayerScore(9, 20);
            }
            ranOnce = true;
        }
    }

    void ShowPlayerScore(int index, int yPos) {
        int amountOfScores = GameManager.instance.scores.Count;

        if (amountOfScores > index) {
            string name = GameManager.instance.scores[index].name;
            float score = GameManager.instance.scores[index].score;
            string text = name + ":";

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
            GameManager.instance.scores[i].id = i;
            SaveManager.SaveScore("#" + i, GameManager.instance.scores[i]);
        }
    }

    void AddTextComponent(GameObject go) {
        Text goText = go.AddComponent<Text>();
        goText.font = myFont;
        goText.fontSize = fontSize;
        goText.color = Color.black;
        goText.alignment = TextAnchor.UpperLeft;
        goText.horizontalOverflow = HorizontalWrapMode.Overflow;
    }

    void ChangeText(GameObject go, string newText) {
        Text goText = go.GetComponent<Text>();
        goText.text = newText;
    }

    void SetTextPosition(GameObject textObj, Vector3 pos) {
        RectTransform go = textObj.GetComponent<RectTransform>();
        go.position = pos;
    }
    
    void ClearScoreBoard() {
        foreach(Transform removeObj in tf) {
            Destroy(removeObj.gameObject);
        }
    }

    public void RanAgain() {
        ranOnce = false;
    }

    void CreateTextObject(string name) {
        GameObject textObj = new GameObject(name, typeof(RectTransform), typeof(Text));
    }
}
