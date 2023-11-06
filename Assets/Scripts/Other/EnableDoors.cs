using System.Collections;
using System.Collections.Generic;
using other;
using UnityEngine;
using DG.Tweening;

public class EnableDoors : MonoBehaviour
{
    public InterectableTrigger interectableTriggerDir;
    public InterectableTrigger interectableTriggerEsq;
    public bool isReseted  = false;
    public bool isChanged = false;
    public Light TopLight;
    public Color LightAfterColor;
    public DoorCloseAndOpen Door1;
    public DoorCloseAndOpen Door2;
    public Animator _doorAnim;
    private int AlavancaCount;
    private AudioSource _botAudio;
    public AudioSource _alavSucessoAudio;
    // Start is called before the first frame update
    void Start()
    {
        _botAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReseted && !isChanged)
        {
            isReseted = false;
            isChanged = true;
            _doorAnim.SetTrigger("CanOpen");
            _botAudio.Play();

        }
        if((interectableTriggerDir.isReseted && interectableTriggerEsq.isReseted) && !isReseted){
            Debug.Log("Portas destrancadas");
            _alavSucessoAudio.Play();
            isReseted = true;
            Door1.hasBeenReseted = true;
            Door2.hasBeenReseted = true;
            TopLight.DOColor(LightAfterColor,.5f);
        }
    }

    public void IncreaseCounter(){
        AlavancaCount++;
    }

    


}
