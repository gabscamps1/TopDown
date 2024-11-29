using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider soundFXSlider;
    [SerializeField] private Slider musicSlider;

    GameData gameData;
    private void Start()
    {
        if (GameManager.instance != null)
        {
            gameData = GameManager.instance.gameData;

            audioMixer.SetFloat("masterVolume", gameData.masterVolume);
            audioMixer.SetFloat("soundFXVolume", gameData.soundFXVolume);
            audioMixer.SetFloat("musicVolume", gameData.musicVolume);


        }




    }

    void Update() {
        float value;
        if (masterSlider != null)

            if (audioMixer.GetFloat("masterVolume", out value))
            {
                //print (value);
                //masterSlider.value = value;
            }

        if (soundFXSlider != null)
            if (audioMixer.GetFloat("soundFXVolume", out value))
        {
            //print(value);
            //soundFXSlider.value = value;
        }

        if (musicSlider != null)
            if (audioMixer.GetFloat("musicVolume", out value))
        {
            //print(value);
            //musicSlider.value = value;
        }
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
        gameData.masterVolume = level;
    }

    public void SetSoundVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
        gameData.soundFXVolume = Mathf.Log10(level) * 20f;
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        gameData.musicVolume = Mathf.Log10(level) * 20f;
    }


}
