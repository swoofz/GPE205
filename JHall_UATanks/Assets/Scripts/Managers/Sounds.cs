using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sounds {
    public enum SoundType { Music, FX };

    public string name;
    public SoundType type;

    [Range(0f, 1f)]
    public float volume;
    public AudioClip clip;

}
