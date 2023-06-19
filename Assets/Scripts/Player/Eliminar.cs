using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Eliminar : MonoBehaviour
{
    [SerializeField] private NavMeshAgent NavMeshAgente;
    [SerializeField] private Transform Player;
    private int vida;
    public int vidaMax;
    

    private void Start()
    {
        
        vida = vidaMax;
    }
    void Update()
    {
        if(vida <= 0)
        Destroy(this.gameObject);

        NavMeshAgente.SetDestination(Player.position);

    }

    public void TomarDano(int dano)
    {
        this.vida -= dano;
    }
}
