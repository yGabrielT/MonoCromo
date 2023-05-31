using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using TMPro;

public class ManuseioTemp : MonoBehaviour
{
    public float TempParado, TempCooldown;
    public float timeCrono, SlowCooldown;
    private StarterAssetsInputs _input;
    [SerializeField]
    public static bool timeToggle = false, startcooldown;
    [SerializeField]
    private float timeScale = .25f, defaultTimeScale = 1f, defaultFixedDeltaTime = 0.02f;

    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }
    void Start()
    {
        SlowCooldown = TempCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Ativar slow down após cooldown de 10 secs
        if (_input.slow)
        {
            _input.slow = false;
            if (timeToggle == false && !startcooldown)
            {
                timeToggle = true;
                SlowCooldown = 0;
                Time.timeScale = timeScale;
            }
        }

        // desativar slow down após 10 secs
        if (timeCrono >= TempParado && timeToggle == true)
        {
            
            timeCrono = 0;
            timeToggle = false;
            Time.timeScale = defaultTimeScale;
            _input.slow = false;
            startcooldown = true;
        }
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

        //iniciar cronometro do slow
        if (timeToggle)
        {
            timeCrono += Time.unscaledDeltaTime;
        }
        //iniciar e encerrar cooldown do slow
        if (startcooldown)
        {
            SlowCooldown += Time.unscaledDeltaTime;
        }
        if (SlowCooldown >= TempCooldown)
        {
            startcooldown = false;
        }

    }



}
