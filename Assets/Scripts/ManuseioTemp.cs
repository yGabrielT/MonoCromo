using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManuseioTemp : MonoBehaviour
{
    [SerializeField]
    private float timeCrono, SlowCooldown;
    private InputManager inputManager;
    public static bool timeToggle = false;
    private bool startcooldown;
    private float timeScale = .25f, defaultTimeScale = 1f, defaultFixedDeltaTime = 0.02f;
 
    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        SlowCooldown = 10;
    }

    // Update is called once per frame
    void Update()
    {
        // Ativar slow down após cooldown de 10 secs
        if (inputManager.isSlow && timeToggle == false && !startcooldown)
        {
            timeToggle = true;
            SlowCooldown = 0;
            Time.timeScale = timeScale;
        }
        // desativar slow down após 10 secs
        if (timeCrono >= 5 && timeToggle == true)
        {

            timeCrono = 0;
            timeToggle = false;
            Time.timeScale = defaultTimeScale;
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
        if (SlowCooldown >= 10)
        {
            startcooldown = false;
        }


    }

}
