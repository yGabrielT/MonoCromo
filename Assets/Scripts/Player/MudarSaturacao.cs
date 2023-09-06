using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class MudarSaturacao : MonoBehaviour
{
    public Volume volume;
    private float SatCor;
    [SerializeField] private float HueCor;
    [SerializeField] private float lensIntensity = -0.8f;
    private float t = 0f;
    private ColorAdjustments colorAdjustments; 
    private LensDistortion lens;
    private bool isChangeNow;

    private void Start()
    {
        // Obtem o componente Color Adjustments do volume
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out lens);
    }

    private void Update()
    {
        // Exemplo de ativacao da saturacao
        if (ManuseioTemp.timeToggle)
        {
            if(!isChangeNow){
                isChangeNow = true;
                DOVirtual.Float(0f,lensIntensity,.01f, v => lens.intensity.value = v).SetEase(Ease.InBounce).OnComplete(() => DOVirtual.Float(lensIntensity,0,.1f, v => lens.intensity.value = v).SetEase(Ease.OutBounce));
                //DOVirtual.Float(lensIntensity,0,.4f, v => lens.intensity.value = v).SetEase(Ease.InOutExpo);
            }
            
            Debug.Log("Mudando tempo");
            
            t += Time.unscaledDeltaTime;
            SatCor = Mathf.Lerp(-100, 0, t);

            colorAdjustments.hueShift.value = -HueCor;
            if(t < 1f)
            {
                
                colorAdjustments.saturation.value = SatCor;
            }
        }
        else
        {
            Debug.Log("Fora do tempo");
            
            
            t = 0f;
            SatCor = 0f;
            colorAdjustments.hueShift.value = 0;
            if(isChangeNow){
                isChangeNow = false;
                DOVirtual.Float(0f,lensIntensity,.1f, v => lens.intensity.value = v).SetEase(Ease.InBounce).OnComplete(() => DOVirtual.Float(lensIntensity,0,.1f, v => lens.intensity.value = v).SetEase(Ease.OutBounce));
            }
        }

        
    }




}
