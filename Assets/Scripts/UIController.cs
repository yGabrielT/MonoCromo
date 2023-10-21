using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject menuOpcoes;
    [SerializeField] private GameObject menuControles;
    [SerializeField] private GameObject menuCreditos;
    [SerializeField] private string NomeDaCena;
    [SerializeField] private float delay;
    [SerializeField] private TransitionSettings set;

    public void IniciarGame()
    {
        TransitionManager.Instance().Transition(NomeDaCena, set, delay);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
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

    public void AbrirControles()
    {
        menuOpcoes.SetActive(false);
        menuControles.SetActive(true);
    }

    public void FecharControles()
    {
        menuControles.SetActive(false);
        menuOpcoes.SetActive(true);
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
