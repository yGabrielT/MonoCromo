using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void vidaMaxima(int vida)
    {
        slider.maxValue = vida;
        slider.value = vida;
    }

    public void atualizarVida(int vida)
    {
        slider.value = vida;
    }

}
