using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject menuOpcoes;
    [SerializeField] private GameObject menuCreditos;

    public void IniciarGame()
    {
        SceneManager.LoadScene(1);
    }

    public void AbrirOpcoes()
    {
        menuPrincipal.SetActive(false);
        menuOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        menuPrincipal.SetActive(true);
        menuOpcoes.SetActive(false);
    }

    public void AbrirCreditos()
    {
        menuPrincipal.SetActive(false);
        menuCreditos.SetActive(true);
    }

    public void FecharCreditos()
    {
        menuPrincipal.SetActive(true);
        menuCreditos.SetActive(false);
    }

    public void Sair()
    {
        Application.Quit();
    }
}
