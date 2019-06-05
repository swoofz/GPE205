using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    public Slider MusicVolume;
    public Slider FX;
    public Sounds[] sounds;

    private float changeVolumeBy;
    private AudioSource backgroundAudio;

    void Awake() {
        changeVolumeBy = 1;
        // Attach to Gamemanage so should never have two
        instance = this;
        backgroundAudio = GetComponent<AudioSource>();
    }

    void Start() {
        // Load our sound setting
        LoadSoundVolume();
    }

    public bool HaveClip(string name) {
        foreach (Sounds sound in sounds) {
            if (name == sound.name) {
                return true;
            }
        }
        return false;
    }

    public AudioClip GetClip(string name) {
        foreach(Sounds sound in sounds) {
            if(name == sound.name) {
                return sound.clip;
            }
        }

        Debug.LogWarning("No Sound with that name was found");
        return null;
    }

    public float volume(string name) {
        foreach (Sounds sound in sounds) {
            if (name == sound.name) {
                if(sound.type == Sounds.SoundType.FX) {
                    changeVolumeBy = FX.value / 100;
                } else if (sound.type == Sounds.SoundType.Music) {
                    changeVolumeBy = MusicVolume.value / 100;
                }

                return sound.volume * changeVolumeBy;
            }
        }

        return 1;
    }

    public void LoadSoundVolume() {
        MusicVolume.value = PlayerPrefs.GetFloat("menuMusic", 100f);
        FX.value = PlayerPrefs.GetFloat("FXsound", 100f);

        backgroundAudio.clip = GetClip("MenuMusic");
        backgroundAudio.volume = volume("MenuMusic");
        backgroundAudio.Play();
    }

    public void ChangeBackgroundAudio(string name) {
        backgroundAudio.clip = GetClip(name);
        backgroundAudio.Play();
    }
}
