using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sounds {
    // Background Music / FX sounds
    public enum SoundType { Music, FX };

    public string name;         // find clip by this name
    public SoundType type;      // Type of sound we want to be classed wit

    [Range(0f, 1f)]
    public float volume;        // volume of clip to be played at
    public AudioClip clip;      // audio clip

}
