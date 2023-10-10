using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloseAndOpen : MonoBehaviour
{
    private Animator _anim;
    public bool hasBeenReseted = false;
    public Light SpotLight;
    public Light PointLight;
    public Color AfterResetColor;
    private bool isChangedColor = false;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hasBeenReseted && !isChangedColor)
        {
            isChangedColor = true;
            SpotLight.color = AfterResetColor;
            PointLight.color = AfterResetColor;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !hasBeenReseted)
        {
            _anim.SetTrigger("IsPlayerClose");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenReseted)
        {
            _anim.SetTrigger("PlayerLeft");
        }
    }
}
