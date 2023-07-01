using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using TMPro;
    public class ManuseioTemp : MonoBehaviour
    {
        public TMP_Text TempoText;
        public float SetTempParado, SetTempCooldown;
        private float timeCrono, SlowCooldown;
        private StarterAssetsInputs _input;
        [SerializeField]
        private float timeScale = .5f;
        public static bool timeToggle = false, startcooldown;
        private float defaultTimeScale = 1f, defaultFixedDeltaTime = 0.02f;

        private void Awake()
        {
            _input = GetComponent<StarterAssetsInputs>();
        }
        void Start()
        {
            SlowCooldown = SetTempCooldown;
        }

        void Update()
        {
            PararTempo();
            AtualizarCanvas();

        }

        void PararTempo()
        {
            // Ativar slow down ap�s cooldown de 10 secs
            if (_input.slow)
            {

                if (timeToggle == false && !startcooldown)
                {
                    timeToggle = true;
                    SlowCooldown = 0;
                    Time.timeScale = timeScale;

                }
            }

            // desativar slow down ap�s 10 secs
            if (timeCrono >= SetTempParado && timeToggle == true)
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
                _input.slow = false;
                SlowCooldown += Time.unscaledDeltaTime;
                
            }
            if (SlowCooldown >= SetTempCooldown)
            {
                _input.slow = false;
                startcooldown = false;
            }
        }

        void AtualizarCanvas()
        {
            if (timeToggle)
            {
                TempoText.SetText(timeCrono.ToString("0"));
               //  Cooldown.fillAmount = 1;
            }
            if (startcooldown && !timeToggle)
            {
                TempoText.SetText(SlowCooldown.ToString("0"));
                // Cooldown.fillAmount -= 1 / SetTempCooldown * Time.deltaTime;
            }
            if (!timeToggle && !startcooldown)
            {
                TempoText.SetText("");
                // Cooldown.fillAmount = 0;
            }
        }

    



}


