using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioSlider : MonoBehaviour
{
    public Slider slider;
    public AudioMixer mixer;
    public string SliderName;
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(SliderName, 0.75f);
    }
    public void SetLevel(float SliderValue)
    {
        mixer.SetFloat(SliderName, Mathf.Log10(SliderValue) * 20);
        PlayerPrefs.SetFloat(SliderName, SliderValue);
    }
}
