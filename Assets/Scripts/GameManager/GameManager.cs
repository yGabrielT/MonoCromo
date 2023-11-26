using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using EasyTransition;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private string NomeDaCena;
    [SerializeField] private float delay;

    [SerializeField] private TransitionSettings set;

    private TransitionManager _transition;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        
    }

    void Start(){
        _transition = TransitionManager.Instance();
    }


    public void AvancarCena(int numCena)
    {
        Time.timeScale = 1f;
        _transition.Transition(numCena, set, delay);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void VoltarMenuGame()
    {
        _transition.Transition(0, set, delay);
    }

    public void ResetarFaseAtual(){
        _transition.Transition((SceneManager.GetActiveScene().buildIndex), set, delay);
    }

    

}


