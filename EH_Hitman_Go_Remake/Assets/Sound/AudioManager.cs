using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Singleton")]
    private static AudioManager _instance;
    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindAnyObjectByType<AudioManager>();
            if (_instance == null)
                Debug.LogError("Audio Manager not found, can't create singleton object");
            return _instance;
        }
    }

    public AudioMixer mixer;
    private AudioSource musicSource;
    public GameObject SFXSource;

    public AudioClip BGM;

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
        PlayMusicTrack(BGM);
    }

    public void PlayMusicTrack(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        AudioSource currentSFX = GameObject.Instantiate(SFXSource).GetComponent<AudioSource>();
        currentSFX.clip = clip;
        currentSFX.Play();
        Destroy(currentSFX.gameObject, clip.length);
    }

    public void PlaySFXRanPitch(AudioClip clip, float randomPitch)
    {
        AudioSource currentSFX = GameObject.Instantiate(SFXSource).GetComponent<AudioSource>();
        currentSFX.pitch = Random.Range(1 - randomPitch, 1 + randomPitch);
        currentSFX.clip = clip;
        currentSFX.Play();
        Destroy(currentSFX.gameObject, clip.length);
    }

    public void SetVolume(string chosenMixer, float newVolume)
    {
        mixer.SetFloat(chosenMixer, ConvertToDecibel(newVolume));
    }
    public float ConvertToDecibel(float value)
    {
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
    }
}