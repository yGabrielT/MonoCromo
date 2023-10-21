using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class DataController : MonoBehaviour
{
    [Header("Mixers")]
    [SerializeField] private AudioMixer mixer;

    const string MIXER = "VolMaster";
    const string MIXER_MUSIC = "VolMusica";
    const string MIXER_SFX = "VolSFX";

    [Header("Volume Geral")]
    public Slider volumeSliderGeral = null;

    [Header("Volume Música")]
    public Slider volumeSliderMusica = null;

    [Header("Volume Efeitos")]
    public Slider volumeSliderEfeitos = null;

    [Header("DropDown")]
    public TMP_Dropdown dropdown;

    private void Start()
    {
        if (PlayerPrefs.GetFloat("VolumeGeral") == 0 && PlayerPrefs.GetFloat("VolumeMusica") == 0 && PlayerPrefs.GetFloat("VolumeEfeitos") == 0)
        {
            float valorQualquer = 0.5f;

            PlayerPrefs.SetFloat("VolumeGeral", valorQualquer);
            PlayerPrefs.SetFloat("VolumeMusica", valorQualquer);
            PlayerPrefs.SetFloat("VolumeEfeitos", valorQualquer);
        }
        else
        {
            CarregarValores();
        }
        
    }

    public void SalvarVolume()
    {
        // salva a qualidade 
        int qualidadeSelecionada = QualityChanger.qualidadeSelecionada;
        PlayerPrefs.SetInt("ValorQualidade", qualidadeSelecionada);

        // Linka os valores dos sliders
        float valorVolumeGeral = volumeSliderGeral.value;
        float valorVolumeMusica = volumeSliderMusica.value;
        float valorVolumeEfeitos = volumeSliderEfeitos.value;

        // Armazena as informações
        PlayerPrefs.SetFloat("VolumeGeral", valorVolumeGeral);
        PlayerPrefs.SetFloat("VolumeMusica", valorVolumeMusica);
        PlayerPrefs.SetFloat("VolumeEfeitos", valorVolumeEfeitos);

        CarregarValores();
    }

    void CarregarValores()
    {
        int qualidadeSelecionada = PlayerPrefs.GetInt("ValorQualidade");
        QualityChanger.qualidadeSelecionada = qualidadeSelecionada;
        dropdown.value = qualidadeSelecionada;

        float valorVolumeGeral = PlayerPrefs.GetFloat("VolumeGeral");
        volumeSliderGeral.value = valorVolumeGeral;
        mixer.SetFloat(MIXER, Mathf.Log10(valorVolumeGeral) * 20);

        float valorVolumeMusica = PlayerPrefs.GetFloat("VolumeMusica");
        volumeSliderMusica.value = valorVolumeMusica;
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(valorVolumeMusica) * 20);

        float valorVolumeEfeitos = PlayerPrefs.GetFloat("VolumeEfeitos");
        volumeSliderEfeitos.value = valorVolumeEfeitos;
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(valorVolumeEfeitos) * 20);
    }
}
