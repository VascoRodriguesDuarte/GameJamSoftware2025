using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer = default;
    [SerializeField] private Slider masterSlider = default;
    [SerializeField] private Slider musicSlider = default;
    [SerializeField] private Slider effectsSlider = default;

    private void Start()
    {
        // Adjust the sliders to the actual volume of the game.
        float audio;
        audioMixer.GetFloat("masterVol", out audio);
        masterSlider.value = audio;

        audioMixer.GetFloat("musicVol", out audio);
        musicSlider.value = audio;

        audioMixer.GetFloat("effectsVol", out audio);
        effectsSlider.value = audio;
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVol", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVol", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVol", volume);
    }
}