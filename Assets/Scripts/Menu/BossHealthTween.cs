using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class BossHealthTween : MonoBehaviour
{
    public Image HealthFillImage;
    private CanvasGroup _thisHealthContainer;
    public static bool ChangedLife = false;
    public Boss _boss;
    private float _vidaMaxBoss;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _vidaMaxBoss = _boss.VidaAtual;
        
        _thisHealthContainer = GetComponent<CanvasGroup>();
        _thisHealthContainer.alpha = 0f;
        _thisHealthContainer.DOFade(1f,2f).SetEase(Ease.InQuint);
    }

    // Update is called once per frame
    void Update(){
        if(ChangedLife){
            ChangedLife = true;
            HealthFillImage.DOFillAmount(_boss.VidaAtual/_vidaMaxBoss,.25f).SetEase(Ease.OutExpo);
            
        }
        if (_boss.VidaAtual <= 0){
           _thisHealthContainer.DOFade(0f,2f).SetEase(Ease.OutQuint); 
        }
    }
    
}
