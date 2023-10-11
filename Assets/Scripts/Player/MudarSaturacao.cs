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
    public float timerBeforeEnds =1f;
    [SerializeField] private float HueCor;
    [SerializeField] private float lensIntensity = -0.8f;
    private float t = 0f;
    private ColorAdjustments colorAdjustments; 
    private LensDistortion lens;
    private bool isChangeNow;

    private ManuseioTemp _manuseio;

    private float hueStart;
    private float satStart;

    private void Start()
    {
        _manuseio = GetComponent<ManuseioTemp>();
        // Obtem o componente Color Adjustments do volume
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out lens);

        
        satStart = colorAdjustments.saturation.value;
        hueStart = colorAdjustments.hueShift.value;
    }

    private void Update()
    {
        // Exemplo de ativacao da saturacao
        if (_manuseio.timeToggle)
        {
            
            if(!isChangeNow){
                isChangeNow = true;
                DOVirtual.Float(0f,lensIntensity,.01f, v => lens.intensity.value = v).SetEase(Ease.InBounce).OnComplete(() => DOVirtual.Float(lensIntensity,0,.1f, v => lens.intensity.value = v).SetEase(Ease.OutBounce));
                //DOVirtual.Float(lensIntensity,0,.4f, v => lens.intensity.value = v).SetEase(Ease.InOutExpo);
            }
            

            t += Time.unscaledDeltaTime;
            SatCor = Mathf.Lerp(-100, satStart, t);

            colorAdjustments.hueShift.value = -HueCor;
            if(t < 1f)
            {
                
                colorAdjustments.saturation.value = SatCor;
            }
        }
        else
        {

            t = 0f;
            SatCor = satStart;
            colorAdjustments.hueShift.value = hueStart;

            if(isChangeNow){
                isChangeNow = false;
                DOVirtual.Float(0f,lensIntensity,.25f, v => lens.intensity.value = v).SetEase(Ease.InBounce).OnComplete(() => DOVirtual.Float(lensIntensity,0,.25f, v => lens.intensity.value = v).SetEase(Ease.OutBounce));
            }
            
           
        }

        

        
    }
}
