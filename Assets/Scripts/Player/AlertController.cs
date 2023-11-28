using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertController : MonoBehaviour
{
    
    [SerializeField] private GameObject alertBox;
    [SerializeField] private Animator alertaAnimator;
    public string textoAplicado; 
    [SerializeField] private TMP_Text alertaTxt;

    // Update is called once per frame
    void Start(){
        
    }
    private void OnTriggerEnter(Collider colisaoInstrucao)
    {
        Notificacao();
        
    }

    public void Notificacao(){
        alertBox.SetActive(false);
        alertBox.SetActive(true);
        
        alertaAnimator.Play("alerta");
        alertaTxt.text = textoAplicado;
    }
}
