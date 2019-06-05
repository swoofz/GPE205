using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;        // Making singleton

    public Slider MusicVolume;  // Getting our slider for our Music Volume 
    public Slider FX;           // Getting our slider for FX Volume
    public Sounds[] sounds;     // Make an array of Sound objects

    private float changeVolumeBy;           // Value to change volume
    private AudioSource backgroundAudio;    // Where our backgournd music will be changed and played
    
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

    // Function: HAVE_CLIP
    // Find if there is a clip
    public bool HaveClip(string name) {
        // Look throught our array of Sounds and find a clip
        foreach (Sounds sound in sounds) {
            if (name == sound.name) {
                // Founded 
                return true;
            }
        }
        // Not Founded
        Debug.Log("No Clip found");
        return false;
    }

    // Function: GET_CLIP
    // Find the clip and get it
    public AudioClip GetClip(string name) {
        foreach(Sounds sound in sounds) {
            if(name == sound.name) {
                return sound.clip;
            }
        }

        Debug.LogWarning("No Sound with that name was found");
        return null;
    }


    // Function: VOLUME
    // Set our volume for our Sounds
    public float volume(string name) {
        foreach (Sounds sound in sounds) {
            if (name == sound.name) {
                if(sound.type == Sounds.SoundType.FX) {                 // FX Volume change
                    changeVolumeBy = FX.value / 100;
                } else if (sound.type == Sounds.SoundType.Music) {      // Background Music Change
                    changeVolumeBy = MusicVolume.value / 100;
                }

                return sound.volume * changeVolumeBy;
            }
        }

        // Didn't Find so return 1
        return 1;
    }

    // Function: LOAD_SOUND_VOLUME
    // Load in existing Volume Levels
    public void LoadSoundVolume() {
        // Get Background music and FX sound volumes
        MusicVolume.value = PlayerPrefs.GetFloat("menuMusic", 100f);
        FX.value = PlayerPrefs.GetFloat("FXsound", 100f);

        // Play a Starter clip
        backgroundAudio.clip = GetClip("MenuMusic");
        backgroundAudio.volume = volume("MenuMusic");
        backgroundAudio.Play();
    }

    // Function: CHANGE_BACKGROUDN_AUDIO
    // Change what our background audio is
    public void ChangeBackgroundAudio(string name) {
        backgroundAudio.clip = GetClip(name);
        backgroundAudio.Play();
    }
}
