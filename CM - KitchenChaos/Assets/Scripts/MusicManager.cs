using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    private const string PALYER_PREFS_MUSIC_VOLUME = "MusicVloume";
    public static MusicManager Instance { get; private set; }

    private float volume = .3f;
    private AudioSource audioSource;
    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PALYER_PREFS_MUSIC_VOLUME, 0.3f);
        audioSource.volume = volume;
    }

    public void ChangeVolume() {
        volume += .1f;
        if (volume > 1f) volume = 0f;

        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PALYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume() { return volume; }
}   
