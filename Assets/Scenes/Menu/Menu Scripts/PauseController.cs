using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static bool estaPausado = false;

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject primeiraTelaUI;
    [SerializeField] private GameObject ajustesUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("clicou");
            if (estaPausado)
            {
                Voltar();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Pausar();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void Pausar()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        estaPausado = true;
    }

    public void Voltar()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        estaPausado = false;
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
