using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertController : MonoBehaviour
{
    [SerializeField] private Collider colisao;
    [SerializeField] private GameObject alertBox;
    [SerializeField] private Animator alertaAnimator;
    public string textoAplicado; 
    [SerializeField] private TMP_Text alertaTxt;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider colisaoInstrucao)
    {
        alertBox.SetActive(false);
        alertBox.SetActive(true);
        Destroy(colisao);
        alertaAnimator.Play("alerta");
        alertaTxt.text = textoAplicado;
        
    }
}
