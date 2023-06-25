using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public TMP_Text textoFPS;
    public float cool;
    private float tempo, refresh, frameRate;
    [SerializeField]
    private Image image;
    private StarterAssetsInputs _input;



    private void Awake()
    {

        _input = GetComponent<StarterAssetsInputs>();
    }

    void Start()
    {
        image.fillAmount = 0;
    }



    // Update is called once per frame
    void Update()
    {
        imagemTempo();
        atualizandoFPS();
    }

    private void imagemTempo()
    {
        //Para o tempo
        if (_input.slow)
        {
            Debug.Log("FUNCIONA");
            image.fillAmount = 1;
        }


        // Cooldown
        if (!_input.slow)
        {
            image.fillAmount -= 1 / cool * Time.deltaTime;


            // Termino do cooldonw
            if (image.fillAmount <= 0)
            {
                image.fillAmount = 0;
            }
        }
    }

    private void atualizandoFPS()
    {
        float timelapse = Time.smoothDeltaTime;
        tempo = tempo <= 0 ? refresh : tempo -= timelapse;

        if (tempo <= 0 ) frameRate = (int)(1f / timelapse);
        textoFPS.SetText(frameRate.ToString("0") + " FPS");
        
    }
}
