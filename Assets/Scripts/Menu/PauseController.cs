using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static bool estaPausado = false;

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject primeiraTelaUI;
    [SerializeField] private GameObject ajustesUI;

    private Personagem _pers;

    private DialogueManager _diag;
    

    private void Start() 
    {
        _pers = GameObject.FindGameObjectWithTag("Player").GetComponent<Personagem>();
        _diag = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueManager>();

    }
    // Update is called once per frame
    void Update()
    {
        if(estaPausado){
            _pers._input.interact = false;
        }else{
            
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("clicou");
            if (estaPausado)
            {
                Voltar();
                
            }
            else
            {
                Pausar();
                
            }
        }
    }

    public void Pausar()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        estaPausado = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _pers.ativarControles = false;
        
    }

    public void Voltar()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        estaPausado = false;
        if (!_diag._alreadyChatitng){
            _pers.ativarControles = true;
        }
        
    }

    public void AbrirAjustes()
    {
        ajustesUI.SetActive(true);
        primeiraTelaUI.SetActive(false);
    }

    public void FecharAjustes()
    {
        ajustesUI.SetActive(false);
        primeiraTelaUI.SetActive(true);
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene(0);
    }
}
