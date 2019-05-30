using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public Text text;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Save() {
        PlayerPrefs.SetString("TextData", text.text);
        PlayerPrefs.Save();
    }

    public void Load() {
        text.text = PlayerPrefs.GetString("TextData");
    }
}
