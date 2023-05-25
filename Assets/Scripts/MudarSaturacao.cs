using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MudarSaturacao : MonoBehaviour
{
    public Volume volume; 
    private ColorAdjustments colorAdjustments; 

    private void Start()
    {
        // Obtém o componente Color Adjustments do volume
        volume.profile.TryGet(out colorAdjustments);
    }

    private void Update()
    {
        // Exemplo de ativação da saturação
        if (ManuseioTemp.timeToggle)
        {
            colorAdjustments.hueShift.value = 125; 
        }
        else
        {
            colorAdjustments.hueShift.value = 0;
        }
    }
}
