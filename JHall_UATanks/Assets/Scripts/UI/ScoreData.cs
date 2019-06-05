using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ScoreData : IComparable<ScoreData> {

    public int id;              // Our unqire ID (kind of)
    public float score;         // player score
    public string name;         // player name

    // Function: COMPARE_TO
    // Compare other score to each other
    public int CompareTo(ScoreData other) {
        if(other == null) {
            return 1;
        }

        if(this.score > other.score) {
            return 1;
        }

        if(this.score < other.score) {
            return -1;
        }

        return 0;
    }
}
