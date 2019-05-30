using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ScoreData : IComparable<ScoreData> {

    public float score;
    public string name;

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


    /* Notes:
     *  - List<ScoreData> scores;
     *  - // Add scores to list
     *  - scores.Sort();
     *  
     *  // Limit how many scores see
     *  scores = scores.GetRange(0,3);
     *  
     *  // High scores
     *  scores.Reverse();  // Do before trim scores down
     */
}
