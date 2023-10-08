using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicController : MonoBehaviour
{
    private int musicaAtual;
    private bool estaParada = false;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] musicas;
    [SerializeField] private TMP_Text nomeMusica;
    [SerializeField] private Slider duracaoMusica;

    // Start is called before the first frame update
    void Start()
    {
        ComecarAudio();
    }

    // Update is called once per frame
    void Update()
    {
        if (!estaParada)
        {
            duracaoMusica.value += Time.deltaTime;

            if(duracaoMusica.value >= audioSource.clip.length)
            {
                musicaAtual++;
                if (musicaAtual >= musicas.Length)
                {
                    musicaAtual = 0;
                }
                ComecarAudio();
            }
        }
    }

    public void ComecarAudio(int changeMusic = 0)
    {
        musicaAtual += changeMusic;

        if(musicaAtual >= musicas.Length)
        {
            musicaAtual = 0;
        }
        else if(musicaAtual < 0)
        {
            musicaAtual = musicas.Length - 1;
        }

        if(audioSource.isPlaying && changeMusic == 0)
        {
            return;
        }
        if (estaParada)
        {
            estaParada = false;
        }

        audioSource.clip = musicas[musicaAtual];
        nomeMusica.text = audioSource.clip.name;
        duracaoMusica.maxValue = audioSource.clip.length;
        duracaoMusica.value = 0;
        audioSource.Play();
    }

    public void PararMusica()
    {
        audioSource.Stop();
        estaParada = true;
    }
}
