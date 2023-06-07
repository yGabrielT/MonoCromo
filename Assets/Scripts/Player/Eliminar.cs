using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Eliminar : MonoBehaviour
{
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
    }

    public void TomarDano(int dano)
    {
        this.vida -= dano;
    }
}
