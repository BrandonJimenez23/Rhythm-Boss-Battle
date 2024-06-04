using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Los sliders se configuran cuando el menú de opciones se abre
    }

    public void RegisterVolumeSliders(Slider masterVolumeSlider, Slider sfxVolumeSlider, Slider musicVolumeSlider)
    {
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
            masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0.5f);
            sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        }
    }

    public void ChangeMasterVolume(float sliderValue)
    {
        float volume = Remap(sliderValue, 0f, 1f, -50f, 20f);
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }


    public void ChangeSFXVolume(float sliderValue)
    {
        float volume = Remap(sliderValue, 0f, 1f, -50f, 20f);
        audioMixer.SetFloat("SfxVolume", volume);
        PlayerPrefs.SetFloat("SfxVolume", sliderValue);
    }

    public void ChangeMusicVolume(float sliderValue)
    {
        float volume = Remap(sliderValue, 0f, 1f, -50f, 20f);
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
    public void RestoreVolumes()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        ChangeMasterVolume(masterVolume);

        float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.5f);
        ChangeSFXVolume(sfxVolume);

        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        ChangeMusicVolume(musicVolume);
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
