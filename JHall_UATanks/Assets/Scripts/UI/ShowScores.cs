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

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(!GameManager.instance.gameIsRunning && !ranOnce) {
            if (highScores) {
                // Show High Scores
            } else {
                ClearScoreBoard();
                ShowPlayerScores();
            }
            ranOnce = true;
        }
    }

    void ShowPlayerScores() {
        int i = 1;
        foreach(ScoreData score in GameManager.instance.scores) {
            // Player Name GameObject
            GameObject player = new GameObject();
            player.name = score.name;
            AddTextComponent(player);
            ChangeText(player, score.name + ":");
            // Score GameObejct
            GameObject scoreObj = new GameObject();
            scoreObj.name = "Score";
            AddTextComponent(scoreObj);
            ChangeText(scoreObj, score.score + "");

            // Set Postion
            if(i == 1) {
                SetTextPosition(player, new Vector3(-180, 70, 0));

            } else if(i == 2) {
                SetTextPosition(player, new Vector3(-180, 20, 0));

            }
            SetTextPosition(scoreObj, new Vector3(155, 0, 0));


            // Set Parent Object
            player.transform.SetParent(tf, false);
            scoreObj.transform.SetParent(player.transform, false);
            i++;
        }
    }

    void AddTextComponent(GameObject go) {
        Text goText = go.AddComponent<Text>();
        goText.font = myFont;
        goText.fontSize = fontSize;
        goText.color = Color.black;
        goText.alignment = TextAnchor.UpperLeft;
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
}
