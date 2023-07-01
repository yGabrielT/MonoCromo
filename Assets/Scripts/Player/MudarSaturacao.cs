using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MudarSaturacao : MonoBehaviour
{
    public Volume volume;
    private float SatCor;
    private float t = 0f;
    private ColorAdjustments colorAdjustments; 

    private void Start()
    {
        // Obtem o componente Color Adjustments do volume
        volume.profile.TryGet(out colorAdjustments);
    }

    private void Update()
    {
        // Exemplo de ativacao da saturacao
        if (ManuseioTemp.timeToggle)
        {
            t += Time.unscaledDeltaTime;
            SatCor = Mathf.Lerp(-100, 0, t);

            colorAdjustments.hueShift.value = -123;
            if(t < 1f)
            {
                
                colorAdjustments.saturation.value = SatCor;
            }
        }
        else
        {
            t = 0f;
            SatCor = 0f;
            colorAdjustments.hueShift.value = 0;
        }
    }
}
