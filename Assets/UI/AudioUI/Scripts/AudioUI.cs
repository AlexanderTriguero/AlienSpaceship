using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider sliderVolMaster;
    [SerializeField] Slider sliderVolMusica;
    [SerializeField] Slider sliderVolSonido;


    private void Start()
    {
        float masterValue;
        float musicaValue;
        float sonidoValue;
        mixer.GetFloat("VolMaster",out masterValue);
        sliderVolMaster.value = DecibelToLinear(masterValue);
        mixer.GetFloat("VolMusica", out musicaValue);
        sliderVolMusica.value = DecibelToLinear(musicaValue);
        mixer.GetFloat("VolSonido", out sonidoValue);
        sliderVolSonido.value = DecibelToLinear(sonidoValue);
    }
    public void setVolMaster(float sliderValue)
    {
        mixer.SetFloat("VolMaster", LinearToDecibel(sliderValue));
    }
    public void setVolMusica(float sliderValue)
    {
        mixer.SetFloat("VolMusica", LinearToDecibel(sliderValue));
    }
    public void setVolSonido(float sliderValue)
    {
        mixer.SetFloat("VolSonido", LinearToDecibel(sliderValue));
    }

    //https://answers.unity.com/questions/283192/how-to-convert-decibel-number-to-audio-source-volu.html
    private float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }
    private float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20.0f);

        return linear;
    }
}
