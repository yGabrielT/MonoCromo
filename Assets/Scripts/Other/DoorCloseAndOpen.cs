using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorCloseAndOpen : MonoBehaviour
{
    private Animator _anim;
    public bool hasBeenReseted = false;
    public Light SpotLight;
    public Light PointLight;
    public Color AfterResetColor;
    private bool isChangedColor = false;
    private AudioSource _portAudio;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _portAudio = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        if (hasBeenReseted && !isChangedColor)
        {
            isChangedColor = true;
            SpotLight.DOColor(AfterResetColor,.5f);
            PointLight.DOColor(AfterResetColor,.5f);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !hasBeenReseted)
        {
            _anim.SetTrigger("IsPlayerClose");
            _portAudio.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenReseted)
        {
            _anim.SetTrigger("PlayerLeft");
            _portAudio.Play();
        }
    }
}
