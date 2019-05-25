﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        foreach (Transform child in transform) {
            GameManager.instance.SpawnPoints.Add(child);
        }
    }
}
