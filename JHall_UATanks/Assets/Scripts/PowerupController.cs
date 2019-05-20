using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour {

    public List<Powerup> powerups;

    private TankData data;

    // Start is called before the first frame update
    void Start() {
        powerups = new List<Powerup>();
        data = GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void Add(Powerup powerup) {
        powerup.OnActivate(data);
        powerup.OnDeactivate(data);
    }
}
